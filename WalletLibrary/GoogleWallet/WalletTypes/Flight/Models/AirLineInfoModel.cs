using WalletLibrary.GoogleWallet.Models;
using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Models
{
    public class AirLineInfoModel
    {
        /// <summary>
        /// 欄位: 航空公司 IATA <br/>
        /// 備註: 例如 CI<br/>
        /// 長度: 2<br/>
        /// </summary>
        public string CarrierCode { get; set; }

        /// <summary>
        /// 欄位: 航空公司 航班編號<br/>
        /// 備註: 例如 123<br/>
        /// 長度: 4<br/>
        /// </summary>
        public string FlightNumber { get; set; }

        /// <summary>
        /// 航空公司 Name
        /// </summary>
        public LocalizedLanguageItem AirLineName { get; set; }

        /// <summary>
        /// 航空公司 Logo Uri & Description
        /// </summary>
        public ImageModel? AirlineLogo { get; set; }

        ///// <summary>
        ///// 航空公司 Wide Logo Uri & Description
        ///// </summary>
        //public ImageModel WideAirlineLogo { get; set; }

        ///// <summary>
        ///// 航空聯盟 AllianceLogo Uri & Description
        ///// </summary>
        //public ImageModel AirlineAllianceLogo { get; set; }
    }
}
