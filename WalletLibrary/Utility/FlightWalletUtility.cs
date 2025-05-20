using System.Globalization;
using Google.Apis.Walletobjects.v1.Data;
using Newtonsoft.Json;
using WalletLibrary.GoogleWallet.Models;
using WalletLibrary.GoogleWallet.Models.Languages;
using GoogleApiUri = Google.Apis.Walletobjects.v1.Data.Uri;
using SystemUri = System.Uri;

namespace WalletLibrary.DTO
{
    public static class FlightWalletUtility
    {
        public static string FormatFlightNumber(string carrier, string number)
        {
            return string.Format("{0,-2}{1,4}", carrier, number); // carrier 左對齊佔 2 格，number 右對齊佔 4 格
        }

        /// <summary>
        /// 將指定格式的日期 (yyyy/MM/dd) 和時間 (HH:mm) 合併並轉換成指定格式的字串。
        /// </summary>
        /// <param name="date">日期字串，格式為 yyyy/MM/dd</param>
        /// <param name="time">時間字串，格式為 HH:mm</param>
        /// <returns>格式化後的時間字串，格式為 formater 格式</returns>
        internal static string FormatToDateTimeString(string date, string time, string formater)
        {
            return System
                .DateTime.ParseExact(
                    $"{date} {time}",
                    new string[] { "yyyy/MM/dd HH:mm" },
                    CultureInfo.InvariantCulture
                )
                .ToString(formater);
        }

        /// <summary>
        /// 將指定格式的日期 (yyyy/MM/dd) 和時間 (HH:mm) 合併並轉換成 ISO 8601 格式 (yyyy-MM-ddTHH:mm:ssZ) 的字串。
        /// </summary>
        /// <param name="date">日期字串，格式為 yyyy/MM/dd</param>
        /// <param name="time">時間字串，格式為 HH:mm</param>
        /// <returns>格式化後的時間字串，格式為 yyyy-MM-ddTHH:mm:ssZ</returns>
        public static string ToIso8601DateTimeString(this System.DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// 將指定格式的日期 (yyyy/MM/dd) 和時間 (HH:mm) 合併並轉換成 ISO 8601 格式 (yyyy-MM-ddTHH:mm:ss) 的字串。
        /// </summary>
        /// <param name="date">日期字串，格式為 yyyy/MM/dd</param>
        /// <param name="time">時間字串，格式為 HH:mm</param>
        /// <returns>格式化後的時間字串，格式為 yyyy-MM-ddTHH:mm:ss</returns>
        public static string FormatToIso8601DateTimeString(string date, string time)
        {
            return FormatToDateTimeString(date, time, "yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// 設定 圖片 Uri & Description
        /// </summary>
        /// <param name="imgaeId"></param>
        /// <param name="imageUri"></param>
        /// <param name="description"></param>
        /// <returns>Image</returns>
        public static Image BuildImageModule(string imgaeId, string imageUri, string description)
        {
            if (
                !string.IsNullOrWhiteSpace(imgaeId)
                && !string.IsNullOrWhiteSpace(imageUri)
                && !string.IsNullOrWhiteSpace(description)
            )
                return new Image
                {
                    SourceUri = new ImageUri { Uri = imageUri, Description = description },
                };
            throw new ArgumentNullException(
                $"BuildImageModule Error By {imageUri}, {imageUri}, {description}"
            );
        }

        /// <summary>
        /// 設定 圖片 Uri & Description
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image ToImageModule(this ImageModel image, string imageId)
        {
            if (image == null)
                return null;
            return BuildImageModule(imageId, image.Uri, image.Description);
        }

        /// <summary>
        /// 設定 圖片 Uri & Description
        /// </summary>
        /// <param name="imageUri"></param>
        /// <param name="description"></param>
        /// <returns>Image</returns>
        public static GoogleApiUri BuildLinksUriModule(
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
        /// <param name="imageUri"></param>
        /// <param name="description"></param>
        /// <returns>Image</returns>
        public static GoogleApiUri? ToUriModule(this UriModel uriModel, string linkId)
        {
            if (uriModel == null)
                return null;
            return BuildLinksUriModule(linkId, uriModel.Uri, uriModel.Description);
        }

        public static TextModuleData ToTextModule(this LocalizedItem localizedItem, string textId)
        {
            if (localizedItem == null)
                return null;
            if (string.IsNullOrEmpty(textId))
                return null;
            //if (string.IsNullOrWhiteSpace(textId))
            //    throw new ArgumentNullException(
            //        $"ToTextModule Error: TextDataModule ID is NullorEmpty"
            //    );
            var textModuleData = new TextModuleData()
            {
                Id = textId,
                LocalizedHeader = localizedItem.Header.ToLocalizedString(),
                LocalizedBody = localizedItem.Body.ToLocalizedString(),
            };
            return textModuleData;
        }

        public static TextModuleData ToDefaultTextModule(
            string textId,
            string textHeader,
            string textBody,
            string language = "en-US"
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
                LocalizedHeader = new LocalizedString
                {
                    DefaultValue = new TranslatedString { Value = textHeader, Language = language },
                    TranslatedValues = new List<TranslatedString>
                    {
                        new TranslatedString { Value = textHeader, Language = "zh-TW" },
                    },
                },
                LocalizedBody = new LocalizedString
                {
                    DefaultValue = new TranslatedString { Value = textBody, Language = language },
                    TranslatedValues = new List<TranslatedString>
                    {
                        new TranslatedString { Value = textBody, Language = "zh-TW" },
                    },
                },
            };
            return textModuleData;
        }

        public static Message ToMessage(this MessageModel localizedItem, string messageId)
        {
            if (localizedItem == null)
                return null;
            if (string.IsNullOrEmpty(messageId))
                return null;
            var message = new Message()
            {
                Id = messageId,
                MessageType = localizedItem.MessageType,
                DisplayInterval = new TimeInterval
                {
                    Start = new Google.Apis.Walletobjects.v1.Data.DateTime
                    {
                        Date = localizedItem.MessageStart.ToIso8601DateTimeString(),
                    },
                    End = new Google.Apis.Walletobjects.v1.Data.DateTime
                    {
                        Date = localizedItem.MessageEnd.ToIso8601DateTimeString(),
                    },
                },
                LocalizedHeader = localizedItem.Header.ToLocalizedString(),
                LocalizedBody = localizedItem.Body.ToLocalizedString(),
            };
            return message;
        }

        public static LocalizedString ToLocalizedString(this LocalizedLanguageItem languageItem)
        {
            if (languageItem == null)
                return null;
            if (
                string.IsNullOrWhiteSpace(languageItem.Default.Language)
                || string.IsNullOrWhiteSpace(languageItem.Default.Value)
            )
                throw new ArgumentNullException(
                    $"ToLocalizedString Error: {nameof(languageItem)}: {JsonConvert.SerializeObject(languageItem)}"
                );
            return new LocalizedString
            {
                DefaultValue = languageItem.Default.ToDefaultValue(),
                TranslatedValues = languageItem.TranslatedValues.ToTranslatedValues(),
            };
        }

        /// <summary>
        /// 設定其他語系
        /// 將 List<LocalizedLanguageStringItem> 轉換為 List<TranslatedString>，<br/>
        /// </summary>
        /// <param name="localizedLanguageList"></param>
        /// <returns> 多語系設定 返回TranslatedString, 無設定 返回null</returns>
        public static List<TranslatedString>? ToTranslatedValues(
            this HashSet<LocalizedLanguageStringItem> localizedLanguageList
        )
        {
            if (localizedLanguageList == null || localizedLanguageList.Count <= 0)
                return null;
            return localizedLanguageList
                .Select(item => ToDefaultValue(item))
                .Where(ts => ts != null)
                .ToList();
        }

        /// <summary>
        /// 將 LocalizedLanguageStringItem 轉換為 TranslatedString，<br/>
        /// 若 Language 或 Value 為空白則回傳 null。
        /// </summary>
        /// <param name="localizedLanguage"></param>
        /// <returns></returns>
        public static TranslatedString ToDefaultValue(this LocalizedLanguageStringItem languageItem)
        {
            if (
                string.IsNullOrWhiteSpace(languageItem.Language)
                || string.IsNullOrWhiteSpace(languageItem.Value)
            )
                throw new ArgumentNullException(
                    $"ToDefaultValue Error: {JsonConvert.SerializeObject(languageItem)}"
                );
            return new TranslatedString
            {
                Language = languageItem.Language,
                Value = languageItem.Value,
            };
        }
    }
}
