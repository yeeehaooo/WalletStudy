using WalletLibrary.GoogleWallet.Models;
using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Models
{
    public class PassengerInfo
    {
        /// <summary>
        /// 欄位: 01,渠道<br/>
        /// 備註: 無<br/>
        /// Amadeus: Channel<br/>
        /// 長度: 10<br/>
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// FlightClass ID 的後綴<br/>
        /// 每個航班的唯一ID<br/>
        /// </summary>
        public string ClassSuffix { get; set; }

        /// <summary>
        /// FlightObject ID 的後綴<br/>
        /// 每個旅客的唯一ID<br/>
        /// </summary>
        public string ObjectSuffix { get; set; }

        /// <summary>
        /// 欄位: 04,旅客是從哪個航空公司購買的航班<br/>
        /// 實際執行航班的是另一家航空公司OperatingCarrier, OperatingFlightNb<br/>
        /// 備註: 例如 JL<br/>
        /// Amadeus: MarketingCarrier, MarketingFlightNb<br/>
        /// 長度: 2<br/>
        /// </summary>
        public AirLineInfoModel? Marketing { get; set; }

        /// <summary>
        /// 欄位: 02,旅客姓名<br/>
        /// 備註: 姓名之間:[ ],例如 WANG HSIAOMING<br/>
        /// Amadeus: Surname FirstName<br/>
        /// 長度: 20<br/>
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// 欄位: 14,登機順序<br/>
        /// 備註: 無,例如 ZONE2<br/>
        /// Amadeus: BoardingPriority<br/>
        /// 長度: 6<br/>
        /// </summary>
        public string BoardingZone { get; set; }

        /// <summary>
        /// 欄位: 19,訂位艙等<br/>
        /// 備註: 例如 C<br/>
        /// Amadeus: BookingClass<br/>
        /// 長度: 1<br/>
        /// </summary>
        public string BookingClass { get; set; }

        /// <summary>
        /// 欄位: 20,座位分組<br/>
        /// 備註: 例如 5G<br/>
        /// Amadeus: SeatNumber<br/>
        /// 長度: 4<br/>
        /// </summary>
        public string SeatNumber { get; set; }

        /// <summary>
        /// 欄位: 21,報到序號<br/>
        /// 備註: 固定前綴[SEQ:],補齊數字[..],例如: SEQ:002<br/>
        /// Amadeus: SecurityNumber<br/>
        /// 長度: 3(7)<br/>
        /// </summary>
        public string SecurityNumber { get; set; }

        /// <summary>
        /// 欄位: 22,是否TSA PreCheck<br/>
        /// 備註: 是否顯示TSA Pre ICon 僅美國出發,例如: TSA PRE<br/>
        /// Amadeus: TSAPreCheckIndicator: where 3<br/>
        /// 長度: 7<br/>
        /// </summary>
        public bool IsTSAPreCheck { get; set; }

        /// <summary>
        /// 欄位: 22,是否TSA PreCheck<br/>
        /// 備註: 顯示文字 僅美國出發,例如: TSA PRE<br/>
        /// Amadeus: TSAPreCheckIndicator<br/>
        /// 長度: 7<br/>
        /// </summary>
        public string TSAPreCheckIndicator { get; set; }

        /// <summary>
        /// 欄位: 23,華航會員級別<br/>
        /// 備註: 例如: PARAGON<br/>
        /// Amadeus: FQTVTierDescription<br/>
        /// 長度: 10<br/>
        /// </summary>
        public string FQTVTierDescription { get; set; }

        /// <summary>
        /// 欄位: 24,華航會員卡號<br/>
        /// 備註: 例如: CT0000000<br/>
        /// Amadeus: FQTVNumber<br/>
        /// 長度: 14<br/>
        /// </summary>
        public string FQTVNumber { get; set; }

        /// <summary>
        /// 欄位: 25,SKY PRIORITY<br/>
        /// 備註: 是否顯示  SKY PRIORITY ICON<br/>
        /// Amadeus: FQTVAllianceTier FQTVAllianceTierCode<br/>
        /// 長度: <br/>
        /// </summary>
        public bool IsSkyPriority { get; set; }

        /// <summary>
        /// 欄位: 26,貴賓室資訊<br/>
        /// 備註: 例如: LOUNGE-VLSF<br/>
        /// Amadeus: AdditionalTextString<br/>
        /// 長度: <br/>
        /// </summary>
        public string AdditionalTextString { get; set; }

        /// <summary>
        /// 欄位: 27,訂位代號<br/>
        /// 備註: 例如: 6LTO8V<br/>
        /// Amadeus: SBRRecordLocator<br/>
        /// 長度: 6<br/>
        /// </summary>
        public string ConfirmationCode { get; set; }

        /// <summary>
        /// 欄位: 28,電子機票號碼<br/>
        /// 備註: 例如: 297240203609001<br/>
        /// Amadeus: EtktNumber<br/>
        /// 長度: 15<br/>
        /// </summary>
        public string ElecTicketNumber { get; set; }

        /// <summary>
        /// QR Code Value
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 欄位: 29,特別餐資訊<br/>
        /// 備註: 用[, ]組合, 例如: VOML, XXML<br/>
        /// Amadeus: SSR type="Table"<br/>
        /// 長度: <br/>
        /// </summary>
        public string SpecialMealCode { get; set; }

        /// <summary>
        /// 欄位: 30,行李牌資訊<br/>
        /// 備註: 用[, ]組合, 例如: 10A55300006CE42B, 10A55300006CE42C<br/>
        /// Amadeus: -<br/>
        /// 長度: <br/>
        /// </summary>
        public List<string> BaggagesValues { get; set; } = new List<string>();

        /// <summary>
        /// 欄位: 35,搭乘艙等名稱<br/>
        /// 備註: 例如 BUSINESS CLASS<br/>
        /// Amadeus: CabinComments<br/>
        /// </summary>
        public string CabinComments { get; set; }
    }
}
