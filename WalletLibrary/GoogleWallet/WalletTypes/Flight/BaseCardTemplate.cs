using System.Reflection;
using Google.Apis.Walletobjects.v1.Data;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight
{
    public class FieldItem
    {
        public FieldItem(
            string fieldName,
            string fieldPath,
            string? dateFormat = null,
            bool isPredefinedItem = false
        )
        {
            FieldName = fieldName;
            FieldPath = fieldPath;
            DateFormat = dateFormat;
            IsPredefinedItem = isPredefinedItem;
        }

        public string FieldName { get; set; }
        public string FieldPath { get; set; }
        public string? DateFormat { get; set; } = null;
        public bool IsPredefinedItem { get; set; } = false;
    }

    /// <summary>
    /// 設定 模板會用的欄位 與對應 Google Api 欄位 跟 欄位ID
    /// </summary>
    public static class CardTemplateFields
    {
        #region 定義要顯示的資訊欄位
        public static readonly FieldItem DepartureDateTime = new FieldItem(
            "LocalScheduledDepartureDateTime",
            "class.localScheduledDepartureDateTime",
            "TIME_ONLY"
        );

        public static readonly FieldItem OriginTerminal = new FieldItem(
            "OriginTerminal",
            "class.origin.terminal"
        );

        public static readonly FieldItem ArrivalDateTime = new FieldItem(
            "LocalScheduledArrivalDateTime",
            "class.localScheduledArrivalDateTime",
            "TIME_ONLY"
        );

        public static readonly FieldItem DestinationTerminal = new FieldItem(
            "DestinationTerminal",
            "class.destination.terminal"
        );

        public static readonly FieldItem BoardingDateTime = new FieldItem(
            "LocalBoardingDateTime",
            "class.localBoardingDateTime",
            "DATE_TIME"
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
            "DATE_ONLY"
        );

        public static readonly FieldItem FlyerProgram = new FieldItem(
            "FlyerProgram",
            "frequentFlyerProgramNameAndNumber",
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

        public static readonly FieldItem CabinClass = new FieldItem(
            "CabinClass",
            "object.textModulesData['CabinClass']"
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

        public static ClassTemplateInfo CI_TemplateInfo()
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
                            StartItem = CardTemplateFields.DepartureDateTime.ToCardRowItem(
                                CardTemplateFields.OriginTerminal
                            ),
                            EndItem = CardTemplateFields.ArrivalDateTime.ToCardRowItem(
                                CardTemplateFields.DestinationTerminal
                            ),
                        },
                    },
                    //// 第二列（1. 登機時間， 2. 登機門， 3.登機分組/座位 ）
                    new CardRowTemplateInfo
                    {
                        ThreeItems = new CardRowThreeItems
                        {
                            StartItem = CardTemplateFields.BoardingDateTime.ToCardRowItem(),
                            MiddleItem = CardTemplateFields.OriginGate.ToCardRowItem(),
                            EndItem = CardTemplateFields.BoardingGroup.ToCardRowItem(
                                CardTemplateFields.SeatNumber
                            ),
                        },
                    },
                    // 第三列：（1. 旅客姓名， 2. 序號 ）
                    new CardRowTemplateInfo
                    {
                        TwoItems = new CardRowTwoItems
                        {
                            StartItem = CardTemplateFields.PassengerName.ToCardRowItem(),
                            EndItem = CardTemplateFields.SequenceNumber.ToCardRowItem(),
                        },
                    },
                },
            };
            classTemplateInfo.DetailsTemplateOverride = new DetailsTemplateOverride
            {
                DetailsItemInfos = new List<DetailsItemInfo>
                {
                    CardTemplateFields.AirportCheckinInfo.ToDetailItem(),
                    CardTemplateFields.BaggageInfo.ToDetailItem(),
                    CardTemplateFields.FlyerProgram.ToDetailItem(),
                    CardTemplateFields.ETicketNumber.ToDetailItem(),
                    CardTemplateFields.ConfirmationCode.ToDetailItem(),
                    CardTemplateFields.FlightDate.ToDetailItem(),
                    CardTemplateFields.CabinClass.ToDetailItem(SeatClass),
                    CardTemplateFields.CodeShare.ToDetailItem(),
                    CardTemplateFields.BaggagesValues.ToDetailItem(),
                    CardTemplateFields.SpecialMealCode.ToDetailItem(),
                    CardTemplateFields.ReminderMessage.ToDetailItem(),
                },
            };

            classTemplateInfo.CardBarcodeSectionDetails = new CardBarcodeSectionDetails
            {
                FirstTopDetail = CardTemplateFields.SecurityProgramLogo.ToBarcodeSectionDetail(),
                SecondTopDetail = CardTemplateFields.PrivilegeImage.ToBarcodeSectionDetail(),
                FirstBottomDetail =
                    CardTemplateFields.AdditionalTextString.ToBarcodeSectionDetail(),
            };

            return classTemplateInfo;
        }
    }
}
