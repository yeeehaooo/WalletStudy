namespace GoogleWalletApi.DTO
{
    public class FlightClassInfo
    {
        public FlightClassInfo() { }

        /// <summary>
        /// Flight Class Id
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 航空公司代碼，例如 "MM"
        /// </summary>
        public string CarrierIataCode { get; set; }

        /// <summary>
        /// 航班編號，例如 "722"
        /// </summary>
        public string FlightNumber { get; set; }

        /// <summary>
        /// 出發地IATA代碼 "TPE"
        /// </summary>
        public string OriginAirportIataCode { get; set; }

        /// <summary>
        /// 出發航廈 "1"
        /// </summary>
        public string OriginTerminal { get; set; }

        /// <summary>
        /// 出發登機門 "6"
        /// </summary>
        public string OriginGate { get; set; }

        /// <summary>
        /// 目的地IATA代碼 "NGO"
        /// </summary>
        public string DestinationAirportIataCode { get; set; }

        /// <summary>
        /// 目的地航廈 "2"
        /// </summary>
        public string DestinationTerminal { get; set; }

        /// <summary>
        /// 目的地登機門 "C3"
        /// </summary>
        public string DestinationGate { get; set; }

        /// <summary>
        /// 本地排定起飛時間 2025-05-01T15:30:00
        /// </summary>
        public DateTime LocalScheduledDepartureDateTime { get; set; }
    }
}
