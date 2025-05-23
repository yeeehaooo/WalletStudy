using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Walletobjects.v1.Data;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Context.Interfaces;
using WalletLibrary.Logger;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight
{
    public class BoardingPassesHandler
        : BaseHandlerLogger<BoardingPassesHandler>,
            IWalletHandler<FlightClass, FlightObject>
    {
        /// <summary>
        /// 航班類別的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        public IClassRepository<FlightClass> ClassRepository { get; private set; }

        /// <summary>
        /// 航班對象的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        public IObjectRepository<FlightObject> ObjectRepository { get; private set; }

        /// <summary>
        /// Google Wallet 基本設定
        /// </summary>
        private IGoogleWalletContext WalletContext { get; }

        public BoardingPassesHandler(
            ILogger<BoardingPassesHandler> logger,
            IGoogleWalletContext walletContext,
            IClassRepository<FlightClass> flightClass,
            IObjectRepository<FlightObject> flightObject
        )
            : base(logger)
        {
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
        public async Task<string> GetJwtToken(
            string flightClassResourceId,
            string flightObjectResourceId
        )
        {
            // 設置 JSON 序列化設置以忽略空值。
            JsonSerializerSettings excludeNulls = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            var getFlightObject = new FlightObject
            {
                Id = flightObjectResourceId,
                ClassId = flightClassResourceId,
            };

            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(getFlightObject, excludeNulls)
            );
            JObject jwtPayload = JObject.Parse(
                JsonConvert.SerializeObject(
                    new
                    {
                        iss = WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = WalletContext.WalletSettings.Origins
                            ?? new List<string> { "https://google.com" }, // 設置來源。
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

            LogResponse($"Add to Google Wallet link: https://pay.google.com/gp/v/save/{token}");
            return $"https://pay.google.com/gp/v/save/{token}";
        }

        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By FlightObject ID。
        /// </summary>
        /// <param name="objectResourceId">航班對象的 ID。</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        public async Task<string> GetJwtToken(string objectResourceId)
        {
            // 設置 JSON 序列化設置以忽略空值。
            JsonSerializerSettings excludeNulls = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(
                    new FlightObject { Id = objectResourceId },
                    excludeNulls
                )
            );
            JObject jwtPayload = JObject.Parse(
                JsonConvert.SerializeObject(
                    new
                    {
                        iss = WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = WalletContext.WalletSettings.Origins
                            ?? new List<string> { "https://google.com" }, // 設置來源。
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

            LogResponse($"Add to Google Wallet link: https://pay.google.com/gp/v/save/{token}");
            return $"https://pay.google.com/gp/v/save/{token}";
        }
    }
}
