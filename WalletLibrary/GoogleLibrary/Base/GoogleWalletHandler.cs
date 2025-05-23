using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Walletobjects.v1.Data;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WalletLibrary.GoogleLibrary.Base.Interfaces;
using WalletLibrary.GoogleLibrary.Context.Interfaces;
using WalletLibrary.GoogleLibrary.Settings;

namespace WalletLibrary.GoogleLibrary.Base
{
    public class GoogleWalletHandler : IGoogleWalletHandler
    {
        public GoogleWalletSettings WalletSettings => _WalletContext.WalletSettings;

        /// <summary>
        /// Wallet Context<br/>
        /// 各種設定 & 金鑰憑證<br/>
        /// </summary>
        private readonly IGoogleWalletContext _WalletContext;

        /// <summary>
        /// 航班錢包
        /// </summary>
        public IWalletHandler<FlightClass, FlightObject> FlightWallet { get; private set; }

        // Other Wallets
        // 可擴充錢包：
        // public IWalletHandler<GiftCardClass, GiftCardObject> GiftCardWallet { get; private set; }

        public GoogleWalletHandler(
            IGoogleWalletContext context,
            IWalletHandler<FlightClass, FlightObject> flightWallet
        )
        {
            _WalletContext = context;
            FlightWallet = flightWallet;
        }

        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By Class Resource ID 和 Object Resource ID。
        /// </summary>
        /// <param name="classResourceId">Class Resource ID。</param>
        /// <param name="objectResourceId">Object Resource ID。</param>
        /// <param name="type">Wallet 類型</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        /// <exception cref="ArgumentException">未指定 Wallet 類型</exception>
        public string GetJwtToken(string classResourceId, string objectResourceId, string type)
        {
            // 設置 JSON 序列化設置以忽略空值。
            JsonSerializerSettings excludeNulls = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            var getFlightObject = new FlightObject
            {
                Id = classResourceId,
                ClassId = objectResourceId,
            };

            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(getFlightObject, excludeNulls)
            );
            JObject jwtPayload = JObject.Parse(
                JsonConvert.SerializeObject(
                    new
                    {
                        iss = _WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = _WalletContext.WalletSettings.Origins
                            ?? new List<string> { "https://google.com" }, // 設置來源。
                        typ = "savetowallet", // 設置類型。
                        payload = type.ToLower() switch
                        {
                            "boardingpasses" => FlightWallet.GetPayloadObject(
                                classResourceId,
                                objectResourceId
                            ),
                            //"giftcard" => GiftCardWallet.GetPayloadObject(objectResourceId),
                            _ => throw new ArgumentException(
                                $"未知的 Wallet 類型: {type}",
                                nameof(type)
                            ),
                        },
                    },
                    excludeNulls
                )
            );
            // 將 JSON 負載反序列化為 JwtPayload。
            JwtPayload claims = JwtPayload.Deserialize(jwtPayload.ToString());

            // 使用服務帳戶憑證簽署 JWT。
            RsaSecurityKey key = new RsaSecurityKey(_WalletContext.Credential.Key)
            {
                KeyId = _WalletContext.Credential.KeyId,
            };

            SigningCredentials signingCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.RsaSha256
            );
            JwtSecurityToken jwt = new JwtSecurityToken(new JwtHeader(signingCredentials), claims);
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            //LogResponse($"Add to Google Wallet link: https://pay.google.com/gp/v/save/{token}");
            return $"https://pay.google.com/gp/v/save/{token}";
        }

        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By FlightObject ID。
        /// </summary>
        /// <param name="objectResourceId">航班對象的 ID。</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        public string GetJwtToken(string objectResourceId, string type)
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
                        iss = _WalletContext.Credential.Id, // 設置發行者 ID。
                        aud = "google", // 設置受眾為 Google。
                        origins = _WalletContext.WalletSettings.Origins
                            ?? new List<string> { "https://google.com" }, // 設置來源。
                        typ = "savetowallet", // 設置類型。
                        payload = type.ToLower() switch
                        {
                            "boardingpasses" => FlightWallet.GetPayloadObject(objectResourceId),
                            //"giftcard" => GiftCardWallet.GetPayloadObject(objectResourceId),
                            _ => throw new ArgumentException(
                                $"未知的 Wallet 類型: {type}",
                                nameof(type)
                            ),
                        },
                    },
                    excludeNulls
                )
            );
            // 將 JSON 負載反序列化為 JwtPayload。
            JwtPayload claims = JwtPayload.Deserialize(jwtPayload.ToString());

            // 使用服務帳戶憑證簽署 JWT。
            RsaSecurityKey key = new RsaSecurityKey(_WalletContext.Credential.Key)
            {
                KeyId = _WalletContext.Credential.KeyId,
            };

            SigningCredentials signingCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.RsaSha256
            );
            JwtSecurityToken jwt = new JwtSecurityToken(new JwtHeader(signingCredentials), claims);
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            //LogResponse($"Add to Google Wallet link: https://pay.google.com/gp/v/save/{token}");
            return $"https://pay.google.com/gp/v/save/{token}";
        }
    }
}
