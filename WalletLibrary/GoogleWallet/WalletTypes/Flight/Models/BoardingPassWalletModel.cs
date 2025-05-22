namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Models
{
    /// <summary>
    /// Boarding Pass Wallet 登機證錢包
    /// </summary>
    public class BoardingPassWalletModel
    {
        public FlightInfo Flight { get; set; }
        public PassengerInfo Passenger { get; set; }
    }
}
