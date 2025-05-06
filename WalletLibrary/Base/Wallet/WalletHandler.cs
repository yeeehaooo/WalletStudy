using Microsoft.Extensions.DependencyInjection;
using WalletLibrary.Base.Wallet.Interfaces;
using WalletLibrary.GoogleWallet.Base.Interfaces;

namespace WalletLibrary.Base.Wallet
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
