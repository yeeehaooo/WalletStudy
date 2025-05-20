using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Models
{
    public class AirportInfoModel
    {
        /// <summary>
        /// 機場的 IATA
        /// </summary>
        public string IATA { get; set; }

        /// <summary>
        /// 機場的 ICAO
        /// </summary>
        public string ICAO { get; set; }

        /// <summary>
        /// 城市名稱
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 航廈編號
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 登機門編號
        /// </summary>
        public string Gate { get; set; }

        /// <summary>
        /// 機場名稱 複寫
        /// </summary>
        public LocalizedLanguageItem? NameOverride { get; set; }
    }
}
