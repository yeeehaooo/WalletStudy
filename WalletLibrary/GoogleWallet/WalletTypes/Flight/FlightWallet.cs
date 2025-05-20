using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Google.Apis.Walletobjects.v1.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WalletLibrary.Base.Models;
using WalletLibrary.GoogleWallet.Context.Interfaces;
using WalletLibrary.GoogleWallet.WalletTypes.Flight.Interfaces;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight
{
    public class FlightWallet : IFlightWallet
    {
        private readonly ILogger<FlightWallet> _logger;

        /// <summary>
        /// 航班類別的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        public IFlightClassRepository ClassRepository { get; }

        /// <summary>
        /// 航班對象的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        public IFlightObjectRepository ObjectRepository { get; }

        private IGoogleWalletContext WalletContext { get; }

        public FlightWallet(
            ILogger<FlightWallet> logger,
            IGoogleWalletContext walletContext,
            IFlightClassRepository flightClass,
            IFlightObjectRepository flightObject
        )
        {
            _logger = logger;
            WalletContext = walletContext;
            ClassRepository = flightClass;
            ObjectRepository = flightObject;
        }

        /// <summary>
        /// 創建一個簽名的 JWT 以創建新的航班類別和對象。
        /// </summary>
        /// <param name="flightClass">要創建的航班類別。</param>
        /// <param name="flightObject">要創建的航班對象。</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        public async Task<string> CreateJWTNewObjects(
            FlightClass flightClass,
            FlightObject flightObject
        )
        {
            // 設置 JSON 序列化設置以忽略空值。
            JsonSerializerSettings excludeNulls = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            // 檢查航班類別是否已存在。
            var createFlightClass = await ClassRepository.InsertAsync(flightClass);
            // 將航班類別序列化為 JSON。
            JObject serializedClass = JObject.Parse(
                JsonConvert.SerializeObject(createFlightClass, excludeNulls)
            );

            // 檢查航班對象是否已存在。
            var createFlightObject = await ObjectRepository.InsertAsync(flightObject);
            // 將航班對象序列化為 JSON。
            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(createFlightObject, excludeNulls)
            );

            // 創建 JWT 負載作為 JSON 對象。
            JObject jwtPayload = JObject.Parse(
                JsonConvert.SerializeObject(
                    new
                    {
                        iss = WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = new List<string> { "www.example.com" }, // 設置來源。
                        typ = "savetowallet", // 設置類型。
                        payload = JObject.Parse(
                            JsonConvert.SerializeObject(
                                new
                                {
                                    // 當用戶將通行證保存到錢包時，將創建列出的類別和對象。
                                    flightClasses = new List<JObject> { serializedClass },
                                    flightObjects = new List<JObject> { serializedObject },
                                }
                            )
                        ),
                    }
                )
            );

            // 將 JSON 負載反序列化為 JwtPayload。
            JwtPayload claims = JwtPayload.Deserialize(jwtPayload.ToString());

            // 使用服務帳戶憑證簽署 JWT。
            RsaSecurityKey key = new RsaSecurityKey(WalletContext.Credential.Key);
            SigningCredentials signingCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.RsaSha256
            );
            JwtSecurityToken jwt = new JwtSecurityToken(new JwtHeader(signingCredentials), claims);
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            _logger.LogInformation(
                "Add to Google Wallet link: https://pay.google.com/gp/v/save/{token}",
                token
            );
            return $"https://pay.google.com/gp/v/save/{token}";
        }

        /// <summary>
        /// 創建一個簽名的 JWT 以引用現有的航班類別和對象。
        /// </summary>
        /// <param name="flightClassId">航班類別的 ID。</param>
        /// <param name="flightObjectId">航班對象的 ID。</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        public async Task<string> GetJwtToken(string flightClassId, string flightObjectId)
        {
            // 設置 JSON 序列化設置以忽略空值。
            JsonSerializerSettings excludeNulls = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            // 檢查航班類別是否存在。
            var getFlightClass = await ClassRepository.GetAsync(flightClassId);
            // 將航班類別序列化為 JSON。
            JObject serializedClass = JObject.Parse(
                JsonConvert.SerializeObject(getFlightClass, excludeNulls)
            );

            // 檢查航班對象是否存在。
            var getFlightObject = await ObjectRepository.GetAsync(flightObjectId);
            // 將航班對象序列化為 JSON。
            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(getFlightObject, excludeNulls)
            );

            // 創建 JWT 負載作為 JSON 對象。
            JObject jwtPayload = JObject.Parse(
                JsonConvert.SerializeObject(
                    new
                    {
                        iss = WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = new List<string> { "www.example.com" }, // 設置來源。
                        typ = "savetowallet", // 設置類型。
                        payload = JObject.Parse(
                            JsonConvert.SerializeObject(
                                new
                                {
                                    // 當用戶將通行證保存到錢包時，將創建列出的類別和對象。
                                    flightClasses = new List<JObject> { serializedClass },
                                    flightObjects = new List<JObject> { serializedObject },
                                }
                            )
                        ),
                    }
                )
            );

            // 將 JSON 負載反序列化為 JwtPayload。
            JwtPayload claims = JwtPayload.Deserialize(jwtPayload.ToString());
            // 使用服務帳戶憑證簽署 JWT。
            RsaSecurityKey key = new RsaSecurityKey(WalletContext.Credential.Key);
            SigningCredentials signingCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.RsaSha256
            );
            JwtSecurityToken jwt = new JwtSecurityToken(new JwtHeader(signingCredentials), claims);
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            _logger.LogDebug(
                "Add to Google Wallet link: https://pay.google.com/gp/v/save/{token}",
                token
            );
            return $"https://pay.google.com/gp/v/save/{token}";
        }
    }
}
