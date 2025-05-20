using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Walletobjects.v1.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        /// 生成 "Add to Google Wallet" 的連結 By FlightClass ID 和 FlightObject ID。
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
            var getFlightObject = new FlightObject { Id = flightObjectId, ClassId = flightClassId };

            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(getFlightObject, excludeNulls)
            );
            JObject jwtPayload = JObject.Parse(
                JsonConvert.SerializeObject(
                    new
                    {
                        iss = WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = new List<string> { "https://www.china-airlines.com/tw/zh" }, // 設置來源。
                        typ = "savetowallet", // 設置類型。
                        payload = JObject.Parse(
                            JsonConvert.SerializeObject(
                                new { flightObjects = new List<JObject> { serializedObject } }
                            )
                        ),
                    },
                    excludeNulls
                )
            );
            // 將 JSON 負載反序列化為 JwtPayload。
            JwtPayload claims = JwtPayload.Deserialize(jwtPayload.ToString());

            // 使用服務帳戶憑證簽署 JWT。
            RsaSecurityKey key = new RsaSecurityKey(WalletContext.Credential.Key)
            {
                KeyId = WalletContext.Credential.KeyId,
            };

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

        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By FlightObject ID。
        /// </summary>
        /// <param name="flightObjectId">航班對象的 ID。</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        public async Task<string> GetJwtToken(string flightObjectId)
        {
            // 設置 JSON 序列化設置以忽略空值。
            JsonSerializerSettings excludeNulls = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            var getFlightObject = await ObjectRepository.GetAsync(flightObjectId);

            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(
                    new FlightObject { Id = getFlightObject.Id, ClassId = getFlightObject.ClassId },
                    excludeNulls
                )
            );
            JObject jwtPayload = JObject.Parse(
                JsonConvert.SerializeObject(
                    new
                    {
                        iss = WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = new List<string> { "https://www.china-airlines.com/tw/zh" }, // 設置來源。
                        typ = "savetowallet", // 設置類型。
                        payload = JObject.Parse(
                            JsonConvert.SerializeObject(
                                new { flightObjects = new List<JObject> { serializedObject } }
                            )
                        ),
                    },
                    excludeNulls
                )
            );
            // 將 JSON 負載反序列化為 JwtPayload。
            JwtPayload claims = JwtPayload.Deserialize(jwtPayload.ToString());

            // 使用服務帳戶憑證簽署 JWT。
            RsaSecurityKey key = new RsaSecurityKey(WalletContext.Credential.Key)
            {
                KeyId = WalletContext.Credential.KeyId,
            };

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
