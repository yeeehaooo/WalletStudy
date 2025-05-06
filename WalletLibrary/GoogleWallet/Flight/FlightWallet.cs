using Google.Apis.Walletobjects.v1.Data;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using WalletLibrary.GoogleWallet.Context.Interfaces;
using WalletLibrary.GoogleWallet.Flight.Interfaces;

namespace WalletLibrary.GoogleWallet.Flight
{
    public class FlightWallet : IFlightWallet
    {
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
            IGoogleWalletContext walletContext,
            IFlightClassRepository flightClass,
            IFlightObjectRepository flightObject)
        {
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
            Google.Apis.Walletobjects.v1.Data.FlightObject flightObject
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

            Console.WriteLine("Add to Google Wallet link");
            Console.WriteLine($"https://pay.google.com/gp/v/save/{token}");

            return $"https://pay.google.com/gp/v/save/{token}";
        }

        /// <summary>
        /// 創建一個簽名的 JWT 以引用現有的航班類別和對象。
        /// </summary>
        /// <param name="flightClassId">航班類別的 ID。</param>
        /// <param name="flightObjectId">航班對象的 ID。</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        public async Task<string> CreateJWTNewObjects(string flightClassId, string flightObjectId)
        {
            // 設置 JSON 序列化設置以忽略空值。
            JsonSerializerSettings excludeNulls = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            // 檢查航班類別是否存在。
            var createFlightClass = await ClassRepository.GetAsync(flightClassId);
            // 將航班類別序列化為 JSON。
            JObject serializedClass = JObject.Parse(
                JsonConvert.SerializeObject(createFlightClass, excludeNulls)
            );

            // 檢查航班對象是否存在。
            var createFlightObject = await ObjectRepository.GetAsync(flightObjectId);
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

            Console.WriteLine("Add to Google Wallet link");
            Console.WriteLine($"https://pay.google.com/gp/v/save/{token}");

            return $"https://pay.google.com/gp/v/save/{token}";
        }

        public Task<FlightClass> GetClassAsync(string resourceId)
        {
            return ClassRepository.GetAsync(resourceId);
        }

        public Task<FlightClass> InsertClassAsync(FlightClass flightClass)
        {
            return ClassRepository.InsertAsync(flightClass);
        }

        public Task<FlightClass> UpdateClassAsync(FlightClass flightClass)
        {
            return ClassRepository.UpdateAsync(flightClass);
        }

        public Task<FlightClass> PatchClassAsync(FlightClass flightClass)
        {
            return ClassRepository.PatchAsync(flightClass);
        }

        public Task<FlightClass> AddClassMessageAsync(AddMessageRequest addMessageRequest, string resourceId)
        {
            return ClassRepository.AddMessageAsync(addMessageRequest, resourceId);
        }

        public Task<Google.Apis.Walletobjects.v1.Data.FlightObject> GetObjectAsync(string resourceId)
        {
            return ObjectRepository.GetAsync(resourceId);
        }

        public Task<Google.Apis.Walletobjects.v1.Data.FlightObject> InsertObjectAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject)
        {
            return ObjectRepository.InsertAsync(flightObject);
        }

        public Task<Google.Apis.Walletobjects.v1.Data.FlightObject> UpdateObjectAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject)
        {
            return ObjectRepository.UpdateAsync(flightObject);
        }

        public Task<Google.Apis.Walletobjects.v1.Data.FlightObject> PatchObjectAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject)
        {
            return ObjectRepository.PatchAsync(flightObject);
        }

        public Task<Google.Apis.Walletobjects.v1.Data.FlightObject> AddObjectMessageAsync(AddMessageRequest addMessageRequest, string resourceId)
        {
            return ObjectRepository.AddMessageAsync(addMessageRequest, resourceId);
        }

        public Task<Google.Apis.Walletobjects.v1.Data.FlightObject> ExpireObjectAsync(string resourceId)
        {
            return ObjectRepository.ExpireObjectAsync(resourceId);
        }

        public Task<Google.Apis.Walletobjects.v1.Data.FlightObject> UpdateObjectStateAsync(string resourceId, string objectState)
        {
            return ObjectRepository.UpdateObjectStateAsync(resourceId, objectState);
        }
    }
}
