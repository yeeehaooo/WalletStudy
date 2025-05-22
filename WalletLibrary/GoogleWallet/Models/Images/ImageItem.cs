using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.Models.Images
{
    public class ImageItem
    {
        public ImageItem(
            string uri,
            string uriDescription,
            LocalizedStringItem? uriLocalizedDescription = null,
            LocalizedStringItem? contentDescription = null
        )
        {
            ContentDescription = contentDescription;
            SourceUri = new ImageUriItem(uri, uriDescription);
            SourceUri.LocalizedDescription = uriLocalizedDescription;
        }

        public LocalizedStringItem? ContentDescription { get; set; }

        public ImageUriItem SourceUri { get; set; }
    }
}
