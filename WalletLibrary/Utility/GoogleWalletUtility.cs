using System.Globalization;
using Google.Apis.Walletobjects.v1.Data;
using Newtonsoft.Json;
using WalletLibrary.GoogleWallet.Models;
using WalletLibrary.GoogleWallet.Models.Images;
using WalletLibrary.GoogleWallet.Models.Languages;
using GoogleApiUri = Google.Apis.Walletobjects.v1.Data.Uri;
using SystemUri = System.Uri;

namespace WalletLibrary.DTO
{
    public static partial class GoogleWalletUtility
    {
        /// <summary>
        /// 格式化航班編號 ex CI 123,CI2222
        /// </summary>
        /// <param name="carrier">IATA</param>
        /// <param name="number">Flight Number</param>
        /// <returns></returns>
        public static string FormatFlightNumber(string carrier, string number)
        {
            return string.Format("{0,-2}{1,4}", carrier, number);
        }

        #region 時間格式
        /// <summary>
        /// 將指定格式的日期 (yyyy/MM/dd) 和時間 (HH:mm) 合併成 System.DateTime()。
        /// </summary>
        /// <param name="date">日期字串，格式為 yyyy/MM/dd</param>
        /// <param name="time">時間字串，格式為 HH:mm</param>
        /// <returns>格式化後的時間字串，格式為 yyyy-MM-ddTHH:mm:ss</returns>
        private static System.DateTime ToDateTime(
            string date,
            string time,
            string[]? formats = null
        )
        {
            return System.DateTime.ParseExact(
                $"{date} {time}",
                formats ??= new[] { "yyyy/MM/dd HH:mm" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None
            );
        }

        /// <summary>
        /// 將指定格式的日期 (yyyy/MM/dd) 和時間 (HH:mm) 合併並轉換成 ISO 8601 格式 (yyyy-MM-ddTHH:mm:ssZ) 的字串。
        /// </summary>
        /// <param name="date">日期字串，格式為 yyyy/MM/dd</param>
        /// <param name="time">時間字串，格式為 HH:mm</param>
        /// <returns>格式化後的時間字串，格式為 yyyy-MM-ddTHH:mm:ssZ</returns>
        public static string ToIso8601DateTimeString(this System.DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// 將指定格式的日期（yyyy/MM/dd）與時間（HH:mm）合併，並轉換為 ISO 8601 格式（yyyy-MM-ddTHH:mm:ss）的字串。
        /// </summary>
        /// <param name="date">日期字串，格式為 yyyy/MM/dd。</param>
        /// <param name="time">時間字串，格式為 HH:mm。</param>
        /// <returns>合併並格式化後的 ISO 8601 時間字串（yyyy-MM-ddTHH:mm:ss）。</returns>
        public static string FormatToIso8601DateTimeString(string date, string time)
        {
            return ToDateTime(date, time).ToIso8601DateTimeString();
        }
        #endregion


        #region Google Api Image 相關方法
        /// <summary>
        /// 設定 圖片 Uri & Description
        /// </summary>
        /// <param name="imgaeId">Image ID or Template Id</param>
        /// <param name="imageUri">Image Uri</param>
        /// <param name="description">Image Description</param>
        /// <param name="localizedDescription">Image Localized Description</param>
        /// <param name="contentDescription">Image ContentDescription(多語系)</param>
        /// <returns>Google Api Image 物件</returns>
        public static Image CreateImageModule(
            string imgaeId,
            string imageUri,
            string description,
            LocalizedStringItem? localizedDescription = null,
            LocalizedStringItem? contentDescription = null
        )
        {
            if (
                !string.IsNullOrWhiteSpace(imgaeId)
                && !string.IsNullOrWhiteSpace(imageUri)
                && !string.IsNullOrWhiteSpace(description)
            )
                return new Image
                {
                    SourceUri = new ImageUri
                    {
                        Uri = imageUri,
                        Description = description,
                        LocalizedDescription = localizedDescription?.ToLocalizedString(),
                    },
                    ContentDescription = contentDescription?.ToLocalizedString(),
                };
            throw new ArgumentNullException(
                $"BuildImageModule Error By {imageUri}, {imageUri}, {description}"
            );
        }

        public static Image ToImageModule(this ImageUriItem imageUriItem, string Id)
        {
            return CreateImageModule(Id, imageUriItem.Uri, imageUriItem.Description);
        }

        #endregion

        #region Google Link Uri 擴充方法

        /// <summary>
        /// 設定 圖片 Uri & Description
        /// </summary>
        /// <param name="uriId"></param>
        /// <param name="uriValue"></param>
        /// <param name="description"></param>
        /// <returns>Google Api Uri 物件</returns>
        public static GoogleApiUri CreateLinksUriModule(
            string uriId,
            string uriValue,
            string description
        )
        {
            if (
                string.IsNullOrWhiteSpace(uriId)
                || string.IsNullOrWhiteSpace(uriValue)
                || string.IsNullOrWhiteSpace(description)
            )
                throw new ArgumentNullException(
                    $"BuildLinksUriModule Error By {uriId}, {uriValue}, {description}"
                );
            return new GoogleApiUri
            {
                Id = uriId,
                UriValue = uriValue,
                Description = description,
            };
        }

        /// <summary>
        /// 設定 圖片 Uri & Description
        /// </summary>
        /// <param name="uriModel"></param>
        /// <param name="uriId"></param>
        /// <returns>Google Api Uri 物件</returns>
        public static GoogleApiUri? ToUriModule(this UriItem uriModel, string uriId)
        {
            if (uriModel == null || string.IsNullOrEmpty(uriId))
                return null;
            return CreateLinksUriModule(uriId, uriModel.Uri, uriModel.Description);
        }

        #endregion

        /// <summary>
        /// 轉換為 Google Api TextModuleData 物件,支援多語系標題&內容
        /// </summary>
        /// <param name="localizedItem"></param>
        /// <param name="textId">TextModuleData Id</param>
        /// <returns></returns>
        public static TextModuleData ToTextModule(this TextDataItem localizedItem, string textId)
        {
            if (localizedItem == null)
                return null;
            if (string.IsNullOrEmpty(textId))
                return null;
            var textModuleData = new TextModuleData()
            {
                Id = textId,
                Body = localizedItem.Body,
                Header = localizedItem.Header,
                LocalizedHeader = localizedItem.LocalizedHeader?.ToLocalizedString(),
                LocalizedBody = localizedItem.LocalizedBody?.ToLocalizedString(),
            };
            return textModuleData;
        }

        /// <summary>
        /// 轉換為 Google Api TextModuleData 物件,單一標題&內容
        /// </summary>
        /// <param name="textId"></param>
        /// <param name="textHeader"></param>
        /// <param name="textBody"></param>
        /// <returns></returns>
        public static TextModuleData CreateTextModuleData(
            string textId,
            string textHeader,
            string textBody,
            LocalizedStringItem? localizedHeader = null,
            LocalizedStringItem? localizedBody = null
        )
        {
            if (
                string.IsNullOrEmpty(textId)
                || string.IsNullOrEmpty(textHeader)
                || string.IsNullOrEmpty(textBody)
            )
                return null;
            var textModuleData = new TextModuleData()
            {
                Id = textId,
                Header = textHeader,
                Body = textBody,
                LocalizedHeader = localizedHeader?.ToLocalizedString(),
                LocalizedBody = localizedBody?.ToLocalizedString(),
            };
            return textModuleData;
        }

        public static Message ToMessage(this MessageItem messageItem, string messageId)
        {
            if (messageItem == null)
                return null;
            if (string.IsNullOrEmpty(messageId))
                return null;
            var message = new Message()
            {
                Id = messageId,
                MessageType = messageItem.MessageType,
                DisplayInterval = new TimeInterval
                {
                    Start = new Google.Apis.Walletobjects.v1.Data.DateTime
                    {
                        Date = messageItem.MessageStart.ToIso8601DateTimeString(),
                    },
                    End = new Google.Apis.Walletobjects.v1.Data.DateTime
                    {
                        Date = messageItem.MessageEnd.ToIso8601DateTimeString(),
                    },
                },
                Header = messageItem.Header,
                Body = messageItem.Body,
                LocalizedHeader = messageItem.LocalizedHeader.ToLocalizedString(),
                LocalizedBody = messageItem.LocalizedBody.ToLocalizedString(),
            };
            return message;
        }

        /// <summary>
        /// Googl Api 多語系物件
        /// </summary>
        /// <param name="languageItem"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LocalizedString ToLocalizedString(this LocalizedStringItem languageItem)
        {
            if (languageItem == null)
                return null;
            var defaultValue = languageItem.DefaultValue?.ToTranslatedString();
            if (defaultValue == null)
                throw new ArgumentNullException(
                    $"ToLocalizedString Error: {nameof(languageItem)}: {JsonConvert.SerializeObject(languageItem)}"
                );
            return new LocalizedString
            {
                DefaultValue = defaultValue,
                TranslatedValues = languageItem.TranslatedValues?.ToTranslatedValues(),
            };
        }

        /// <summary>
        /// 設定其他語系
        /// 將 List<LocalizedLanguageStringItem> 轉換為 List<TranslatedString>，<br/>
        /// </summary>
        /// <param name="localizedLanguageList"></param>
        /// <returns> 多語系設定 返回TranslatedString, 無設定 返回null</returns>
        public static List<TranslatedString>? ToTranslatedValues(
            this HashSet<TranslatedStringItem> localizedLanguageList
        )
        {
            if (localizedLanguageList == null || localizedLanguageList.Count <= 0)
                return null;
            return localizedLanguageList
                .Select(item => ToTranslatedString(item))
                .Where(ts => ts != null)
                .ToList();
        }

        /// <summary>
        /// 將 LocalizedLanguageStringItem 轉換為 TranslatedString，<br/>
        /// 若 Language 為空白則回傳 null。
        /// </summary>
        /// <param name="translatedStringItem"></param>
        /// <returns></returns>
        public static TranslatedString ToTranslatedString(
            this TranslatedStringItem translatedStringItem
        )
        {
            if (string.IsNullOrWhiteSpace(translatedStringItem.Language))
                throw new ArgumentNullException(
                    $"ToDefaultValue Error: {JsonConvert.SerializeObject(translatedStringItem)}"
                );
            return CreateTranslatedString(
                translatedStringItem.Language,
                translatedStringItem.Value
            );
        }

        /// <summary>
        /// 建立 自訂語系 的 LocalizedString物件
        /// </summary>
        /// <param name="language"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LocalizedString CreateLocalizedString(string language, string value)
        {
            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentNullException($"CreateLocalizedString Error: language is Null");
            return new LocalizedString { DefaultValue = CreateTranslatedString(language, value) };
        }

        ///// <summary>
        ///// 建立 en-Us語系 的 LocalizedString物件
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentNullException"></exception>
        //public static LocalizedString CreateLocalizedString_US(string value)
        //{
        //    return new LocalizedString { DefaultValue = CreateTranslatedString("en-US", value) };
        //}

        ///// <summary>
        ///// 建立 zh-TW語系 的 LocalizedString物件
        ///// </summary>
        ///// <param name="language"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentNullException"></exception>
        //public static LocalizedString CreateLocalizedString_TW(string value)
        //{
        //    return new LocalizedString { DefaultValue = CreateTranslatedString("zh-TW", value) };
        //}

        /// <summary>
        /// 建立 自訂語系 的 TranslatedString物件
        /// </summary>
        /// <param name="language"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static TranslatedString CreateTranslatedString(string language, string value)
        {
            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentNullException($"ToDefaultValue Error: language is Null");
            return new TranslatedString { Language = language, Value = value };
        }

        ///// <summary>
        ///// 建立 en-Us語系 的 TranslatedString物件
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static TranslatedString CreateTranslatedString_US(string value)
        //{
        //    return CreateTranslatedString("en-US", value);
        //}

        ///// <summary>
        ///// 建立 zh-TW語系 的 TranslatedString物件
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static TranslatedString CreateTranslatedString_TW(string value)
        //{
        //    return CreateTranslatedString("zh-TW", value);
        //}
    }
}
