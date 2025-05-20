namespace WalletLibrary.GoogleWallet.Models.Languages
{
    public class LocalizedLanguageItem
    {
        /// <summary>
        /// 預設語系<br/>
        /// Key = 語系 Ex: en-US, zh-TW, ja-JP<br/>
        /// Value = 內容<br/>
        /// </summary>
        public LocalizedLanguageStringItem Default { get; set; }

        //public string Language { get; set; }
        //public string DefaultValue { get; set; }

        /// <summary>
        /// 其他語系<br/>
        /// Key = 語系 Ex: en-US, zh-TW, ja-JP<br/>
        /// Value = 內容<br/>
        /// </summary>
        public HashSet<LocalizedLanguageStringItem>? TranslatedValues { get; set; } =
            new HashSet<LocalizedLanguageStringItem>();
    }
}
