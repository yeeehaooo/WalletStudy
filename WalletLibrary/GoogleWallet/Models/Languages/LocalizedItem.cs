using System.ComponentModel;

namespace WalletLibrary.GoogleWallet.Models.Languages
{
    /// <summary>
    /// 設定標題 & 內容 (含多語系)物件
    /// </summary>
    public class LocalizedItem
    {
        public LocalizedItem(
            string? header,
            string? body,
            LocalizedStringItem? localizedHeader,
            LocalizedStringItem? localizedBody
        )
        {
            Header = header;
            Body = body;
            LocalizedHeader = localizedHeader;
            LocalizedBody = localizedBody;
        }

        public string Header { get; set; }
        public string Body { get; set; }

        /// <summary>
        /// 標題 (預設語系+多語系)
        /// </summary>
        public LocalizedStringItem? LocalizedHeader { get; set; } = new();

        /// <summary>
        /// 內文 (預設語系+多語系)
        /// </summary>
        public LocalizedStringItem? LocalizedBody { get; set; } = new();
    }
}
