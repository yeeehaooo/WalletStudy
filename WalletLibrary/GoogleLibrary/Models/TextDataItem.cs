using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.Models
{
    public class TextDataItem : LocalizedItem
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <param name="localizedHeader"></param>
        /// <param name="localizedBody"></param>
        public TextDataItem(
            string? header,
            string? body,
            LocalizedStringItem? localizedHeader = null,
            LocalizedStringItem? localizedBody = null
        )
            : base(header, body, localizedHeader, localizedBody) { }
    }
}
