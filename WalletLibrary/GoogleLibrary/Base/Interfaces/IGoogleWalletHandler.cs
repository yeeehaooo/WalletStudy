using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.Settings;

namespace WalletLibrary.GoogleWallet.Base.Interfaces
{
    public interface IGoogleWalletHandler
    {
        GoogleWalletSettings WalletSettings { get; }

        IWalletHandler<FlightClass, FlightObject> FlightWallet { get; }

        // 預留：未來如需擴充
        //IGiftCardWallet GiftCardWallet { get; }
    }
}
