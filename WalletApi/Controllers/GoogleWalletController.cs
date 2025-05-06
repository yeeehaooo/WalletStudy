using Google.Apis.Walletobjects.v1.Data;
using GoogleWalletApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WalletLibrary.Base.Define;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Define.Flight;
using WalletLibrary.GoogleWallet.Settings;

namespace GoogleWalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleWalletController : Controller
    {
        /// <summary>華航的 Google Wallet 處理器。</summary>
        private readonly IGoogleWalletHandler ChinaairlinesHandler;
        /// <summary>華航的 Google Wallet 處理器。</summary>
        private readonly IGoogleWalletHandler MandarinAirlinesHandler;
        /// <summary>包含所有公司設定的集合。</summary>
        //private readonly CompanyGoogleWalletSettings _googleWalletCompanySettings;

        public GoogleWalletController(
            [FromKeyedServices(IATADefine.CI)] IGoogleWalletHandler chinaairlinesHandler, [FromKeyedServices(IATADefine.AE)] IGoogleWalletHandler mandarinAirlinesHandler
        )
        {
            ChinaairlinesHandler = chinaairlinesHandler;
            MandarinAirlinesHandler = mandarinAirlinesHandler;
        }

        [HttpPost("GetClass")]
        public async Task<IActionResult> GetClass(string classId)
        {
            return Ok(
                await ChinaairlinesHandler.FlightWallet.GetClassAsync($"{ChinaairlinesHandler.WalletSettings.IssuerId}.{classId}")
                );
        }

        [HttpPost("GetObject")]
        public async Task<IActionResult> GetObject(string objectId)
        {
            return Ok(await ChinaairlinesHandler.FlightWallet.GetObjectAsync($"{ChinaairlinesHandler.WalletSettings.IssuerId}.{objectId}"));
        }


        [HttpPost("CreateClass")]
        public async Task<IActionResult> CreateClass(FlightClassInfo flightClassInfo)
        {
            var getFlightClassRequest = await ChinaairlinesHandler.FlightWallet.GetClassAsync(
                flightClassInfo.ClassId
            );
            if (getFlightClassRequest != null)
                throw new Exception($"FlightClass {flightClassInfo.ClassId} already exists!");
            FlightClass newClass = new FlightClass
            {
                Id = $"{this.ChinaairlinesHandler.WalletSettings.IssuerId}.{flightClassInfo.ClassId}",
                IssuerName = this.ChinaairlinesHandler.WalletSettings.IssuerName,
                // Note: reviewStatus must be 'UNDER_REVIEW' or 'DRAFT' for updates
                ReviewStatus = FlightClassDefine.ReviewStatus.UnderReview.GetDescription(),
                LocalScheduledDepartureDateTime =
                    flightClassInfo.LocalScheduledDepartureDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                FlightHeader = new FlightHeader
                {
                    Carrier = new FlightCarrier
                    {
                        CarrierIataCode = flightClassInfo.CarrierIataCode,
                    },
                    FlightNumber = flightClassInfo.FlightNumber,
                },
                Origin = new AirportInfo
                {
                    AirportIataCode = flightClassInfo.OriginAirportIataCode,
                    Terminal = flightClassInfo.OriginTerminal,
                    Gate = flightClassInfo.OriginGate,
                },
                Destination = new AirportInfo
                {
                    AirportIataCode = flightClassInfo.DestinationAirportIataCode,
                    Terminal = flightClassInfo.DestinationTerminal,
                    Gate = flightClassInfo.DestinationGate,
                },
            };
            return Ok(await ChinaairlinesHandler.FlightWallet.InsertClassAsync(newClass));
        }

        [HttpPost("UpdateClass")]
        public async Task<IActionResult> UpdateClass(FlightClassInfo flightClassInfo)
        {
            var flightClass = await ChinaairlinesHandler.FlightWallet.GetClassAsync(flightClassInfo.ClassId);
            if (flightClass == null)
                throw new Exception($"can not find flightclass: {flightClassInfo.ClassId}");
            // Note: reviewStatus must be 'UNDER_REVIEW' or 'DRAFT' for updates
            flightClass.ReviewStatus = FlightClassDefine.ReviewStatus.UnderReview.GetDescription();
            flightClass.LocalScheduledDepartureDateTime =
                flightClassInfo.LocalScheduledDepartureDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
            flightClass.FlightHeader = new FlightHeader
            {
                Carrier = new FlightCarrier { CarrierIataCode = flightClassInfo.CarrierIataCode },
                FlightNumber = flightClassInfo.FlightNumber,
            };
            flightClass.Origin = new AirportInfo
            {
                AirportIataCode = flightClassInfo.OriginAirportIataCode,
                Terminal = flightClassInfo.OriginTerminal,
                Gate = flightClassInfo.OriginGate,
            };
            flightClass.Destination = new AirportInfo
            {
                AirportIataCode = flightClassInfo.DestinationAirportIataCode,
                Terminal = flightClassInfo.DestinationTerminal,
                Gate = flightClassInfo.DestinationGate,
            };

            return Ok(await ChinaairlinesHandler.FlightWallet.UpdateClassAsync(flightClass));
        }

        [HttpPost("AddClassMessage")]
        public async Task<IActionResult> AddClassMessage(string classId)
        {
            var flightClass = await ChinaairlinesHandler.FlightWallet.GetClassAsync($"{ChinaairlinesHandler.WalletSettings.IssuerId}.{classId}");
            if (flightClass == null)
                throw new Exception($"can not find flightclass: {classId}");
            // Note: reviewStatus must be 'UNDER_REVIEW' or 'DRAFT' for updates
            FlightClass newFlightClass = new FlightClass();
            newFlightClass.Id = flightClass.Id;
            newFlightClass.ReviewStatus = FlightClassDefine.ReviewStatus.UnderReview.GetDescription();
            newFlightClass.Messages = new List<Message>()
            {
                new()
                {
                    Id = "delay_notice",
                    LocalizedHeader = new LocalizedString
                    {
                        DefaultValue = new TranslatedString
                        {
                            Language = "en-US",
                            Value = "Flight Delay Notice",
                        },
                        TranslatedValues = new List<TranslatedString>
                        {
                            new() { Language = "zh-TW", Value = "航班延誤通知" },
                            new() { Language = "ja-JP", Value = "遅延のお知らせ" },
                        },
                    },
                    LocalizedBody = new LocalizedString
                    {
                        DefaultValue = new TranslatedString
                        {
                            Language = "en-US",
                            Value =
                                "Your flight has been delayed by 1.5 hour. We apologize for the inconvenience.",
                        },
                        TranslatedValues = new List<TranslatedString>
                        {
                            new()
                            {
                                Language = "zh-TW",
                                Value = "您的航班延誤 1.5 小時，造成不便敬請見諒。",
                            },
                            new()
                            {
                                Language = "ja-JP",
                                Value =
                                    "ご搭乗便は1.5時間遅延しています。ご不便をおかけして申し訳ありません。",
                            },
                        },
                    },
                },
                new()
                {
                    Id = "delay_notice",
                    LocalizedHeader = new LocalizedString
                    {
                        DefaultValue = new TranslatedString
                        {
                            Language = "en-US",
                            Value = "Flight Delay Notice",
                        },
                        TranslatedValues = new List<TranslatedString>
                        {
                            new() { Language = "zh-TW", Value = "航班延誤通知" },
                            new() { Language = "ja-JP", Value = "遅延のお知らせ" },
                        },
                    },
                    LocalizedBody = new LocalizedString
                    {
                        DefaultValue = new TranslatedString
                        {
                            Language = "en-US",
                            Value =
                                "Your flight has been delayed by 2 hour. We apologize for the inconvenience.",
                        },
                        TranslatedValues = new List<TranslatedString>
                        {
                            new()
                            {
                                Language = "zh-TW",
                                Value = "您的航班延誤 2 小時，造成不便敬請見諒。",
                            },
                            new()
                            {
                                Language = "ja-JP",
                                Value =
                                    "ご搭乗便は2時間遅延しています。ご不便をおかけして申し訳ありません。",
                            },
                        },
                    },
                },
            };

            return Ok(await ChinaairlinesHandler.FlightWallet.PatchClassAsync(newFlightClass));
        }

        [HttpPost("CreateObject")]
        public async Task<IActionResult> CreateObject(FlightObjectInfo flightObjectInfo)
        {
            FlightObject newObject = new FlightObject
            {
                // 卡片的唯一識別碼，格式為「發行者ID.物件ID」
                Id = $"{ChinaairlinesHandler.WalletSettings.IssuerId}.{flightObjectInfo.Id}",

                // 對應的班機類別ID，格式為「發行者ID.類別ID」
                ClassId = $"{ChinaairlinesHandler.WalletSettings.IssuerId}.{flightObjectInfo.ClassId}",

                // 卡片狀態，例如 ACTIVE、INACTIVE 等
                State = FlightObjectDefine.State.ACTIVE,

                // 頁首圖（大圖）設定，可提升卡片視覺效果
                HeroImage = new Image
                {
                    SourceUri = new ImageUri { Uri = ChinaairlinesHandler.WalletSettings.HeroImageUri },
                    ContentDescription = new LocalizedString
                    {
                        DefaultValue = new TranslatedString
                        {
                            Language = "en-US",
                            Value = ChinaairlinesHandler.WalletSettings.HeroImageDescription,
                        },
                    },
                },
                //自訂多語系文字模組，例如提醒資訊等
                TextModulesData = new List<TextModuleData>
                {
                    new TextModuleData
                    {
                        Id = "boarding_reminder",
                        LocalizedHeader = new LocalizedString
                        {
                            DefaultValue = new TranslatedString
                            {
                                Language = "en-US",
                                Value = "Boarding Reminder",
                            },
                            TranslatedValues = new List<TranslatedString>
                            {
                                new() { Language = "zh-TW", Value = "登機提醒" },
                                new() { Language = "ja-JP", Value = "搭乗のリマインダー" },
                            },
                        },
                        LocalizedBody = new LocalizedString
                        {
                            DefaultValue = new TranslatedString
                            {
                                Language = "en-US",
                                Value = "Please arrive 30 minutes before departure.",
                            },
                            TranslatedValues = new List<TranslatedString>
                            {
                                new()
                                {
                                    Language = "zh-TW",
                                    Value = "請於起飛前 30 分鐘完成報到，並攜帶有效證件。",
                                },
                                new()
                                {
                                    Language = "ja-JP",
                                    Value = "出発の30分前までにご到着ください。",
                                },
                            },
                        },
                    },
                },

                // 可點擊的連結模組，例如「航班資訊」、「條款」等
                LinksModuleData = new LinksModuleData
                {
                    Uris = ChinaairlinesHandler.WalletSettings
                        .LinkModuleUris.Select(uri => new Google.Apis.Walletobjects.v1.Data.Uri
                        {
                            UriValue = uri.UriValue,
                            Description = uri.Description,
                            Id = uri.Id,
                        })
                        .ToList(),
                },

                // 小圖模組，通常用於品牌 Logo 或其他輔助圖片
                ImageModulesData = new List<ImageModuleData>
                {
                    new ImageModuleData
                    {
                        MainImage = new Image
                        {
                            SourceUri = new ImageUri { Uri = ChinaairlinesHandler.WalletSettings.ImageModuleUri },
                            ContentDescription = new LocalizedString
                            {
                                DefaultValue = new TranslatedString
                                {
                                    Language = "en-US",
                                    Value = ChinaairlinesHandler.WalletSettings.ImageModuleDescription,
                                },
                            },
                        },
                        Id = "IMAGE_MODULE_ID",
                    },
                },

                // 條碼模組，使用 QR_CODE 並設定值
                Barcode = new Barcode { Type = "QR_CODE", Value = flightObjectInfo.BarcodeValue },

                // 卡券顯示的位置，當用戶靠近此地點時會出現在鎖定畫面（支援多筆）
                Locations = new List<LatLongPoint>
                {
                    new LatLongPoint
                    {
                        Latitude = ChinaairlinesHandler.WalletSettings.DefaultLatitude,
                        Longitude = ChinaairlinesHandler.WalletSettings.DefaultLongitude,
                    },
                },

                // 乘客名稱
                PassengerName = flightObjectInfo.PassengerName,

                // 登機與座位資訊
                BoardingAndSeatingInfo = new BoardingAndSeatingInfo
                {
                    BoardingGroup = flightObjectInfo.BoardingGroup,
                    SeatNumber = flightObjectInfo.SeatNumber,
                },

                // 預約資訊，例如訂位代號
                ReservationInfo = new ReservationInfo
                {
                    ConfirmationCode = flightObjectInfo.ConfirmationCode,
                },
                
                //[有效期間]設定後，Google Wallet 會根據時間自動?顯示或隱藏卡券，過期後顯示為已失效。
                //若未設定 validTimeInterval，卡券預設會一直顯示為有效
                //此欄位主要是給系統判斷卡券是否要「變成灰色／不可使用」
                ValidTimeInterval = new TimeInterval
                {
                    //Start：卡券開始生效的時間（UTC）
                    Start = new Google.Apis.Walletobjects.v1.Data.DateTime
                    {
                        Date = "2025-04-29T00:00:00Z",
                    },
                    //End：卡券失效的時間（UTC）
                    End = new Google.Apis.Walletobjects.v1.Data.DateTime
                    {
                        Date = "2025-04-30T23:59:59Z",
                    },
                },
            };
            return Ok(await ChinaairlinesHandler.FlightWallet.InsertObjectAsync(newObject));
        }

        [HttpPost("UpdateObject")]
        public async Task<IActionResult> UpdateObject(FlightObjectInfo flightObjectInfo)
        {
            var flightOjbect = await ChinaairlinesHandler.FlightWallet.GetObjectAsync(flightObjectInfo.Id);
            if (flightOjbect == null)
                throw new Exception($"can not find flightobject: {flightObjectInfo.Id}");
            flightOjbect.PassengerName = flightObjectInfo.PassengerName;
            flightOjbect.BoardingAndSeatingInfo.SeatNumber = flightObjectInfo.SeatNumber;
            flightOjbect.BoardingAndSeatingInfo.BoardingGroup = flightObjectInfo.BoardingGroup;
            flightOjbect.Barcode.Value = flightObjectInfo.BarcodeValue;
            flightOjbect.ReservationInfo = new ReservationInfo
            {
                ConfirmationCode = flightObjectInfo.ConfirmationCode,
            };
            flightOjbect.ValidTimeInterval = new TimeInterval
            {
                //Start：卡券開始生效的時間（UTC）
                Start = new Google.Apis.Walletobjects.v1.Data.DateTime
                {
                    Date = flightObjectInfo.StartValidTime,
                },
                //End：卡券失效的時間（UTC）
                End = new Google.Apis.Walletobjects.v1.Data.DateTime
                {
                    Date = flightObjectInfo.EndValidTime,
                },
            };
            if (
                System.DateTime.TryParse(flightObjectInfo.EndValidTime, out System.DateTime endTime)
                && System.DateTime.Now >= endTime
            )
                flightOjbect.State = FlightObjectDefine.State.EXPIRED;
            return Ok(await ChinaairlinesHandler.FlightWallet.UpdateObjectAsync(flightOjbect));
        }


    }
}
