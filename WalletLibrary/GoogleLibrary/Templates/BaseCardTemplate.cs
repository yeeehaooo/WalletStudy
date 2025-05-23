using System.Reflection;
using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.Define;
using WalletLibrary.GoogleLibrary.Defines;

namespace WalletLibrary.GoogleLibrary.Templates
{
    /// <summary>
    /// 設定 模板會用的欄位 與對應 Google Api 欄位 跟 欄位ID
    /// </summary>
    public static class BaseCardTemplate
    {
        #region 定義要顯示的資訊欄位

        public static readonly FieldItem FlightNumber = new FieldItem(
            "FlightNumber",
            GoogleDefine.PredefinedItem.FLIGHT_NUMBER_AND_OPERATING_FLIGHT_NUMBER.ToString(),
            null,
            true
        );

        public static readonly FieldItem DepartureDateTime = new FieldItem(
            "LocalScheduledDepartureDateTime",
            "class.localScheduledDepartureDateTime",
            GoogleDefine.DateFormat.TIME_ONLY.ToString()
        );

        public static readonly FieldItem OriginTerminal = new FieldItem(
            "OriginTerminal",
            "class.origin.terminal"
        );

        public static readonly FieldItem ArrivalDateTime = new FieldItem(
            "LocalScheduledArrivalDateTime",
            "class.localScheduledArrivalDateTime",
            GoogleDefine.DateFormat.TIME_ONLY.ToString()
        );

        public static readonly FieldItem DestinationTerminal = new FieldItem(
            "DestinationTerminal",
            "class.destination.terminal"
        );

        public static readonly FieldItem BoardingDateTime = new FieldItem(
            "LocalBoardingDateTime",
            "class.localBoardingDateTime",
            GoogleDefine.DateFormat.DATE_TIME.ToString()
        );

        public static readonly FieldItem OriginGate = new FieldItem(
            "OriginGate",
            "class.origin.gate"
        );

        public static readonly FieldItem BoardingGroup = new FieldItem(
            "BoardingGroup",
            "object.boardingAndSeatingInfo.boardingGroup"
        );

        public static readonly FieldItem SeatNumber = new FieldItem(
            "SeatNumber",
            "object.boardingAndSeatingInfo.seatNumber"
        );

        public static readonly FieldItem PassengerName = new FieldItem(
            "PassengerName",
            "object.passengerName"
        );

        public static readonly FieldItem SequenceNumber = new FieldItem(
            "SequenceNumber",
            "object.boardingAndSeatingInfo.sequenceNumber"
        );

        public static readonly FieldItem AirportCheckinInfo = new FieldItem(
            "AirportCheckinInfo",
            "class.linksModuleData.uris['AirportCheckinInfo']"
        );
        public static readonly FieldItem ReminderMessage = new FieldItem(
            "ReminderMessage",
            "class.textModulesData['ReminderMessage']"
        );

        public static readonly FieldItem BaggageInfo = new FieldItem(
            "BaggageInfo",
            "class.linksModuleData.uris['BaggageInfo']"
        );

        public static readonly FieldItem FlightDate = new FieldItem(
            "LocalScheduledDepartureDateTime",
            "class.localScheduledDepartureDateTime",
            GoogleDefine.DateFormat.YEAR_MONTH_DAY.ToString()
        );

        public static readonly FieldItem FlyerProgram = new FieldItem(
            "FlyerProgram",
            GoogleDefine.PredefinedItem.FREQUENT_FLYER_PROGRAM_NAME_AND_NUMBER.ToString(),
            null,
            true // IsPredefinedItem
        );

        public static readonly FieldItem ETicketNumber = new FieldItem(
            "ETicketNumber",
            "object.reservationInfo.eticketNumber"
        );

        public static readonly FieldItem ConfirmationCode = new FieldItem(
            "ConfirmationCode",
            "object.reservationInfo.confirmationCode"
        );

        public static readonly FieldItem BookingClass = new FieldItem(
            "BookingClass",
            "object.textModulesData['BookingClass']"
        );

        public static readonly FieldItem SeatClass = new FieldItem(
            "SeatClass",
            "object.boardingAndSeatingInfo.seatClass"
        );

        public static readonly FieldItem BaggagesValues = new FieldItem(
            "BaggagesValues",
            "object.textModulesData['BaggagesValues']"
        );

        public static readonly FieldItem CodeShare = new FieldItem(
            "CodeShare",
            "object.textModulesData['CodeShare']"
        );

        public static readonly FieldItem SecurityProgramLogo = new FieldItem(
            "SecurityProgramLogo",
            "object.securityProgramLogo"
        );

        public static readonly FieldItem PrivilegeImage = new FieldItem(
            "PrivilegeImage",
            "object.boardingAndSeatingInfo.boardingPrivilegeImage"
        );
        public static readonly FieldItem SpecialMealCode = new FieldItem(
            "SpecialMealCode",
            "object.textModulesData['SpecialMealCode']"
        );
        public static readonly FieldItem AdditionalTextString = new FieldItem(
            "AdditionalTextString",
            "object.textModulesData['AdditionalTextString']"
        );
        #endregion

        public static TemplateItem ToCardRowItem(this FieldItem item, FieldItem? secondItem = null)
        {
            if (item.IsPredefinedItem)
            {
                return new TemplateItem { PredefinedItem = item.FieldPath };
            }

            return new TemplateItem
            {
                FirstValue = new FieldSelector
                {
                    Fields = new List<FieldReference> { item.ToFieldReference() },
                },
                SecondValue =
                    secondItem == null
                        ? null
                        : new FieldSelector
                        {
                            Fields = new List<FieldReference> { secondItem.ToFieldReference() },
                        },
            };
        }

        public static BarcodeSectionDetail ToBarcodeSectionDetail(this FieldItem item)
        {
            return new BarcodeSectionDetail
            {
                FieldSelector = new FieldSelector
                {
                    Fields = new List<FieldReference> { item.ToFieldReference() },
                },
            };
        }

        public static DetailsItemInfo ToDetailItem(
            this FieldItem item,
            FieldItem? secondItem = null
        )
        {
            return new DetailsItemInfo { Item = item.ToCardRowItem(secondItem) };
        }

        public static FieldReference ToFieldReference(this FieldItem item)
        {
            return new FieldReference { FieldPath = item.FieldPath, DateFormat = item.DateFormat };
        }

        //範本
        public static ClassTemplateInfo GetCardTemplate(string companyCode)
        {
            switch (companyCode)
            {
                case AirLineIATADefine.CI:
                    return CI_Template();
                case AirLineIATADefine.AE:
                    return AE_Template();
                default:
                    return DefaultTemplate();
            }
        }

        private static ClassTemplateInfo DefaultTemplate()
        {
            // 若沒有公司代碼對應時，可以回傳 null 或是 fallback 預設模板
            return null;
        }

        private static ClassTemplateInfo CI_Template()
        {
            var classTemplateInfo = new ClassTemplateInfo();

            classTemplateInfo.CardTemplateOverride = new CardTemplateOverride
            {
                CardRowTemplateInfos = new List<CardRowTemplateInfo>
                {
                    // 第一列：（1. 出發時間/航廈， 2. 抵達時間/抵達航廈）
                    new CardRowTemplateInfo
                    {
                        TwoItems = new CardRowTwoItems
                        {
                            StartItem = DepartureDateTime.ToCardRowItem(OriginTerminal),
                            EndItem = ArrivalDateTime.ToCardRowItem(DestinationTerminal),
                        },
                    },
                    //// 第二列（1. 登機時間， 2. 登機門， 3.登機分組/座位 ）
                    new CardRowTemplateInfo
                    {
                        ThreeItems = new CardRowThreeItems
                        {
                            StartItem = BoardingDateTime.ToCardRowItem(),
                            MiddleItem = OriginGate.ToCardRowItem(),
                            EndItem = BoardingGroup.ToCardRowItem(SeatNumber),
                        },
                    },
                    // 第三列：（1. 旅客姓名， 2. 序號 ）
                    new CardRowTemplateInfo
                    {
                        TwoItems = new CardRowTwoItems
                        {
                            StartItem = PassengerName.ToCardRowItem(),
                            EndItem = SequenceNumber.ToCardRowItem(),
                        },
                    },
                },
            };
            classTemplateInfo.DetailsTemplateOverride = new DetailsTemplateOverride
            {
                DetailsItemInfos = new List<DetailsItemInfo>
                {
                    AirportCheckinInfo.ToDetailItem(),
                    BaggageInfo.ToDetailItem(),
                    FlyerProgram.ToDetailItem(),
                    ETicketNumber.ToDetailItem(),
                    ConfirmationCode.ToDetailItem(),
                    FlightDate.ToDetailItem(),
                    SeatClass.ToDetailItem(),
                    BookingClass.ToDetailItem(),
                    CodeShare.ToDetailItem(),
                    BaggagesValues.ToDetailItem(),
                    SpecialMealCode.ToDetailItem(),
                    ReminderMessage.ToDetailItem(),
                },
            };

            classTemplateInfo.CardBarcodeSectionDetails = new CardBarcodeSectionDetails
            {
                FirstTopDetail = SecurityProgramLogo.ToBarcodeSectionDetail(),
                SecondTopDetail = PrivilegeImage.ToBarcodeSectionDetail(),
                FirstBottomDetail = AdditionalTextString.ToBarcodeSectionDetail(),
            };

            return classTemplateInfo;
        }

        private static ClassTemplateInfo AE_Template()
        {
            var classTemplateInfo = new ClassTemplateInfo();

            classTemplateInfo.CardTemplateOverride = new CardTemplateOverride
            {
                CardRowTemplateInfos = new List<CardRowTemplateInfo>
                {
                    // 第一列：（1. 出發時間/航廈， 2. 抵達時間/抵達航廈）
                    new CardRowTemplateInfo
                    {
                        ThreeItems = new CardRowThreeItems
                        {
                            StartItem = FlightNumber.ToCardRowItem(),
                            MiddleItem = BoardingDateTime.ToCardRowItem(),
                            EndItem = OriginGate.ToCardRowItem(),
                        },
                    },
                    // 第二列：（1. 旅客姓名）
                    new CardRowTemplateInfo
                    {
                        OneItem = new CardRowOneItem { Item = PassengerName.ToCardRowItem() },
                    },
                    //// 第三列（1. 序號， 2. 座位， 3.艙等 ）
                    new CardRowTemplateInfo
                    {
                        ThreeItems = new CardRowThreeItems
                        {
                            StartItem = SequenceNumber.ToCardRowItem(),
                            MiddleItem = SeatNumber.ToCardRowItem(),
                            EndItem = SeatClass.ToCardRowItem(),
                        },
                    },
                },
            };
            classTemplateInfo.DetailsTemplateOverride = new DetailsTemplateOverride
            {
                DetailsItemInfos = new List<DetailsItemInfo>
                {
                    AirportCheckinInfo.ToDetailItem(),
                    BaggageInfo.ToDetailItem(),
                    FlyerProgram.ToDetailItem(),
                    ETicketNumber.ToDetailItem(),
                    ConfirmationCode.ToDetailItem(),
                    FlightDate.ToDetailItem(),
                    SeatClass.ToDetailItem(),
                    BookingClass.ToDetailItem(),
                    CodeShare.ToDetailItem(),
                    BaggagesValues.ToDetailItem(),
                    SpecialMealCode.ToDetailItem(),
                    ReminderMessage.ToDetailItem(),
                },
            };

            classTemplateInfo.CardBarcodeSectionDetails = new CardBarcodeSectionDetails
            {
                FirstTopDetail = SecurityProgramLogo.ToBarcodeSectionDetail(),
                SecondTopDetail = PrivilegeImage.ToBarcodeSectionDetail(),
                FirstBottomDetail = AdditionalTextString.ToBarcodeSectionDetail(),
            };

            return classTemplateInfo;
        }
    }
}
