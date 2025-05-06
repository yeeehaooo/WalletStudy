using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Walletobjects.v1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletLibrary.GoogleWallet.Base;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Context;
using WalletLibrary.GoogleWallet.Flight;
using WalletLibrary.GoogleWallet.Settings;

namespace WalletLibrary.GoogleWallet.ServiceExtensions
{
    /// <summary>
    /// 提供 Google Wallet 相關服務的 DI 註冊擴充方法。
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
            this IServiceCollection services, IConfiguration configuration
        )
        {
            // 綁定設定檔並註冊為 IOptions
            services.Configure<CompanyGoogleWalletSettings>(options =>
            {
                configuration.GetSection("GoogleWalletSettings").Bind(options);
            });

            var companySettings = configuration.GetSection("GoogleWalletSettings").Get<CompanyGoogleWalletSettings>();
            services.AddSingleton(companySettings);

            foreach (var companySetting in companySettings.CompanySettings)
            {
                var companyCode = companySetting.Key;
                var setting = companySetting.Value;
                //Check Option
                if (string.IsNullOrWhiteSpace(setting.ServiceAccountJsonPath))
                    throw new InvalidOperationException($"CompanyCode: {companyCode}，設定檔錯誤，缺少 ServiceAccountJsonPath。");

                var credential = (ServiceAccountCredential)GoogleCredential
                        .FromFile(setting.ServiceAccountJsonPath)
                        .CreateScoped(WalletobjectsService.ScopeConstants.WalletObjectIssuer)
                        .UnderlyingCredential;
                var walletService = new WalletobjectsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential
                });

                //先建立好完整服務,子元件應該都透過Wallet Handler操作
                var googleWalletContext = new GoogleWalletContext(setting, walletService, credential);
                var flightWalletClassService = new FlightClassRepository(googleWalletContext.WalletobjectsService);
                var flightWalletObjectService = new FlightObjectRepository(googleWalletContext.WalletobjectsService);
                var flightWalletService = new FlightWallet(googleWalletContext, flightWalletClassService, flightWalletObjectService);
                var googleWalletService = new GoogleWalletHandler(googleWalletContext, flightWalletService);

                //services.AddKeyedSingleton<IGoogleWalletContext, GoogleWalletContext>(companyCode, (provider, key) =>
                //{
                //    return googleWalletContext;
                //});

                //services.AddKeyedSingleton<IFlightClassRepository, FlightClassRepository>(companyCode, (provider, key) =>
                //{
                //    return flightWalletClassService;
                //});

                //services.AddKeyedSingleton<IFlightObjectRepository, FlightObjectRepository>(companyCode, (provider, key) =>
                //{
                //    return flightWalletObjectService;
                //});

                //services.AddKeyedSingleton<IFlightWallet, FlightWallet>(companyCode, (provider, key) =>
                //{
                //    return flightWalletService;
                //});


                services.AddKeyedSingleton<IGoogleWalletHandler, GoogleWalletHandler>(companyCode, (provider, key) =>
                {
                    return googleWalletService;
                });

                //services.AddKeyedSingleton<IWalletHandler, WalletHandler>(companyCode, (provider, key) =>
                //{
                //    return new WalletHandler(companyCode, googleWalletService);
                //});

            }

            return services;
        }

    }
}
