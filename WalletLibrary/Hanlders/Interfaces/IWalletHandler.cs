using WalletLibrary.GoogleWallet.Base.Interfaces;

namespace WalletLibrary.Hanlders.Interfaces
{
    public interface IWalletHandler
    {
        string CompanyCode { get; }
        IGoogleWalletHandler GoogleWallet { get; }
    }
}
