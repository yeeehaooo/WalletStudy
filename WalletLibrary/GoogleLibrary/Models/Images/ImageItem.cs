using WalletLibrary.GoogleLibrary.Models.Languages;

namespace WalletLibrary.GoogleLibrary.Models.Images
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
