using WalletLibrary.GoogleWallet.Base.Interfaces;

namespace WalletLibrary.Base.Wallet.Interfaces
{
    public interface IWalletHandler
    {
        string CompanyCode { get; }
        IGoogleWalletHandler GoogleWallet { get;  }
    }
}
