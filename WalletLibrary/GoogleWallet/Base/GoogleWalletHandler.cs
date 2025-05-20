using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Context.Interfaces;
using WalletLibrary.GoogleWallet.Settings;
using WalletLibrary.GoogleWallet.WalletTypes.Flight.Interfaces;

namespace WalletLibrary.GoogleWallet.Base
{
    public class GoogleWalletHandler : IGoogleWalletHandler
    {
        public GoogleWalletSettings WalletSettings => _WalletContext.WalletSettings;

        /// <summary>
        /// Wallet Context<br/>
        /// 各種設定 & 金鑰憑證<br/>
        /// </summary>
        private readonly IGoogleWalletContext _WalletContext;

        /// <summary>
        /// 航班錢包
        /// </summary>
        public IFlightWallet FlightWallet { get; private set; }

        // Other Wallets
        // 可擴充屬性：
        // public IGiftCardWallet GiftCardWallet { get; private set; }

        public GoogleWalletHandler(IGoogleWalletContext context, IFlightWallet flightWallet)
        {
            _WalletContext = context;
            FlightWallet = flightWallet;
        }
    }
}
