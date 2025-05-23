namespace WalletLibrary.GoogleLibrary.Models.Languages
{
    /// <summary>
    /// 設定語系 & 內容
    /// </summary>
    /// <param name="Language"> Ex: en-US, zh-TW, ja-JP </param>
    /// <param name="Value"></param>
    public class TranslatedStringItem
    {
        public TranslatedStringItem(string language, string value)
        {
            Language = language;
            Value = value;
        }

        public string Language { get; init; }
        public string Value { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is TranslatedStringItem other)
                return this.Language == other.Language;

            return false;
        }

        public override int GetHashCode()
        {
            return this.Language.GetHashCode();
        }
    };
}
