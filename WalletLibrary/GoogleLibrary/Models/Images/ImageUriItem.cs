using WalletLibrary.GoogleLibrary.Models.Languages;

namespace WalletLibrary.GoogleLibrary.Models.Images
{
    /// <param name="Uri"> Image Uri </param>
    /// <param name="Description"> Image Description </param>
    /// <param name="LocalizedDescription"> Image Description (多語系)</param>
    public record ImageUriItem
    {
        public ImageUriItem(
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
