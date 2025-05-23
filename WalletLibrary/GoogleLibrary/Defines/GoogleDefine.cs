using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WalletLibrary.GoogleLibrary.Defines
{
    /// <summary>
    /// 定義 Google Wallet FlightObject 的狀態。
    /// </summary>
    public static class GoogleDefine
    {
        /// <summary>
        /// 審核狀態，表示物件的審核過程與結果。
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ReviewStatus
        {
            /// <summary>
            /// 物件正在審核中，尚未通過審核，Google 尚未批准。
            /// </summary>
            [EnumMember(Value = "UNDER_REVIEW")]
            UNDER_REVIEW,

            /// <summary>
            /// 物件審核通過，已經正常上線並可以使用。
            /// </summary>
            [EnumMember(Value = "APPROVED")]
            APPROVED,

            /// <summary>
            /// 物件審核不通過，無法使用。
            /// </summary>
            [EnumMember(Value = "REJECTED")]
            REJECTED,
        }

        /// <summary>
        /// 票券狀態，表示該物件在 Google Wallet 中的有效性或處理階段。
        /// </summary>

        [JsonConverter(typeof(StringEnumConverter))]
        public enum State
        {
            /// <summary>
            /// 有效票券。使用者可在 Google Wallet 中查看與使用。
            /// </summary>
            [EnumMember(Value = "ACTIVE")]
            ACTIVE,

            /// <summary>
            /// 票券已完成，例如航班已登機。
            /// </summary>
            [EnumMember(Value = "COMPLETED")]
            COMPLETED,

            /// <summary>
            /// 票券已過期。
            /// </summary>
            [EnumMember(Value = "EXPIRED")]
            EXPIRED,

            /// <summary>
            /// 無效票券，例如被取消或停用。
            /// </summary>
            [EnumMember(Value = "INACTIVE")]
            INACTIVE,
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum BarcodeType
        {
            [EnumMember(Value = "QR_CODE")]
            QR_CODE,

            [EnumMember(Value = "PDF_417")]
            PDF_417,
        }

        /// <summary>
        /// 預先定義的項目，用於標準化顯示資訊。
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PredefinedItem
        {
            /// <summary>
            /// 常旅客計劃名稱與編號。
            /// </summary>
            [EnumMember(Value = "FREQUENT_FLYER_PROGRAM_NAME_AND_NUMBER")]
            FREQUENT_FLYER_PROGRAM_NAME_AND_NUMBER,

            /// <summary>
            /// 航班號碼與實際執飛航班號碼。
            /// </summary>
            [EnumMember(Value = "FLIGHT_NUMBER_AND_OPERATING_FLIGHT_NUMBER")]
            FLIGHT_NUMBER_AND_OPERATING_FLIGHT_NUMBER,
        }

        /// <summary>
        /// 顯示日期/時間欄位專用的 DateFormat 選項。
        /// </summary>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum DateFormat
        {
            /// <summary>
            /// 未指定格式時的預設選項，系統不會套用任何格式設定。
            /// </summary>
            [EnumMember(Value = "DATE_FORMAT_UNSPECIFIED")]
            DATE_FORMAT_UNSPECIFIED,

            /// <summary>
            /// 在 en_US 中將 2018-12-14T13:00:00 算繪為 Dec 14, 1:00 PM。
            /// (舊版別名 dateTime 已淘汰)
            /// </summary>
            [EnumMember(Value = "DATE_TIME")]
            DATE_TIME,

            /// <summary>
            /// 在 en_US 中將 2018-12-14T13:00:00 算繪為 Dec 14。
            /// (舊版別名 dateOnly 已淘汰)
            /// </summary>
            [EnumMember(Value = "DATE_ONLY")]
            DATE_ONLY,

            /// <summary>
            /// 在 en_US 中將 2018-12-14T13:00:00 算繪為 1:00 PM。
            /// (舊版別名 timeOnly 已淘汰)
            /// </summary>
            [EnumMember(Value = "TIME_ONLY")]
            TIME_ONLY,

            /// <summary>
            /// 在 en_US 中將 2018-12-14T13:00:00 算繪為 Dec 14, 2018, 1:00 PM。
            /// (舊版別名 dateTimeYear 已淘汰)
            /// </summary>
            [EnumMember(Value = "DATE_TIME_YEAR")]
            DATE_TIME_YEAR,

            /// <summary>
            /// 在 en_US 中將 2018-12-14T13:00:00 算繪為 Dec 14, 2018。
            /// (舊版別名 dateYear 已淘汰)
            /// </summary>
            [EnumMember(Value = "DATE_YEAR")]
            DATE_YEAR,

            /// <summary>
            /// 將 2018-12-14T13:00:00 算繪為 2018-12。
            /// </summary>
            [EnumMember(Value = "YEAR_MONTH")]
            YEAR_MONTH,

            /// <summary>
            /// 將 2018-12-14T13:00:00 算繪為 2018-12-14。
            /// </summary>
            [EnumMember(Value = "YEAR_MONTH_DAY")]
            YEAR_MONTH_DAY,
        }
    }
}
