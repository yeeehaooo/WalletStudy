using Google.Apis.Walletobjects.v1.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WalletLibrary.GoogleLibrary.Base.Interfaces;

namespace WalletLibrary.GoogleLibrary.Wallets.BoardingPasses
{
    public class BoardingPassesHandler : IWalletHandler<FlightClass, FlightObject>
    {
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
