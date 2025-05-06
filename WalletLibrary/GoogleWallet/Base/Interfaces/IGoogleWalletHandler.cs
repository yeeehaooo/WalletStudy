using WalletLibrary.GoogleWallet.Context;
using WalletLibrary.GoogleWallet.Flight.Interfaces;
using WalletLibrary.GoogleWallet.Settings;

namespace WalletLibrary.GoogleWallet.Base.Interfaces
{
    public interface IGoogleWalletHandler
    {
        GoogleWalletSettings WalletSettings { get; }

        IFlightWallet FlightWallet { get; }

        // 預留：未來如需擴充
        //IGiftCardWallet GiftCardWallet { get; }
    }
}
