namespace WalletLibrary.GoogleWallet.Models.Languages
{
    /// <summary>
    /// 設定語系 & 內容
    /// </summary>
    /// <param name="Language"> Ex: en-US, zh-TW, ja-JP </param>
    /// <param name="Value"></param>
    public record LocalizedLanguageStringItem
    {
        public string Language { get; init; }
        public string Value { get; init; }

        public bool Equals(LocalizedLanguageStringItem x, LocalizedLanguageStringItem y)
        {
            return string.Equals(x?.Language, y?.Language, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(LocalizedLanguageStringItem obj)
        {
            return obj.Language?.ToLowerInvariant().GetHashCode() ?? 0;
        }
    };
}
