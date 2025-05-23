using Google.Apis.Auth.OAuth2;
using Google.Apis.Walletobjects.v1;
using WalletLibrary.GoogleLibrary.Context.Interfaces;
using WalletLibrary.GoogleLibrary.Settings;

namespace WalletLibrary.GoogleLibrary.Context
{
    /// <summary>
    /// Google Wallet 設定上下文，包含發卡者資訊與金鑰檔案等設定。
    /// </summary>
    public class GoogleWalletContext : IGoogleWalletContext
    {
        public GoogleWalletSettings WalletSettings { get; private set; }

        public WalletobjectsService WalletobjectsService { get; private set; }

        public ServiceAccountCredential Credential { get; private set; }

        public GoogleWalletContext(
            GoogleWalletSettings googlewalletSettings,
            WalletobjectsService walletobjectsService,
            ServiceAccountCredential credential
        )
        {
            WalletSettings = googlewalletSettings;
            WalletobjectsService = walletobjectsService;
            Credential = credential;
        }
    }
}
