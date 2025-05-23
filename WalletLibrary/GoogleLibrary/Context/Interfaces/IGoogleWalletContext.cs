using Google.Apis.Auth.OAuth2;
using Google.Apis.Walletobjects.v1;
using WalletLibrary.GoogleWallet.Settings;

namespace WalletLibrary.GoogleWallet.Context.Interfaces
{
    public interface IGoogleWalletContext
    {
        public GoogleWalletSettings WalletSettings { get; }
        /// <summary>
        /// 用於驗證 API 請求的服務帳戶憑證。
        /// </summary>
        public WalletobjectsService WalletobjectsService { get; }
        /// <summary>
        /// 用於驗證 API 請求的服務帳戶憑證。
        /// </summary>
        public ServiceAccountCredential Credential { get; }
    }

}
