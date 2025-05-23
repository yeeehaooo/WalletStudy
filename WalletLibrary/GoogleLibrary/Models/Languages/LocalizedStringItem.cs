using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleLibrary.Models.Languages;

namespace WalletLibrary.GoogleLibrary.Models.Languages
{
    /// <summary>
    /// 設定語系 & 內容(Default Value && Translated List)<br/>
    /// 最少需要設定 Default Value
    /// </summary>
    public class LocalizedStringItem
    {
        public LocalizedStringItem() { }

        public LocalizedStringItem(
            TranslatedStringItem? defaultValue,
            List<TranslatedStringItem>? translatedValues = null
        )
        {
            DefaultValue = defaultValue;
            TranslatedValues = translatedValues?.ToHashSet();
        }

        /// <summary>
        /// 預設語系<br/>
        /// Key = 語系 Ex: en-US, zh-TW, ja-JP<br/>
        /// Value = 內容<br/>
        /// </summary>
        public TranslatedStringItem? DefaultValue { get; set; }

        //public string Language { get; set; }
        //public string DefaultValue { get; set; }

        /// <summary>
        /// 其他語系<br/>
        /// Key = 語系 Ex: en-US, zh-TW, ja-JP<br/>
        /// Value = 內容<br/>
        /// </summary>
        public HashSet<TranslatedStringItem>? TranslatedValues { get; set; } =
            new HashSet<TranslatedStringItem>();
    }
}
