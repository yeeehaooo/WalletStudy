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

        /// <summary>
        /// Language
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum LanguageTag
        {
            /// <summary>
            /// 英文（美國）
            /// </summary>
            [EnumMember(Value = "en-US")]
            en_US,

            /// <summary>
            /// 英文（英國）
            /// </summary>
            [EnumMember(Value = "en-GB")]
            en_GB,

            /// <summary>
            /// 英文（加拿大）
            /// </summary>
            [EnumMember(Value = "en-CA")]
            en_CA,

            /// <summary>
            /// 英文（澳洲）
            /// </summary>
            [EnumMember(Value = "en-AU")]
            en_AU,

            /// <summary>
            /// 中文（台灣，正體）
            /// </summary>
            [EnumMember(Value = "zh-TW")]
            zh_TW,

            /// <summary>
            /// 中文（中國，簡體）
            /// </summary>
            [EnumMember(Value = "zh-CN")]
            zh_CN,

            /// <summary>
            /// 中文（香港，正體）
            /// </summary>
            [EnumMember(Value = "zh-HK")]
            zh_HK,

            /// <summary>
            /// 西班牙文（西班牙）
            /// </summary>
            [EnumMember(Value = "es-ES")]
            es_ES,

            /// <summary>
            /// 西班牙文（墨西哥）
            /// </summary>
            [EnumMember(Value = "es-MX")]
            es_MX,

            /// <summary>
            /// 西班牙文（美國）
            /// </summary>
            [EnumMember(Value = "es-US")]
            es_US,

            /// <summary>
            /// 法文（法國）
            /// </summary>
            [EnumMember(Value = "fr-FR")]
            fr_FR,

            /// <summary>
            /// 法文（加拿大）
            /// </summary>
            [EnumMember(Value = "fr-CA")]
            fr_CA,

            /// <summary>
            /// 法文（瑞士）
            /// </summary>
            [EnumMember(Value = "fr-CH")]
            fr_CH,

            /// <summary>
            /// 德文（德國）
            /// </summary>
            [EnumMember(Value = "de-DE")]
            de_DE,

            /// <summary>
            /// 德文（奧地利）
            /// </summary>
            [EnumMember(Value = "de-AT")]
            de_AT,

            /// <summary>
            /// 德文（瑞士）
            /// </summary>
            [EnumMember(Value = "de-CH")]
            de_CH,

            /// <summary>
            /// 日文（日本）
            /// </summary>
            [EnumMember(Value = "ja-JP")]
            ja_JP,

            /// <summary>
            /// 韓文（韓國）
            /// </summary>
            [EnumMember(Value = "ko-KR")]
            ko_KR,

            /// <summary>
            /// 葡萄牙文（葡萄牙）
            /// </summary>
            [EnumMember(Value = "pt-PT")]
            pt_PT,

            /// <summary>
            /// 葡萄牙文（巴西）
            /// </summary>
            [EnumMember(Value = "pt-BR")]
            pt_BR,

            /// <summary>
            /// 俄文（俄羅斯）
            /// </summary>
            [EnumMember(Value = "ru-RU")]
            ru_RU,

            /// <summary>
            /// 阿拉伯文（沙烏地阿拉伯）
            /// </summary>
            [EnumMember(Value = "ar-SA")]
            ar_SA,

            /// <summary>
            /// 義大利文（義大利）
            /// </summary>
            [EnumMember(Value = "it-IT")]
            it_IT,

            /// <summary>
            /// 義大利文（瑞士）
            /// </summary>
            [EnumMember(Value = "it-CH")]
            it_CH,

            /// <summary>
            /// 荷蘭文（荷蘭）
            /// </summary>
            [EnumMember(Value = "nl-NL")]
            nl_NL,

            /// <summary>
            /// 荷蘭文（比利時）
            /// </summary>
            [EnumMember(Value = "nl-BE")]
            nl_BE,

            /// <summary>
            /// 瑞典文（瑞典）
            /// </summary>
            [EnumMember(Value = "sv-SE")]
            sv_SE,

            /// <summary>
            /// 挪威文（挪威）
            /// </summary>
            [EnumMember(Value = "no-NO")]
            no_NO,

            /// <summary>
            /// 波蘭文（波蘭）
            /// </summary>
            [EnumMember(Value = "pl-PL")]
            pl_PL,

            /// <summary>
            /// 丹麥文（丹麥）
            /// </summary>
            [EnumMember(Value = "da-DK")]
            da_DK,

            /// <summary>
            /// 芬蘭文（芬蘭）
            /// </summary>
            [EnumMember(Value = "fi-FI")]
            fi_FI,

            /// <summary>
            /// 土耳其文（土耳其）
            /// </summary>
            [EnumMember(Value = "tr-TR")]
            tr_TR,
        }
    }
}
