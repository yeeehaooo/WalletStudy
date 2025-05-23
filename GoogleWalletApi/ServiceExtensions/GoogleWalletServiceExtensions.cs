using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.Base;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Context;
using WalletLibrary.GoogleWallet.Context.Interfaces;
using WalletLibrary.GoogleWallet.Settings;
using WalletLibrary.GoogleWallet.WalletTypes.Flight;
using WalletLibrary.Services;
using WalletLibrary.Services.Interfaces;

namespace GoogleWalletApi.ServiceCollectionExtensions
{
    /// <summary>
    /// 範例: 提供多組 Google Wallet 相關服務的 DI 註冊擴充方法, By CompanyCode 注入。
    /// </summary>
    public static class GoogleWalletServiceExtensions
    {
        /// <summary>
        /// 註冊 Google Wallet 的核心服務與處理器工廠。
        /// </summary>
        /// <param name="services">依賴注入服務集合。</param>
        /// <param name="configuration">應用程式設定。</param>
        /// <returns>回傳 IServiceCollection 已註冊的服務集合。</returns>
        public static IServiceCollection AddGoogleWalletServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // 綁定設定檔並註冊為 IOptions
            services.Configure<CompanyGoogleWalletSettings>(options =>
            {
                configuration.GetSection("GoogleWalletSettings").Bind(options);
            });

            var companySettings = configuration
                .GetSection("GoogleWalletSettings")
                .Get<CompanyGoogleWalletSettings>();
            services.AddSingleton(companySettings);

            foreach (var companySetting in companySettings.CompanySettings)
            {
                var companyCode = companySetting.Key;

                var setting = companySetting.Value;
                //Check Option
                if (string.IsNullOrWhiteSpace(setting.ServiceAccountJsonPath))
                    throw new InvalidOperationException(
                        $"CompanyCode: {companyCode}，設定檔錯誤，缺少 ServiceAccountJsonPath。"
                    );

                var credential = (ServiceAccountCredential)
                    GoogleCredential
                        .FromFile(setting.ServiceAccountJsonPath)
                        .CreateScoped(WalletobjectsService.ScopeConstants.WalletObjectIssuer)
                        .UnderlyingCredential;
                var walletService = new WalletobjectsService(
                    new BaseClientService.Initializer { HttpClientInitializer = credential }
                );

                //先建立好完整服務,子元件應該都透過Wallet Handler操作
                services.AddKeyedSingleton<IGoogleWalletContext, GoogleWalletContext>(
                    companyCode,
                    (provider, key) =>
                    {
                        return new GoogleWalletContext(setting, walletService, credential);
                    }
                );
                // 依序 DI 注入 By CompanyCode

                // Wallet Types
                // Boarding Passes
                services.AddGoogleBoardingPassesServices(companyCode);

                // Google Wallet Handler
                services.AddKeyedSingleton<IGoogleWalletHandler, GoogleWalletHandler>(
                    companyCode,
                    (provider, key) =>
                        new GoogleWalletHandler(
                            provider.GetRequiredKeyedService<IGoogleWalletContext>(key),
                            provider.GetRequiredKeyedService<
                                IWalletHandler<FlightClass, FlightObject>
                            >(key)
                        )
                );

                // Service
                services.AddKeyedSingleton<IGoogleWalletService, GoogleWalletService>(
                    companyCode,
                    (provider, key) =>
                        new GoogleWalletService(
                            provider.GetRequiredService<ILogger<GoogleWalletService>>(),
                            provider.GetRequiredKeyedService<IGoogleWalletHandler>(key)
                        )
                );
            }

            return services;
        }

        /// <summary>
        /// 註冊 Google Flight Wallet 的服務。
        /// </summary>
        /// <param name="services">依賴注入服務集合。</param>
        /// <param name="companyCode">公司代碼</param>
        /// <returns>回傳 IServiceCollection 已註冊的服務集合。</returns>
        public static IServiceCollection AddGoogleBoardingPassesServices(
            this IServiceCollection services,
            string companyCode
        )
        {
            // Flight Class Repository
            services.AddKeyedSingleton<IClassResource<FlightClass>, FlightClassResource>(
                companyCode,
                (provider, key) =>
                    new FlightClassResource(
                        provider.GetRequiredService<ILogger<FlightClassResource>>(),
                        provider
                            .GetRequiredKeyedService<IGoogleWalletContext>(companyCode)
                            .WalletobjectsService
                    )
            );

            // Flight Object Repository
            services.AddKeyedSingleton<IObjectResource<FlightObject>, FlightObjectResource>(
                companyCode,
                (provider, key) =>
                    new FlightObjectResource(
                        provider.GetRequiredService<ILogger<FlightObjectResource>>(),
                        provider
                            .GetRequiredKeyedService<IGoogleWalletContext>(key)
                            .WalletobjectsService
                    )
            );
            // Wallet
            services.AddKeyedSingleton<
                IWalletHandler<FlightClass, FlightObject>,
                BoardingPassesHandler
            >(
                companyCode,
                (provider, key) =>
                    new BoardingPassesHandler(
                        provider.GetRequiredKeyedService<IClassResource<FlightClass>>(key),
                        provider.GetRequiredKeyedService<IObjectResource<FlightObject>>(key)
                    )
            );

            return services;
        }
    }
}
