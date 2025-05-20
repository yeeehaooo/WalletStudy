namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Models
{
    /// <summary>
    /// Boarding Pass Wallet 登機證錢包
    /// </summary>
    public class BoardingPassWalletModel
    {
        /// <summary>
        /// 欄位: 01,渠道<br/>
        /// 備註: 無<br/>
        /// Amadeus: Channel<br/>
        /// 長度: 10<br/>
        /// </summary>
        public string Channel { get; set; }

        public FlightInfo Flight { get; set; }
        public PassengerInfo Passenger { get; set; }
    }
}
