using Google.Apis.Walletobjects.v1.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WalletLibrary.GoogleLibrary.Base.Interfaces;

namespace WalletLibrary.GoogleLibrary.Wallets.BoardingPasses
{
    public class BoardingPassesHandler : IWalletHandler<FlightClass, FlightObject>
    {
        /*
        API 錯誤回應
下表說明 Google Wallet API 可能會傳回的錯誤代碼、可能的原因以及解決方法。

例外狀況	訊息範例	建議
400 - BadRequestException	要求含有無效引數，資源 ID 無效：{1234567891234567899 - ABCD1234567}。	檢查類型、格式和長度的資料結構，並傳遞正確的引數。
403 - PermissionDeniedException	權限遭拒	確認正確的服務帳戶電子郵件是商家付款和錢包主控台的授權使用者。
404 - NotFoundException	找不到錢包物件 {1234567891234567899.SampleClubCardxf6a8edf-87ca-4022-a813-694cc57e9fd3}。	嘗試提出 PATCH 或 PUT 要求之前，請先對物件 ID 執行 GET，確保您有需要更新的物件且是最新版本。
404 - IssuerClassNotFoundException	找不到錢包物件類別 {1234567891234567899.ABCD.1234567}。	執行更新時，請務必在 PATCH 或 PUT 要求之前執行 GET 要求，確保您的類別可以參照其是最新的類別。此外，也請確認要求中使用的酬載 (物件和類別) 正確無誤。
409 - OnceExistsException	已有電子錢包物件類別 {1234567891234567899.ABCD.1234567}。	請先對類別 ID 執行 GET，再嘗試建立相同的課程 ID。如果該資源存在，建議您使用 PATCH 或 PUT.。
         */
        /// <summary>
        /// 航班類別的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        public IClassResource<FlightClass> ClassResource { get; private set; }

        /// <summary>
        /// 航班對象的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        public IObjectResource<FlightObject> ObjectResource { get; private set; }

        public BoardingPassesHandler(
            IClassResource<FlightClass> flightClass,
            IObjectResource<FlightObject> flightObject
        )
        {
            ClassResource = flightClass; // Fixed the incorrect property name
            ObjectResource = flightObject;
        }

        // 產生 payload JObject
        public JObject GetPayloadObject(string classResourceId, string objectResourceId)
        {
            JObject serializedClass = JObject.Parse(
                JsonConvert.SerializeObject(
                    new FlightClass { Id = classResourceId },
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                )
            );
            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(
                    new FlightObject { ClassId = classResourceId, Id = objectResourceId },
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                )
            );

            var payloadData = new
            {
                flightClasses = new List<JObject> { serializedClass },
                flightObjects = new List<JObject> { serializedObject },
            };
            return JObject.Parse(JsonConvert.SerializeObject(payloadData));
        }

        // 委派：產生 payload JObject
        public JObject GetPayloadObject(string objectResourceId)
        {
            JObject serializedObject = JObject.Parse(
                JsonConvert.SerializeObject(
                    new FlightObject { Id = objectResourceId },
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                )
            );

            var payloadData = new { flightObjects = new List<JObject> { serializedObject } };
            return JObject.Parse(JsonConvert.SerializeObject(payloadData));
        }
    }
}
