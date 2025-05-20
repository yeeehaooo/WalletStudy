using WalletLibrary.GoogleWallet.Models;
using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Models
{
    public class FlightInfo
    {
        /// <summary>
        /// FlightClass ID 的後綴<br/>
        /// 每個航班的唯一ID<br/>
        /// </summary>
        public string ClassSuffix { get; set; }

        /// <summary>
        /// 欄位: 03,營運航班編號<br/>
        /// 備註: 例如 CI1234<br/>
        /// Amadeus: OperatingCarrierCode, OperatingFlightNb<br/>
        /// 長度: 2+4<br/>
        /// </summary>
        public AirLineInfoModel Operating { get; set; } = new();

        /// <summary>
        /// IATA<br/>
        /// 欄位: 05,出發機場代碼<br/>
        /// 備註: 無,例如 TPE<br/>
        /// Amadeus: DepartureAirportCode<br/>
        /// 長度: 3<br/>
        /// <br/>
        /// CityName<br/>
        /// 欄位: 06,出發城市名稱<br/>
        /// 備註: 後台維護對應名稱,例如 台北(松山)<br/>
        /// Amadeus: DepartureAirportCode<br/>
        /// 長度: 20<br/>
        /// <br/>
        /// Terminal<br/>
        /// 欄位: 15,出發航廈<br/>
        /// 備註: 例如 1<br/>
        /// Amadeus: DepartureTerminal<br/>
        /// 長度: 3<br/>
        /// <br/>
        /// Gate<br/>
        /// 欄位: 17,出發登機門<br/>
        /// 備註: 例如 A01<br/>
        /// Amadeus: BoardingGate<br/>
        /// 長度: 5<br/>
        /// <br/>
        /// NameOverride<br/>
        /// 欄位: 36,出發機場名稱<br/>
        /// 備註: 對應後台機場名稱, 例如: 桃園國際機場<br/>
        /// Amadeus: DepartureAirportCode<br/>
        /// <br/>
        /// </summary>
        public AirportInfoModel DepartureAirport { get; set; }

        /// <summary>
        /// IATA<br/>
        /// 欄位: 07,抵達機場代碼<br/>
        /// 備註: 三碼機場代碼<br/>
        /// Amadeus: ArrivalAirportCode<br/>
        /// 長度: 3<br/>
        /// <br/>
        /// CityName<br/>
        /// 欄位: 08,抵達城市名稱<br/>
        /// 備註: 後台維護對應名稱,例如 東京(成田)<br/>
        /// Amadeus: ArrivalAirportCode<br/>
        /// 長度: 20<br/>
        /// <br/>
        /// Terminal<br/>
        /// 欄位: 16,抵達航廈<br/>
        /// 備註: 例如 2<br/>
        /// Amadeus: ArrivalTerminal<br/>
        /// 長度: 3<br/>
        /// <br/>
        /// Gate<br/>
        /// 欄位: 18,抵達登機門<br/>
        /// 備註: 例如 A01<br/>
        /// Amadeus: ArrivalGate<br/>
        /// 長度: 5<br/>
        /// <br/>
        /// NameOverride<br/>
        /// 欄位: 37,抵達機場名稱<br/>
        /// 備註: 對應後台機場名稱, 例如: 成田國際機場<br/>
        /// Amadeus: ArrivalAirportCode<br/>
        /// <br/>
        /// </summary>
        public AirportInfoModel ArrivalAirport { get; set; }

        /// <summary>
        /// 欄位: 09,航班日期<br/>
        /// 備註: yyyy/MM/dd 格式,例如 2025/01/01<br/>
        /// Amadeus: FlightDepartureDate<br/>
        /// 長度: 10<br/>
        /// </summary>
        public string DepartureDate { get; set; }

        /// <summary>
        /// 欄位: 10,出發時間<br/>
        /// 備註: HH:mm 格式,例如 08:30<br/>
        /// Amadeus: DepartureTime<br/>
        /// 長度: 8<br/>
        /// </summary>
        public string DepartureTime { get; set; }

        /// <summary>
        /// 欄位: 11,抵達日期<br/>
        /// 備註: yyyy/MM/dd 格式,例如 2025/01/01<br/>
        /// Amadeus: ArrivalDate<br/>
        /// 長度: 8<br/>
        /// </summary>
        public string ArrivalDate { get; set; }

        /// <summary>
        /// 欄位: 11,抵達時間<br/>
        /// 備註: HH:mm 格式,例如 08:30<br/>
        /// Amadeus: ArrivalTime<br/>
        /// 長度: 5<br/>
        /// </summary>
        public string ArrivalTime { get; set; }

        /// <summary>
        /// 欄位: 12,登機日期<br/>
        /// 備註: yyyy/MM/dd 格式,例如 2025/01/01<br/>
        /// Amadeus: -<br/>
        /// 長度: 10<br/>
        /// </summary>
        public string BoardingDate { get; set; }

        /// <summary>
        /// 欄位: 13,登機時間<br/>
        /// 備註: 最後登機時間, HH:mm 格式,例如 08:30<br/>
        /// Google Wallet 時間格式 ISO 8601 yyyy-MM-ddTHH:mm:ss
        /// Amadeus: LatestBoardingTime<br/>
        /// 長度: 5<br/>
        /// </summary>
        public string LatestBoardingTime { get; set; }

        /// <summary>
        /// 欄位: 31,場站資訊<br/>
        /// 備註: 機場與報到櫃台資訊(By語系) + Link(By語系+場站)<br/>
        /// Amadeus: <br/>
        /// 長度: <br/>
        /// </summary>
        public UriModel? AirportCheckinInfo { get; set; }

        /// <summary>
        /// 欄位: 32, 行李資訊<br/>
        /// 備註: 詳細行李資訊(By語系) + Link(By語系+場站)<br/>
        /// Amadeus: <br/>
        /// 長度: <br/>
        /// </summary>
        public UriModel? BaggageLocalizedMessage { get; set; }

        /// <summary>
        /// 欄位: 34, 營運航司<br/>
        /// 備註: OperatingCarrierCode 對應後台航司名對照表 例如: Operated by China Airlines LTD.<br/>
        /// Amadeus: <br/>
        /// 長度: <br/>
        /// </summary>
        public LocalizedItem? OperatingCarrierName { get; set; }

        /// <summary>
        /// 欄位: 39, 提醒文字<br/>
        /// 備註: 後台設定多語系 <br/>
        /// Amadeus: <br/>
        /// 長度: <br/>
        /// </summary>
        public TextModel? ReminderMessage { get; set; }

        ///// <summary>
        ///// 其他設定, For Google Wallet Flight Class LinksModuleData
        ///// </summary>
        //public List<UriModel> Links { get; set; }
    }
}
