using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.Models
{
    public class UriItem
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="description"></param>
        /// <param name="localizedDescription"></param>
        public UriItem(
            string uri,
            string description,
            LocalizedStringItem? localizedDescription = null
        )
        {
            Uri = uri;
            Description = description;
            LocalizedDescription = localizedDescription;
        }

        public string Uri { get; set; }
        public string Description { get; set; }

        public LocalizedStringItem? LocalizedDescription { get; set; }
    }
}
