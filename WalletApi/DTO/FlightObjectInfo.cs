namespace GoogleWalletApi.DTO
{
    /// <summary>
    /// FlightObject 資訊，用來建立 Google Wallet Boarding Pass。
    /// 表示單一旅客的航班票券資訊，用於產生 Google Wallet 的 FlightObject。
    /// </summary>
    public class FlightObjectInfo
    {
        /// <summary>
        /// FlightObject 的唯一識別碼 (必須符合 Google 要求的格式)。
        /// 通常是自訂一組 Id，例如 "flight12345"。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// FlightClass 的唯一識別碼，必須是先建立的 FlightClass。
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 旅客的姓名 (顯示在票券上)。
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// 旅客的座位號碼，例如 "26"。
        /// </summary>
        public string SeatNumber { get; set; }

        /// <summary>
        /// 登機分組，例如 "A", "B", "C" 等。
        /// </summary>
        public string BoardingGroup { get; set; }

        /// <summary>
        /// 訂位代碼，例如 "ABC123"。
        /// </summary>
        public string ConfirmationCode { get; set; }

        /// <summary>
        /// 條碼的內容，例如可以放訂位代碼或票券號碼。
        /// </summary>
        public string BarcodeValue { get; set; }

        /// <summary>
        /// 卡券開始生效的時間（UTC）
        /// </summary>
        public string StartValidTime { get; set; }

        /// <summary>
        /// 卡券失效的時間（UTC）
        /// </summary>
        public string EndValidTime { get; set; }
    }
}
