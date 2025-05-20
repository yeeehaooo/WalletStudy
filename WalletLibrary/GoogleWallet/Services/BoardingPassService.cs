using System.Net;
using Google.Apis.Walletobjects.v1.Data;
using Microsoft.Extensions.Logging;
using WalletLibrary.Base.Models;
using WalletLibrary.DTO;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Define.Flight;
using WalletLibrary.GoogleWallet.Services.Interfaces;
using WalletLibrary.GoogleWallet.Settings;
using WalletLibrary.GoogleWallet.WalletTypes.Flight.Models;
using static WalletLibrary.GoogleWallet.Define.Flight.FlightClassDefine;
using GoogleApiUri = Google.Apis.Walletobjects.v1.Data.Uri;

namespace WalletLibrary.GoogleWallet.Services
{
    public class BoardingPassService : IBoardingPassService
    {
        private readonly ILogger<BoardingPassService> _logger;

        private readonly IGoogleWalletHandler _googleWalletHandler;

        private readonly string _issuerId;

        private readonly string _issuerName;

        private GoogleWalletSettings _settings;

        public BoardingPassService(
            ILogger<BoardingPassService> logger,
            IGoogleWalletHandler googleWalletHandler
        )
        {
            _logger = logger;
            _googleWalletHandler = googleWalletHandler;
            _settings = googleWalletHandler.WalletSettings;
            _issuerId = googleWalletHandler.WalletSettings.IssuerId;
            _issuerName = googleWalletHandler.WalletSettings.IssuerName;
        }

        /// <summary>
        /// 將 BoardingPassWalletModel 轉換為 Google Wallet 的 FlightClass 物件 範本<br/>
        /// 設定:<br/>
        /// 1. ReviewStatus 狀態<br/>
        /// 2. 背景底色模組<br/>
        /// 3. 主要圖片網址模組<br/>
        /// 4. 連結模組<br/>
        /// 5. 通知訊息模組<br/>
        /// 6. 文字區塊模組<br/>
        /// 7. 出發機場資訊<br/>
        /// 8. 抵達機場資訊<br/>
        /// 9. 預定出發時間 ISO 8601<br/>
        /// 10. 預定抵達時間 ISO 8601<br/>
        /// 11. 登機時間 ISO 8601<br/>
        /// 12. 航班號碼<br/>
        /// 13. 主承運航空公司資訊(IATA、Name、Logo等等)<br/>
        /// </summary>
        /// <param name="classId">航班物件ID, 必填 唯一值</param>
        /// <param name="linksModuleData">連結模組</param>
        /// <param name="messages">通知訊息模組</param>
        /// <param name="textModuleDatas">文字區塊模組</param>
        /// <returns></returns>
        private FlightClass BuildFlightClass(BoardingPassWalletModel boardingPassWallet)
        {
            FlightClass flightClass = new FlightClass();

            flightClass.Id = $"{_settings.IssuerId}.{boardingPassWallet.Flight.ClassSuffix}";
            flightClass.IssuerName = _settings.IssuerName;

            // 1. Note: ReviewStatus must be 'UNDER_REVIEW' or 'DRAFT'
            // 審核狀態（預設為 "UNDER_REVIEW"，若已申請 正式版 production 可改為 "APPROVED"）
            flightClass.ReviewStatus = ReviewStatus.UNDER_REVIEW;

            // 2. 背景底色模組
            flightClass.HexBackgroundColor = _settings.HexBackgroundColor;

            // 3. 主要圖片網址模組
            flightClass.HeroImage =
                FlightWalletUtility.BuildImageModule(
                    _settings.HeroImageId,
                    _settings.HeroImageUri,
                    _settings.HeroImageDescription
                ) ?? null;

            // 4. 連結模組
            // 可以設定多組連結 ex 官網, 非必填
            flightClass.LinksModuleData = new LinksModuleData
            {
                Uris = new List<GoogleApiUri>
                {
                    boardingPassWallet.Flight.AirportCheckinInfo?.ToUriModule("AirportCheckinInfo"),
                    boardingPassWallet.Flight.BaggageLocalizedMessage?.ToUriModule(
                        "BaggageLocalizedMessage"
                    ),
                },
            };

            // LocalizedItem  MessageModel放通知區塊ToMessage() 還是 TextModel放文字區塊ToTextModule()
            var reminderInfo = boardingPassWallet.Flight.ReminderMessage.ToTextModule(
                "ReminderMessage"
            );
            var reminderInfo2 = boardingPassWallet.Flight.ReminderMessage.ToTextModule(
                "ReminderMessage2"
            );
            // 5. 通知訊息模組, 非必填
            // 可以設定多組訊息(支援多語系 標題 & 內文)
            // 可以額外設定 起始時間 & 結束時間 & 通知類型
            flightClass.Messages = new List<Message> { };
            // 6. 文字區塊模組, 非必填
            // 可以設定多組文字區塊(支援多語系 標題 & 內文)
            flightClass.TextModulesData = new List<TextModuleData>();
            flightClass.TextModulesData.Add(reminderInfo);
            flightClass.TextModulesData.Add(reminderInfo2);
            // 航班相關設定
            // 7. 出發機場資訊
            flightClass.Origin = new AirportInfo();
            flightClass.Origin.AirportIataCode = boardingPassWallet.Flight.DepartureAirport.IATA; // 出發地機場的 IATA 三碼代碼
            flightClass.Origin.Terminal = boardingPassWallet.Flight.DepartureAirport.Terminal; // 出發航廈編號（例如：T1、T2）
            flightClass.Origin.Gate = boardingPassWallet.Flight.DepartureAirport.Gate; // 登機門號碼
            flightClass.Origin.AirportNameOverride =
                boardingPassWallet.Flight.DepartureAirport.NameOverride?.ToLocalizedString(); // 出發機場名稱複寫 可以設定多語系

            // 8. 抵達機場資訊
            flightClass.Destination = new AirportInfo();
            flightClass.Destination.AirportIataCode = boardingPassWallet.Flight.ArrivalAirport.IATA; // 抵達地機場的 IATA 三碼代碼
            flightClass.Destination.Terminal = boardingPassWallet.Flight.ArrivalAirport.Terminal; // 抵達航廈編號
            flightClass.Destination.Gate = boardingPassWallet.Flight.ArrivalAirport.Gate; // 抵達登機門號碼
            flightClass.Destination.AirportNameOverride =
                boardingPassWallet.Flight.ArrivalAirport.NameOverride?.ToLocalizedString(); // 抵達機場名稱複寫 可以設定多語系

            // 航班時刻資訊（當地時間），格式必須符合 ISO 8601
            // 9. 預定出發時間 ISO 8601
            flightClass.LocalScheduledDepartureDateTime =
                FlightWalletUtility.FormatToIso8601DateTimeString(
                    boardingPassWallet.Flight.DepartureDate,
                    boardingPassWallet.Flight.DepartureTime
                );
            // 10. 預定抵達時間 ISO 8601
            flightClass.LocalScheduledArrivalDateTime =
                FlightWalletUtility.FormatToIso8601DateTimeString(
                    boardingPassWallet.Flight.ArrivalDate,
                    boardingPassWallet.Flight.ArrivalTime
                );
            // 11.登機時間 ISO 8601
            flightClass.LocalBoardingDateTime = FlightWalletUtility.FormatToIso8601DateTimeString(
                boardingPassWallet.Flight.BoardingDate,
                boardingPassWallet.Flight.LatestBoardingTime
            );

            flightClass.FlightHeader = new FlightHeader
            {
                // 12. 航班號碼
                FlightNumber = boardingPassWallet.Flight.Operating.FlightNumber, // 航班號碼（數字）
                FlightNumberDisplayOverride = FlightWalletUtility.FormatFlightNumber(
                    boardingPassWallet.Flight.Operating.CarrierCode,
                    boardingPassWallet.Flight.Operating.FlightNumber
                ), // 顯示用航班號碼（可能包含航空公司代碼）

                // 13. 主承運航空公司資訊
                Carrier = new FlightCarrier
                {
                    CarrierIataCode = boardingPassWallet.Flight.Operating.CarrierCode, // 航空公司 IATA 簡碼
                    //CarrierIcaoCode = "航空公司 ICAO 簡碼", // 航空公司 ICAO 簡碼
                    // 航空公司名稱
                    AirlineName = new LocalizedString
                    {
                        DefaultValue =
                            boardingPassWallet.Flight.Operating.AirLineName.Default.ToDefaultValue(),
                        TranslatedValues =
                            boardingPassWallet.Flight.Operating.AirLineName.TranslatedValues?.ToTranslatedValues(),
                    },
                    // 航空公司 Logo
                    AirlineLogo = boardingPassWallet.Flight.Operating.AirlineLogo?.ToImageModule(
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
            flightClass.ClassTemplateInfo = CreateFlightCardTemplate();

            return flightClass;
        }

        public ClassTemplateInfo CreateFlightCardTemplate()
        {
            var classTemplateInfo = new ClassTemplateInfo();

            classTemplateInfo.CardTemplateOverride = new CardTemplateOverride
            {
                CardRowTemplateInfos = new List<CardRowTemplateInfo>
                {
                    // 第一列：（1. 出發時間、航廈，2. 抵達時間、抵達航廈）
                    new CardRowTemplateInfo
                    {
                        TwoItems = new CardRowTwoItems
                        {
                            StartItem = new TemplateItem
                            {
                                FirstValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference
                                        {
                                            FieldPath = "class.localScheduledDepartureDateTime",
                                            DateFormat = "TIME_ONLY",
                                        }, //
                                    },
                                },
                                SecondValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference { FieldPath = "class.origin.terminal" }, // 出發航廈
                                    },
                                },
                            },
                            EndItem = new TemplateItem
                            {
                                FirstValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference
                                        {
                                            FieldPath = "class.localScheduledArrivalDateTime",
                                            DateFormat = "TIME_ONLY",
                                        },
                                    },
                                },
                                SecondValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference
                                        {
                                            FieldPath = "class.destination.terminal",
                                        },
                                    },
                                },
                            },
                        },
                    },
                    //// 第二列
                    new CardRowTemplateInfo
                    {
                        ThreeItems = new CardRowThreeItems
                        {
                            StartItem = new TemplateItem
                            {
                                FirstValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference
                                        {
                                            FieldPath = "class.localBoardingDateTime",
                                            DateFormat = "TIME_ONLY",
                                        }, // 登機時間
                                    },
                                },
                            },
                            MiddleItem = new TemplateItem
                            {
                                FirstValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference { FieldPath = "class.origin.gate" }, // 出發登機門
                                    },
                                },
                            },
                            EndItem = new TemplateItem
                            {
                                FirstValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference
                                        {
                                            FieldPath =
                                                "object.boardingAndSeatingInfo.boardingGroup",
                                        }, // 登機順序 ()
                                    },
                                },
                                SecondValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference
                                        {
                                            FieldPath = "object.boardingAndSeatingInfo.seatNumber",
                                        }, // 座位 / 區域 (object)
                                    },
                                },
                            },
                        },
                    },
                    // 第三列：
                    new CardRowTemplateInfo
                    {
                        TwoItems = new CardRowTwoItems
                        {
                            StartItem = new TemplateItem
                            {
                                FirstValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference { FieldPath = "object.passengerName" }, // 乘客姓名
                                    },
                                },
                            },
                            EndItem = new TemplateItem
                            {
                                FirstValue = new FieldSelector
                                {
                                    Fields = new List<FieldReference>
                                    {
                                        new FieldReference
                                        {
                                            FieldPath = "object.textModulesData['CodeShare']",
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };
            //return classTemplateInfo;
            classTemplateInfo.DetailsTemplateOverride = new DetailsTemplateOverride
            {
                DetailsItemInfos = new List<DetailsItemInfo>
                {
                    new DetailsItemInfo
                    {
                        Item = new TemplateItem
                        {
                            FirstValue = new FieldSelector
                            {
                                Fields = new List<FieldReference>
                                {
                                    new FieldReference
                                    {
                                        FieldPath =
                                            "class.linksModuleData.uris['AirportCheckinInfo']",
                                    },
                                },
                            },
                        },
                    },
                    new DetailsItemInfo
                    {
                        Item = new TemplateItem
                        {
                            FirstValue = new FieldSelector
                            {
                                Fields = new List<FieldReference>
                                {
                                    new FieldReference
                                    {
                                        FieldPath =
                                            "class.linksModuleData.uris['BaggageLocalizedMessage']",
                                    },
                                },
                            },
                        },
                    },
                    new DetailsItemInfo
                    {
                        Item = new TemplateItem
                        {
                            PredefinedItem = "frequentFlyerProgramNameAndNumber",
                        },
                    },
                    new DetailsItemInfo
                    {
                        Item = new TemplateItem
                        {
                            FirstValue = new FieldSelector
                            {
                                Fields = new List<FieldReference>
                                {
                                    new FieldReference
                                    {
                                        FieldPath = "object.reservationInfo.eticketNumber",
                                    },
                                },
                            },
                        },
                    },
                    new DetailsItemInfo
                    {
                        Item = new TemplateItem
                        {
                            FirstValue = new FieldSelector
                            {
                                Fields = new List<FieldReference>
                                {
                                    new FieldReference
                                    {
                                        FieldPath = "object.reservationInfo.confirmationCode",
                                    },
                                },
                            },
                        },
                    },
                    new DetailsItemInfo
                    {
                        Item = new TemplateItem
                        {
                            FirstValue = new FieldSelector
                            {
                                Fields = new List<FieldReference>
                                {
                                    new FieldReference
                                    {
                                        FieldPath = "object.textModulesData['BaggageInfo']",
                                    },
                                },
                            },
                        },
                    },
                    new DetailsItemInfo
                    {
                        Item = new TemplateItem
                        {
                            FirstValue = new FieldSelector
                            {
                                Fields = new List<FieldReference>
                                {
                                    new FieldReference
                                    {
                                        FieldPath = "object.textModulesData['CodeShare']",
                                    },
                                },
                            },
                        },
                    },
                },
            };

            //classTemplateInfo.CardBarcodeSectionDetails = new CardBarcodeSectionDetails
            //{
            //    FirstTopDetail = new BarcodeSectionDetail
            //    {
            //        FieldSelector = new FieldSelector
            //        {
            //            Fields = new List<FieldReference>
            //            {
            //                new FieldReference { FieldPath = "" },
            //                new FieldReference { FieldPath = "" },
            //            },
            //        },
            //    },
            //    SecondTopDetail = new BarcodeSectionDetail
            //    {
            //        FieldSelector = new FieldSelector
            //        {
            //            Fields = new List<FieldReference>
            //            {
            //                new FieldReference { FieldPath = "" },
            //                new FieldReference { FieldPath = "" },
            //            },
            //        },
            //    },
            //    FirstBottomDetail = new BarcodeSectionDetail
            //    {
            //        FieldSelector = new FieldSelector
            //        {
            //            Fields = new List<FieldReference>
            //            {
            //                new FieldReference { FieldPath = "" },
            //                new FieldReference { FieldPath = "" },
            //            },
            //        },
            //    },
            //};
            return classTemplateInfo;
        }

        /// <summary>
        /// 建立 Google Wallet 的 FlightObject 物件（搭配 FlightClass）<br/>
        /// 設定：<br/>
        /// 1. Class ID 與 Object ID<br/>
        /// 2. 使用者資訊（姓名）<br/>
        /// 3. 座位資訊（座位號碼、艙等）<br/>
        /// 4. 條碼資訊（登機證用）<br/>
        /// 5. 登機群組、登機區域資訊<br/>
        /// 6. 中央航線標籤（例如：TPE ✈ NRT）<br/>
        /// </summary>
        /// <param name="boardingPassWallet">登機證模型</param>
        /// <returns></returns>
        public FlightObject BuildFlightObject(BoardingPassWalletModel boardingPassWallet)
        {
            var objectId = $"{_settings.IssuerId}.{boardingPassWallet.Passenger.ObjectSuffix}";

            var flightObject = new FlightObject
            {
                // 1. Class ID 與 Object ID
                Id = objectId,
                ClassId = $"{_settings.IssuerId}.{boardingPassWallet.Passenger.ClassSuffix}",
                State = FlightObjectDefine.State.ACTIVE, // ACTIVE, INACTIVE, EXPIRED, etc.
                // 2. 使用者資訊（姓名）
                PassengerName = boardingPassWallet.Passenger.PassengerName,
                // 3. 座位資訊（座位號碼、艙等）
                BoardingAndSeatingInfo = new BoardingAndSeatingInfo
                {
                    // 登機門 (BoardingDoor)
                    // 備註: 若航空公司依座艙分流登機，則不設定此欄位。
                    // BoardingDoor 可以依實際需求設定 boardingPassWallet 內相應欄位 (此處未設定)

                    // 登機分組 (BoardingGroup)
                    // 通常代表旅客的登機分組（如 Zone1, Zone2）
                    BoardingGroup = boardingPassWallet.Passenger.BoardingZone,

                    // 登機順序編號 (BoardingPosition)
                    // 一般指示旅客的登機優先順序編號
                    //BoardingPosition = boardingPassWallet.Passenger.BoardingZone,

                    // 序號 (SequenceNumber)
                    // 旅客的報到序號，通常格式如 "SEQ:002"
                    SequenceNumber = boardingPassWallet.Passenger.SecurityNumber,

                    // 座艙等級 (SeatClass)
                    // 例如：BUSINESS CLASS、ECONOMY 等
                    SeatClass = boardingPassWallet.Passenger.BookingClass,

                    // 座位號碼 (SeatNumber)
                    // 例如 5G、12A 等
                    SeatNumber = boardingPassWallet.Passenger.SeatNumber,

                    // 特殊登機權益圖像 (BoardingPrivilegeImage)
                    // 根據 FQTVAllianceTier 字串決定是否顯示特權圖標
                    // 注意：這裡 boardingPassWallet.Passenger.FQTVAllianceTier 應為圖片 URL
                    BoardingPrivilegeImage = !boardingPassWallet.Passenger.IsSkyPriority
                        ? null
                        : FlightWalletUtility.BuildImageModule(
                            "SkyPriority",
                            _settings.BoardingPrivilegeImage,
                            "SKY PRIORITY"
                        ),
                },
                SecurityProgramLogo = !boardingPassWallet.Passenger.IsTSAPreCheck
                    ? null
                    : FlightWalletUtility.BuildImageModule(
                        "TSAPreCheck",
                        _settings.SecurityProgramLogo,
                        "TSA PRE"
                    ),
                // 4.條碼資訊（QR_Code / AZTEC / PDF417 / CODE128）
                Barcode = new Barcode
                {
                    Type = "QR_CODE",
                    Value = boardingPassWallet.Passenger.ElecTicketNumber, // Barcode Value 未定義
                    AlternateText = boardingPassWallet.Channel,
                    RenderEncoding = "UTF-8",
                    ShowCodeText = new LocalizedString
                    {
                        DefaultValue = new TranslatedString
                        {
                            Language = "zh-TW",
                            Value = "點擊以顯示QR CODE",
                        },
                        TranslatedValues = new List<TranslatedString>
                        {
                            new TranslatedString
                            {
                                Language = "en-US",
                                Value = "Click to Show QR CODE",
                            },
                        },
                    },
                },

                ReservationInfo = new ReservationInfo
                {
                    ConfirmationCode = boardingPassWallet.Passenger.ConfirmationCode,
                    EticketNumber = boardingPassWallet.Passenger.ElecTicketNumber,
                    FrequentFlyerInfo = new FrequentFlyerInfo
                    {
                        FrequentFlyerProgramName =
                            boardingPassWallet.Passenger.FQTVTierDescription.ToLocalizedString(),
                        FrequentFlyerNumber = boardingPassWallet.Passenger.FQTVNumber,
                    },
                },
                TextModulesData = new List<TextModuleData>
                {
                    boardingPassWallet.Passenger.BaggageInfo.ToTextModule("BaggageInfo"),
                    //boardingPassWallet.Passenger.BaggageInfo.ToTextModule("CodeShare"),
                    FlightWalletUtility.ToDefaultTextModuleData(
                        "CodeShare",
                        "Code Share Info",
                        string.Join(
                            Environment.NewLine,
                            new List<string>
                            {
                                $"{FlightWalletUtility.FormatFlightNumber(boardingPassWallet.Flight.Operating.CarrierCode, boardingPassWallet.Flight.Operating.FlightNumber)}．{boardingPassWallet.Flight.BoardingDate}．{boardingPassWallet.Passenger.CabinComments}",
                                $"CODE SHARE {FlightWalletUtility.FormatFlightNumber(boardingPassWallet.Passenger.Marketing.CarrierCode, boardingPassWallet.Passenger.Marketing.FlightNumber)}",
                            }
                        )
                    ),
                    ////
                    //),
                },
            };
            return flightObject;
        }

        #region JWT
        public async Task<BaseReponseModel<string>> GetJwtToken(
            string flightClassId,
            string flightObjectId
        )
        {
            try
            {
                var jwtToken = await _googleWalletHandler.FlightWallet.GetJwtToken(
                    $"{_issuerId}.{flightClassId}",
                    $"{_issuerId}.{flightObjectId}"
                );
                return new BaseReponseModel<string>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = jwtToken,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Class[{ClassId}],Object[{flightObjectId}] Get Error: {ErrorMessage}",
                    flightClassId,
                    flightObjectId,
                    ex.Message
                );
                return new BaseReponseModel<string>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message =
                        $"Class[{flightClassId}],Object[{flightObjectId}] Get Error: {ex.Message}",
                };
            }
        }

        #endregion

        #region ClassRepository
        public Task<BaseReponseModel<FlightClass>> GetClassByClassIdAsync(string classId)
        {
            return GetClassByResourceIdAsync($"{_issuerId}.{classId}");
        }

        /// <summary>
        /// GetClass By ResourceId(FlightClass.Id)
        /// </summary>
        /// <param name="resourceId">格式: IssuerId,ClassId</param>
        /// <returns></returns>
        public async Task<BaseReponseModel<FlightClass>> GetClassByResourceIdAsync(
            string resourceId
        )
        {
            try
            {
                var flightClass = await _googleWalletHandler.FlightWallet.ClassRepository.GetAsync(
                    resourceId
                );
                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = flightClass,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Class[{ResourceId}] Get Error: {ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Class[{resourceId}] Get Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightClass>> InsertClassAsync(
            BoardingPassWalletModel boardingPassModel
        )
        {
            string resourceId = $"{_issuerId}.{boardingPassModel.Flight.ClassSuffix}";
            try
            {
                var flightcalss = BuildFlightClass(boardingPassModel);
                return await InsertClassAsync(flightcalss);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Class[{ResourceId}] Insert Error: {ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Class[{resourceId}] Insert Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightClass>> InsertClassAsync(FlightClass flightClass)
        {
            try
            {
                var resultFlightClass =
                    await _googleWalletHandler.FlightWallet.ClassRepository.InsertAsync(
                        flightClass
                    );

                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightClass,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Class[{ResourceId}] Insert Error: {ErrorMessage}",
                    flightClass.Id,
                    ex.Message
                );
                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Class[{flightClass.Id}] Insert Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightClass>> UpdateClassAsync(FlightClass flightClass)
        {
            try
            {
                flightClass.ReviewStatus = ReviewStatus.UNDER_REVIEW;
                var resultFlightClass =
                    await _googleWalletHandler.FlightWallet.ClassRepository.UpdateAsync(
                        flightClass
                    );

                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightClass,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Class[{ResourceId}] Update Error: {ErrorMessage}",
                    flightClass.Id,
                    ex.Message
                );
                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Class[{flightClass.Id}] Update Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightClass>> PatchClassAsync(FlightClass flightClass)
        {
            try
            {
                flightClass.ReviewStatus = ReviewStatus.UNDER_REVIEW;
                var resultFlightClass =
                    await _googleWalletHandler.FlightWallet.ClassRepository.PatchAsync(flightClass);

                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightClass,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Class[{ResourceId}] Patch Error: {ErrorMessage}",
                    flightClass.Id,
                    ex.Message
                );
                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Class[{flightClass.Id}] Patch Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightClass>> AddClassMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        )
        {
            try
            {
                var resultFlightClass =
                    await _googleWalletHandler.FlightWallet.ClassRepository.AddMessageAsync(
                        addMessageRequest,
                        resourceId
                    );

                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightClass,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Class[{ResourceId}] Add Message Error: {ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightClass>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Class[{resourceId}] Add Message Error: {ex.Message}",
                };
            }
        }
        #endregion


        #region ClassRepository

        public Task<BaseReponseModel<FlightObject>> GetObjectByObjectIdAsync(string objectId)
        {
            return GetObjectByResourceIdAsync($"{_issuerId}.{objectId}");
        }

        public async Task<BaseReponseModel<FlightObject>> GetObjectByResourceIdAsync(
            string resourceId
        )
        {
            try
            {
                var flightObject =
                    await _googleWalletHandler.FlightWallet.ObjectRepository.GetAsync(resourceId);

                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "",
                    Data = flightObject,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Get Error: {ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{resourceId}] Get Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightObject>> InsertObjectAsync(
            BoardingPassWalletModel boardingPassModel
        )
        {
            string resourceId = $"{_issuerId}.{boardingPassModel.Passenger.ObjectSuffix}";
            try
            {
                var flightobject = BuildFlightObject(boardingPassModel);
                return await InsertObjectAsync(flightobject);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Insert Error: {ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{resourceId}] Insert Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightObject>> InsertObjectAsync(
            FlightObject flightObject
        )
        {
            try
            {
                var resultFlightObject =
                    await _googleWalletHandler.FlightWallet.ObjectRepository.InsertAsync(
                        flightObject
                    );

                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightObject,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Insert Error: {ErrorMessage}",
                    flightObject.Id,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{flightObject.Id}] Insert Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightObject>> UpdateObjectAsync(
            FlightObject flightObject
        )
        {
            try
            {
                var resultFlightObject =
                    await _googleWalletHandler.FlightWallet.ObjectRepository.UpdateAsync(
                        flightObject
                    );

                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightObject,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Update Error: {ErrorMessage}",
                    flightObject.Id,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{flightObject.Id}] Update Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightObject>> PatchObjectAsync(
            FlightObject flightObject
        )
        {
            try
            {
                flightObject.HeroImage = null;
                var resultFlightObject =
                    await _googleWalletHandler.FlightWallet.ObjectRepository.PatchAsync(
                        flightObject
                    );

                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightObject,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Patch Error :{ErrorMessage}",
                    flightObject.Id,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{flightObject.Id}] Patch Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightObject>> AddObjectMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        )
        {
            try
            {
                var resultFlightObject =
                    await _googleWalletHandler.FlightWallet.ObjectRepository.AddMessageAsync(
                        addMessageRequest,
                        resourceId
                    );

                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightObject,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Add Message Error :{ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{resourceId}] Add Message Error :{ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightObject>> ExpireObjectAsync(string resourceId)
        {
            try
            {
                var resultFlightObject =
                    await _googleWalletHandler.FlightWallet.ObjectRepository.ExpireObjectAsync(
                        resourceId
                    );

                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightObject,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Expire Error: {ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{resourceId}] Expire Error: {ex.Message}",
                };
            }
        }

        public async Task<BaseReponseModel<FlightObject>> UpdateObjectStatusAsync(
            string resourceId,
            string objectState
        )
        {
            try
            {
                var resultFlightObject =
                    await _googleWalletHandler.FlightWallet.ObjectRepository.UpdateObjectStatusAsync(
                        resourceId,
                        objectState
                    );

                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = resultFlightObject,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Object[{ResourceId}] Update Status Error: {ErrorMessage}",
                    resourceId,
                    ex.Message
                );
                return new BaseReponseModel<FlightObject>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = $"Object[{resourceId}] Update Status Error: {ex.Message}",
                };
            }
        }
        #endregion
    }
}
