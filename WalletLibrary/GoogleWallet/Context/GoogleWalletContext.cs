using Google.Apis.Auth.OAuth2;
using Google.Apis.Walletobjects.v1;
using WalletLibrary.GoogleWallet.Context.Interfaces;
using WalletLibrary.GoogleWallet.Settings;

namespace WalletLibrary.GoogleWallet.Context
{
    public class GoogleWalletContext : IGoogleWalletContext
    {
        public GoogleWalletSettings WalletSettings { get; private set; }

        public WalletobjectsService WalletobjectsService { get; private set; }

        public ServiceAccountCredential Credential { get; private set; }

        public GoogleWalletContext(GoogleWalletSettings googlewalletSettings, WalletobjectsService walletobjectsService, ServiceAccountCredential credential)
        {
            WalletSettings = googlewalletSettings;
            WalletobjectsService = walletobjectsService;
            Credential = credential;
        }
    }
}
