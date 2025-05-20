using System.ComponentModel;

namespace WalletLibrary.GoogleWallet.Models.Languages
{
    /// <summary>
    /// 設定多語系物件
    /// </summary>
    public class LocalizedItem
    {
        /// <summary>
        /// 標題 (預設語系+多語系)
        /// </summary>
        public LocalizedLanguageItem Header { get; set; } = new();

        /// <summary>
        /// 內文 (預設語系+多語系)
        /// </summary>
        public LocalizedLanguageItem Body { get; set; } = new();
    }
}
