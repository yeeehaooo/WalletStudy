using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.Models
{
    /// <param name="Uri"> Image Uri </param>
    /// <param name="Description"> Image Description </param>
    public record ImageModel
    {
        public string Uri { get; init; }
        public string Description { get; init; }
    }
}
