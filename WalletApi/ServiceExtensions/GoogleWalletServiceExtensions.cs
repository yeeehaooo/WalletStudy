//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Services;
//using Google.Apis.Walletobjects.v1;
//using WalletLibrary.Flight;
//using WalletLibrary.Settings;
//using Microsoft.Extensions.Options;

//namespace GoogleWalletApi.ServiceExtensions
//{
//    /// <summary>
//    /// 提供 Google Wallet 相關服務的 DI 註冊擴充方法。
//    /// </summary>
//    public static class GoogleWalletServiceExtensions
//    {
//        /// <summary>
//        /// 註冊 Google Wallet 的核心服務，包括 ServiceAccountCredential 與 WalletobjectsService。
//        /// </summary>
//        /// <param name="services">IServiceCollection 實例。</param>
//        /// <param name="configuration">應用程式的組態資訊。</param>
//        /// <returns>回傳 IServiceCollection，支援方法鏈式呼叫。</returns>
//        public static IServiceCollection AddGoogleWalletServices(
//            this IServiceCollection services,
//            IConfiguration configuration
//        )
//        {
//            // 綁定設定檔並註冊 IOptions<GoogleWalletSettings>
//            services.Configure<GoogleWalletSettings>(configuration.GetSection("GoogleWalletSettings"));

//            // 註冊 ServiceAccountCredential
//            services.AddSingleton<ServiceAccountCredential>(provider =>
//            {
//                var settings = provider.GetRequiredService<IOptions<GoogleWalletSettings>>().Value;
//                return (ServiceAccountCredential)
//                    GoogleCredential
//                        .FromFile(settings.ServiceAccountJsonPath)
//                        .CreateScoped(WalletobjectsService.ScopeConstants.WalletObjectIssuer)
//                        .UnderlyingCredential;
//            });

//            // 註冊 WalletobjectsService
//            services.AddSingleton<WalletobjectsService>(provider =>
//            {
//                var credential = provider.GetRequiredService<ServiceAccountCredential>();
//                return new WalletobjectsService(
//                    new BaseClientService.Initializer { HttpClientInitializer = credential }
//                );
//            });

//            return services;
//        }

//        /// <summary>
//        /// 註冊 Google Wallet Flight 模組所需的服務。
//        /// </summary>
//        /// <param name="services">IServiceCollection 實例。</param>
//        /// <returns>回傳 IServiceCollection，支援方法鏈式呼叫。</returns>
//        public static IServiceCollection AddFlightService(this IServiceCollection services)
//        {
//            services.AddSingleton<FlightHandler>();
//            return services;
//        }
//    }
//}
