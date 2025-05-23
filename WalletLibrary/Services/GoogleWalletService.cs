using Google.Apis.Walletobjects.v1.Data;
using Microsoft.Extensions.Logging;
using WalletLibrary.Define;
using WalletLibrary.GoogleLibrary.Defines;
using WalletLibrary.GoogleLibrary.Templates;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Models;
using WalletLibrary.GoogleWallet.Models.Images;
using WalletLibrary.GoogleWallet.Models.Languages;
using WalletLibrary.GoogleWallet.Settings;
using WalletLibrary.GoogleWallet.WalletTypes.Flight.Models;
using WalletLibrary.Services.Interfaces;
using WalletLibrary.Utility;
using static WalletLibrary.GoogleLibrary.Defines.GoogleDefine;
using GoogleApiUri = Google.Apis.Walletobjects.v1.Data.Uri;

namespace WalletLibrary.Services
{
    public partial class GoogleWalletService : IGoogleWalletService
    {
        private readonly ILogger<GoogleWalletService> _logger;

        private readonly IGoogleWalletHandler _googleWalletHandler;

        private readonly string _issuerId;

        private readonly string _issuerName;

        private GoogleWalletSettings _settings;

        public GoogleWalletService(
            ILogger<GoogleWalletService> logger,
            IGoogleWalletHandler googleWalletHandler
        )
        {
            _logger = logger;
            _googleWalletHandler = googleWalletHandler;
            _settings = googleWalletHandler.WalletSettings;
            _issuerId = googleWalletHandler.WalletSettings.IssuerId;
            _issuerName = googleWalletHandler.WalletSettings.IssuerName;
        }

        #region Google Wallet 登機證
        public async Task<FlightClass> InsertFlightInfoAsync(FlightInfo flightInfo)
        {
            var flightClass = BuildFlightClass(flightInfo);
            return await InsertFlightClassAsync(flightClass);
        }

        public async Task<FlightObject> InsertPassengerInfoAsync(PassengerInfo passengerInfo)
        {
            var flightObject = BuildFlightObject(passengerInfo);
            return await InsertFlightObjectAsync(flightObject);
        }

        /// <summary>
        /// 建立範本<br/>
        /// 根據航班唯一值,取得相關資訊<br/>
        /// 呼叫 BuildFlightClass轉換,上傳Google Api<br/>
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="objectId"></param>
        public async Task<string> CreateFlightAsync(string classId)
        {
            FlightInfo flight = new FlightInfo();
            flight.ClassSuffix = classId;
            // 航班資訊
            var operating = new AirLineInfoModel();
            operating.CarrierCode = "CI";
            operating.FlightNumber = "9527";
            operating.AirlineLogo = new ImageUriItem(
                // 圖片鏈結
                uri: "https://airhex.com/images/airline-logos/alt/china-airlines.png",
                // 圖片描述
                description: "Airline Logo",
                // 多語系圖片描述
                localizedDescription: null
            );
            operating.AirLineName = new LocalizedStringItem(
                // 預設語系
                defaultValue: new TranslatedStringItem("en-US", "China Airlines"),
                // 其他語系
                translatedValues: null
            );
            flight.Operating = operating;

            // 時刻資訊
            flight.DepartureDate = "2025/01/01";
            flight.DepartureTime = "08:30";
            flight.ArrivalDate = "2025/01/01";
            flight.ArrivalTime = "12:30";
            flight.BoardingDate = "2025/01/01";
            flight.LatestBoardingTime = "08:00";

            // 出發機場資訊
            var departure = new AirportInfoModel();
            departure.CityName = "Taipei";
            departure.IATA = "TPE"; // 出發地機場的 IATA 三碼代碼
            departure.Terminal = "6";
            departure.Gate = "A1"; // 登機門號碼
            departure.NameOverride = new LocalizedStringItem(
                // 預設語系
                defaultValue: new TranslatedStringItem("en-US", "Taipei Airport"),
                // 其他語系
                translatedValues: new List<TranslatedStringItem>
                {
                    new TranslatedStringItem("zh-TW", "桃園機場"),
                }
            );
            flight.DepartureAirport = departure;
            var arrival = new AirportInfoModel();
            arrival.CityName = "Tokyo";
            arrival.IATA = "NRT"; // 抵達地機場的 IATA 三碼代碼
            arrival.Terminal = "2";
            arrival.Gate = "B01"; // 登機門號碼
            arrival.NameOverride = new LocalizedStringItem(
                // 預設語系
                defaultValue: new TranslatedStringItem("en-US", "Narita Airport"),
                // 其他語系
                translatedValues: new List<TranslatedStringItem>
                {
                    new TranslatedStringItem("zh-TW", "成田機場"),
                }
            );
            flight.ArrivalAirport = arrival;

            flight.AirportCheckinInfo = new UriItem(
                uri: "http://AirportCheckinInfo.com",
                description: "AirportCheckinInfo"
            );
            flight.BaggageMessageInfo = new UriItem(
                uri: "http://BaggageMessageInfo.com",
                description: "BaggageMessageInfo"
            );
            flight.OperatingCarrierName = new TextDataItem(
                header: "Operating Carrier Name",
                body: "Operated by China Airlines LTD."
            );
            flight.ReminderMessage = new TextDataItem(
                header: "Reminder Message",
                body: "Please arrive at the airport 2 hours before departure."
            );

            var flightClass = await InsertFlightInfoAsync(flight);
            return flightClass.Id;
        }

        /// <summary>
        /// 建立範本<br/>
        /// 根據航班唯一值,根據旅客該航班唯一值,取得相關資訊<br/>
        /// 呼叫 BuildFlightClass轉換,上傳Google Api<br/>
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="objectId"></param>
        public async Task<string> CreatePassengerAsync(string classId, string objectId)
        {
            var passenger = new PassengerInfo();
            passenger.Channel = "WEB";
            passenger.ClassSuffix = classId;
            passenger.ObjectSuffix = objectId;
            passenger.Marketing = new AirLineInfoModel
            {
                CarrierCode = "JL",
                FlightNumber = "5678",
                AirlineLogo = new ImageUriItem(
                    // 圖片鏈結
                    uri: "https://airhex.com/images/airline-logos/alt/japan-airlines.png",
                    // 圖片描述
                    description: "Airline Logo",
                    // 多語系圖片描述
                    localizedDescription: null
                ),
                AirLineName = new LocalizedStringItem(
                    // 預設語系
                    defaultValue: new TranslatedStringItem("en-US", "Japan Airlines"),
                    // 其他語系
                    translatedValues: new List<TranslatedStringItem> { new("zh-TW", "日本航空") }
                ),
            };
            passenger.PassengerName = "WANG HSIAOMING";
            passenger.BoardingZone = "ZONE2";
            passenger.BookingClass = "C";
            passenger.SeatNumber = "5G";
            passenger.SecurityNumber = "002";
            passenger.FQTVTierDescription = "PARAGON";
            passenger.FQTVNumber = "CT0000000";
            passenger.IsSkyPriority = true;
            passenger.TSAPreCheckIndicator = "TSA PRE";
            passenger.IsTSAPreCheck = true;
            passenger.AdditionalTextString = "LOUNGE-VLSF";
            passenger.ConfirmationCode = "6LTO8V";
            passenger.ElecTicketNumber = "297240203609001";
            passenger.BarCode = "QrCode Test";
            passenger.SpecialMealCode = "VOML, XXML";
            passenger.BaggagesValues = new List<string> { "10A55300006CE42B", "10A55300006CE42C" };
            passenger.CabinComments = "BUSINESS CLASS";
            var flightObject = await InsertPassengerInfoAsync(passenger);
            return flightObject.Id;
        }

        #region 私有方法 Request 物件轉換成 Google Api Flight 物件

        /// <summary>
        /// 將 BoardingPassWalletModel 轉換為 Google Wallet 的 FlightClass 物件 範本<br/>
        /// 設定:<br/>
        /// 1. ReviewStatus 審核狀態<br/>
        /// 2. 背景底色<br/>
        /// 3. 主要圖片網址模組<br/>
        /// 航班相關設定<br/>
        /// 4. 出發機場資訊<br/>
        /// 5. 抵達機場資訊<br/>
        /// 航班時刻資訊（當地時間），格式必須符合 ISO 8601<br/>
        /// 6. 預定出發時間<br/>
        /// 7. 預定抵達時間<br/>
        /// 8. 登機時間<br/>
        /// 9. 航班資訊<br/>
        /// 以下為客製(需額外維護多語系標題與內容)<br/>
        /// 10. 文字區塊模組, 可以設定多組文字區塊(支援多語系 標題 & 內文), 非必填<br/>
        /// 11. 連結模組, 可以設定多組連結, 非必填<br/>
        /// 11-1. 機場與報到櫃台資訊(By語系) + Link<br/>
        /// 11-2. 詳細行李資訊(By語系) + Link<br/>
        /// 12. 通知訊息模組, 非必填<br/>
        /// </summary>
        /// <param name="flightInfo"></param>
        /// <returns></returns>
        private FlightClass BuildFlightClass(FlightInfo flightInfo)
        {
            FlightClass flightClass = new FlightClass();

            flightClass.Id = $"{_settings.IssuerId}.{flightInfo.ClassSuffix}";
            flightClass.IssuerName = _settings.IssuerName;

            // 1. Note: ReviewStatus must be 'UNDER_REVIEW' or 'DRAFT'
            // 審核狀態（預設為 "UNDER_REVIEW"，若已申請 正式版 production 可改為 "APPROVED"）
            flightClass.ReviewStatus = ReviewStatus.UNDER_REVIEW.ToString();

            // 2. 背景底色
            flightClass.HexBackgroundColor = _settings.HexBackgroundColor;

            // 3. 主要圖片網址模組
            flightClass.HeroImage =
                GoogleWalletUtility.CreateImageModule(
                    _settings.HeroImageId,
                    _settings.HeroImageUri,
                    _settings.HeroImageDescription
                ) ?? null;

            // 航班相關設定
            // 4. 出發機場資訊
            flightClass.Origin = new AirportInfo();
            flightClass.Origin.AirportIataCode = flightInfo.DepartureAirport.IATA; // 出發地機場的 IATA 三碼代碼
            flightClass.Origin.Terminal = flightInfo.DepartureAirport.Terminal; // 出發航廈編號（例如：T1、T2）
            flightClass.Origin.Gate = flightInfo.DepartureAirport.Gate; // 登機門號碼
            flightClass.Origin.AirportNameOverride =
                flightInfo.DepartureAirport.NameOverride?.ToLocalizedString(); // 出發機場名稱複寫 可以設定多語系
            // 5. 抵達機場資訊
            flightClass.Destination = new AirportInfo();
            flightClass.Destination.AirportIataCode = flightInfo.ArrivalAirport.IATA; // 抵達地機場的 IATA 三碼代碼
            flightClass.Destination.Terminal = flightInfo.ArrivalAirport.Terminal; // 抵達航廈編號
            flightClass.Destination.Gate = flightInfo.ArrivalAirport.Gate; // 抵達登機門號碼
            flightClass.Destination.AirportNameOverride =
                flightInfo.ArrivalAirport.NameOverride?.ToLocalizedString(); // 抵達機場名稱複寫 可以設定多語系

            // 航班時刻資訊（當地時間），格式必須符合 ISO 8601
            // 6. 預定出發時間 ISO 8601
            flightClass.LocalScheduledDepartureDateTime =
                GoogleWalletUtility.FormatToIso8601DateTimeString(
                    flightInfo.DepartureDate,
                    flightInfo.DepartureTime
                );
            // 7. 預定抵達時間 ISO 8601
            flightClass.LocalScheduledArrivalDateTime =
                GoogleWalletUtility.FormatToIso8601DateTimeString(
                    flightInfo.ArrivalDate,
                    flightInfo.ArrivalTime
                );
            // 8. 登機時間 ISO 8601
            flightClass.LocalBoardingDateTime = GoogleWalletUtility.FormatToIso8601DateTimeString(
                flightInfo.BoardingDate,
                flightInfo.LatestBoardingTime
            );

            // 9. 航班資訊
            flightClass.FlightHeader = new FlightHeader
            {
                // 航班號碼（數字）
                FlightNumber = flightInfo.Operating.FlightNumber,
                // 顯示用航班號碼（可能包含航空公司代碼）
                FlightNumberDisplayOverride = GoogleWalletUtility.FormatFlightNumber(
                    flightInfo.Operating.CarrierCode,
                    flightInfo.Operating.FlightNumber
                ),

                // 主承運航空公司資訊
                Carrier = new FlightCarrier
                {
                    // 航空公司 IATA 簡碼
                    CarrierIataCode = flightInfo.Operating.CarrierCode,
                    // 航空公司 ICAO 簡碼
                    //CarrierIcaoCode = "航空公司 ICAO 簡碼",
                    // 航空公司名稱
                    AirlineName = flightInfo.Operating.AirLineName.ToLocalizedString(),
                    // 航空公司 Logo
                    AirlineLogo = flightInfo.Operating.AirlineLogo?.ToImageModule(
                        "OperatingAirlineLogo"
                    ),
                    //// 航空公司 寬版Logo
                    //WideAirlineLogo =
                    //    boardingPassWallet.Flight.Operating.WideAirlineLogo?.ToImageModule(),
                    //// 航空聯盟 Logo
                    //AirlineAllianceLogo =
                    //    boardingPassWallet.Flight.Operating.AirlineAllianceLogo?.ToImageModule(),
                },
            };

            // 以下為客製(需額外維護多語系標題與內容)
            // 10. 文字區塊模組,可以設定多組文字區塊(支援多語系 標題 & 內文), 非必填
            flightClass.TextModulesData = new List<TextModuleData>
            {
                flightInfo.ReminderMessage.ToTextModule(BaseCardTemplate.ReminderMessage.FieldName),
            };

            // 11. 連結模組, 可以設定多組連結, 非必填
            //
            flightClass.LinksModuleData = new LinksModuleData
            {
                Uris = new List<GoogleApiUri>
                {
                    // 11-1. 機場與報到櫃台資訊(By語系) + Link
                    flightInfo.AirportCheckinInfo?.ToUriModule(
                        BaseCardTemplate.AirportCheckinInfo.FieldName
                    ),
                    // 11-2. 詳細行李資訊(By語系) + Link
                    flightInfo.BaggageMessageInfo?.ToUriModule(
                        BaseCardTemplate.BaggageInfo.FieldName
                    ),
                },
            };

            // 12. 通知訊息模組, 非必填
            // 可以設定多組訊息(支援多語系 標題 & 內文)
            // 可以額外設定 起始時間 & 結束時間 & 通知類型
            flightClass.Messages = new List<Message> { };

            // 預設範本(有override 會清空預設排版)
            flightClass.ClassTemplateInfo = BaseCardTemplate.GetCardTemplate(AirLineIATADefine.CI);

            return flightClass;
        }

        /// <summary>
        /// 建立 Google Wallet 的 FlightObject 物件（搭配 FlightClass）<br/>
        /// 設定：<br/>
        /// 1. Class ID 與 Object ID<br/>
        /// 2. 姓名<br/>
        /// 3. 座位資訊（座位號碼、艙等）<br/>
        /// 4. 條碼資訊<br/>
        /// 5. ReservationInfo(確認代碼,機票號碼,常客資訊)<br/>
        /// 以下為客製<br/>
        /// 6. Customer TextModulesData<br/>
        /// 7. Customer LinksModulesData<br/>
        /// </summary>
        /// <param name="boardingPassWallet">登機證模型</param>
        /// <returns></returns>
        private FlightObject BuildFlightObject(PassengerInfo passengerInfo)
        {
            var objectId = $"{_settings.IssuerId}.{passengerInfo.ObjectSuffix}";

            var flightObject = new FlightObject
            {
                // 1. Class ID 與 Object ID
                Id = objectId,
                ClassId = $"{_settings.IssuerId}.{passengerInfo.ClassSuffix}",
                State = GoogleDefine.State.ACTIVE.ToString(), // ACTIVE, INACTIVE, EXPIRED, etc.
                // 2. 姓名
                PassengerName = passengerInfo.PassengerName,
                // 3. 座位資訊（座位號碼、艙等）
                BoardingAndSeatingInfo = new BoardingAndSeatingInfo
                {
                    // 登機門 (BoardingDoor)
                    // 備註: 若航空公司依座艙分流登機，則不設定此欄位。
                    // BoardingDoor 可以依實際需求設定 boardingPassWallet 內相應欄位 (此處未設定)

                    // 登機分組 (BoardingGroup)
                    // 通常代表旅客的登機分組（如 Zone1, Zone2）
                    BoardingGroup = passengerInfo.BoardingZone,

                    // 登機順序編號 (BoardingPosition)
                    // 一般指示旅客的登機優先順序編號
                    //BoardingPosition = boardingPassWallet.Passenger.BoardingZone,

                    // 序號 (SequenceNumber)
                    // 旅客的報到序號，通常格式如 "SEQ:002"
                    SequenceNumber = passengerInfo.SecurityNumber,

                    // 座艙等級 (SeatClass)
                    // 例如：BUSINESS CLASS、ECONOMY 等
                    SeatClass = passengerInfo.CabinComments,

                    // 座位號碼 (SeatNumber)
                    // 例如 5G、12A 等
                    SeatNumber = passengerInfo.SeatNumber,

                    // 特殊登機權益圖像 (BoardingPrivilegeImage)
                    // 根據 FQTVAllianceTier 字串決定是否顯示特權圖標
                    // 注意：這裡 boardingPassWallet.Passenger.FQTVAllianceTier 應為圖片 URL
                    BoardingPrivilegeImage = !passengerInfo.IsSkyPriority
                        ? null
                        : GoogleWalletUtility.CreateImageModule(
                            BaseCardTemplate.PrivilegeImage.FieldName,
                            _settings.BoardingPrivilegeImage,
                            "SKY PRIORITY"
                        ),
                },
                SecurityProgramLogo = !passengerInfo.IsTSAPreCheck
                    ? null
                    : GoogleWalletUtility.CreateImageModule(
                        BaseCardTemplate.SecurityProgramLogo.FieldName,
                        _settings.SecurityProgramLogo,
                        "TSA PRE"
                    ),
                // 4.條碼資訊（QR_Code / AZTEC / PDF417 / CODE128）
                Barcode = new Barcode
                {
                    Type = "QR_CODE",
                    Value = passengerInfo.BarCode,
                    AlternateText = passengerInfo.Channel,
                    RenderEncoding = "UTF-8",
                    //ShowCodeText = new LocalizedString
                    //{
                    //    DefaultValue = new TranslatedString
                    //    {
                    //        Language = "zh-TW",
                    //        Value = "點擊以顯示QR CODE",
                    //    },
                    //    TranslatedValues = new List<TranslatedString>
                    //    {
                    //        new TranslatedString
                    //        {
                    //            Language = "en-US",
                    //            Value = "Click to Show QR CODE",
                    //        },
                    //    },
                    //},
                },
                // 5.
                ReservationInfo = new ReservationInfo
                {
                    ConfirmationCode = passengerInfo.ConfirmationCode,
                    EticketNumber = passengerInfo.ElecTicketNumber,
                    FrequentFlyerInfo = new FrequentFlyerInfo
                    {
                        FrequentFlyerProgramName = GoogleWalletUtility.CreateLocalizedString(
                            "en-US",
                            passengerInfo.FQTVTierDescription
                        ),
                        FrequentFlyerNumber = passengerInfo.FQTVNumber,
                    },
                },
                // 6. Customer TextModulesData
                TextModulesData = new List<TextModuleData>
                {
                    //需要多語系要額外設定標題與內容
                    GoogleWalletUtility.CreateTextModuleData(
                        BaseCardTemplate.BaggagesValues.FieldName,
                        "Baggage Info",
                        string.Join(Environment.NewLine, passengerInfo.BaggagesValues)
                    ),
                    GoogleWalletUtility.CreateTextModuleData(
                        BaseCardTemplate.CodeShare.FieldName,
                        "Code Share",
                        GoogleWalletUtility.FormatFlightNumber(
                            passengerInfo.Marketing.CarrierCode,
                            passengerInfo.Marketing.FlightNumber
                        )
                    ),
                    GoogleWalletUtility.CreateTextModuleData(
                        BaseCardTemplate.BookingClass.FieldName,
                        "Booking Class",
                        passengerInfo.BookingClass
                    ),
                    GoogleWalletUtility.CreateTextModuleData(
                        BaseCardTemplate.SpecialMealCode.FieldName,
                        "Special Meal",
                        passengerInfo.SpecialMealCode
                    ),
                    GoogleWalletUtility.CreateTextModuleData(
                        BaseCardTemplate.AdditionalTextString.FieldName,
                        "Additional Text String",
                        passengerInfo.AdditionalTextString
                    ),
                },
                // 7. Customer LinksModulesData
                LinksModuleData = null,
            };
            return flightObject;
        }

        #endregion

        #region 產生 JWT 鏈接
        public string GetBoardingPassesJwtToken(string flightClassId, string flightObjectId)
        {
            return _googleWalletHandler.GetJwtToken(
                $"{_issuerId}.{flightClassId}",
                $"{_issuerId}.{flightObjectId}",
                "boardingpasses"
            );
        }

        public string GetBoardingPassesJwtToken(string flightObjectId)
        {
            return _googleWalletHandler.GetJwtToken(
                $"{_issuerId}.{flightObjectId}",
                "boardingpasses"
            );
        }

        #endregion

        #region Class Resource
        public async Task<FlightClass> GetFlightClassByClassIdAsync(string classId)
        {
            return await GetFlightClassByResourceIdAsync($"{_issuerId}.{classId}");
        }

        /// <summary>
        /// GetClass By ResourceId(FlightClass.Id)
        /// </summary>
        /// <param name="resourceId">格式: IssuerId,ClassId</param>
        /// <returns></returns>
        public async Task<FlightClass> GetFlightClassByResourceIdAsync(string resourceId)
        {
            return await _googleWalletHandler.FlightWallet.ClassResource.GetAsync(resourceId);
        }

        public async Task<FlightClass> InsertFlightClassAsync(FlightClass flightClass)
        {
            return await _googleWalletHandler.FlightWallet.ClassResource.InsertAsync(flightClass);
        }

        public async Task<FlightClass> UpdateFlightClassAsync(FlightClass flightClass)
        {
            flightClass.ReviewStatus = ReviewStatus.UNDER_REVIEW.ToString();
            return await _googleWalletHandler.FlightWallet.ClassResource.UpdateAsync(flightClass);
        }

        public async Task<FlightClass> PatchFlightClassAsync(FlightClass flightClass)
        {
            flightClass.ReviewStatus = ReviewStatus.UNDER_REVIEW.ToString();
            return await _googleWalletHandler.FlightWallet.ClassResource.PatchAsync(flightClass);
        }

        public async Task<FlightClass> AddFlightClassMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        )
        {
            return await _googleWalletHandler.FlightWallet.ClassResource.AddMessageAsync(
                addMessageRequest,
                resourceId
            );
        }
        #endregion

        #region Object Resource
        public async Task<FlightObject> GetFlightObjectByObjectIdAsync(string objectId)
        {
            return await GetFlightObjectByResourceIdAsync($"{_issuerId}.{objectId}");
        }

        public async Task<FlightObject> GetFlightObjectByResourceIdAsync(string resourceId)
        {
            return await _googleWalletHandler.FlightWallet.ObjectResource.GetAsync(resourceId);
        }

        public async Task<FlightObject> InsertFlightObjectAsync(FlightObject flightObject)
        {
            return await _googleWalletHandler.FlightWallet.ObjectResource.InsertAsync(flightObject);
        }

        public async Task<FlightObject> UpdateFlightObjectAsync(FlightObject flightObject)
        {
            return await _googleWalletHandler.FlightWallet.ObjectResource.UpdateAsync(flightObject);
        }

        public async Task<FlightObject> PatchFlightObjectAsync(FlightObject flightObject)
        {
            flightObject.HeroImage = null;
            return await _googleWalletHandler.FlightWallet.ObjectResource.PatchAsync(flightObject);
        }

        public async Task<FlightObject> AddFlightObjectMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        )
        {
            return await _googleWalletHandler.FlightWallet.ObjectResource.AddMessageAsync(
                addMessageRequest,
                resourceId
            );
        }

        public async Task<FlightObject> ExpireFlightObjectAsync(string objectId)
        {
            return await ExpireFlightObjectByResourceIdAsync(
                $"{_googleWalletHandler.WalletSettings.IssuerId}.{objectId}"
            );
        }

        public async Task<FlightObject> ExpireFlightObjectByResourceIdAsync(string resourceId)
        {
            return await _googleWalletHandler.FlightWallet.ObjectResource.ExpireObjectAsync(
                resourceId
            );
        }

        public async Task<FlightObject> UpdateFlightObjectStatusAsync(
            string objectId,
            string objectState
        )
        {
            return await UpdateFlightObjectStatusByResourceIdAsync(
                $"{_googleWalletHandler.WalletSettings.IssuerId}.{objectId}",
                objectState
            );
        }

        public async Task<FlightObject> UpdateFlightObjectStatusByResourceIdAsync(
            string resourceId,
            string objectState
        )
        {
            return await _googleWalletHandler.FlightWallet.ObjectResource.UpdateObjectStatusAsync(
                resourceId,
                objectState
            );
        }
        #endregion
        #endregion
    }
}
