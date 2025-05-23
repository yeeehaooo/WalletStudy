using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.Hanlders.Interfaces;

namespace WalletLibrary.Hanlders
{
    /// <summary>
    /// WalletHandler<br/>
    /// </summary>
    public class WalletHandler : IWalletHandler
    {
        public string CompanyCode { get; private set; }
        public IGoogleWalletHandler GoogleWallet { get; private set; }

        //Apple Wallet Handler
        //public IAppleWalletHandler Applewallet { get; private set; }
        public WalletHandler(string companyCode, IGoogleWalletHandler googleWallet)
        {
            CompanyCode = companyCode;

            GoogleWallet = googleWallet;
        }
    }
}
