namespace WalletLibrary.GoogleLibrary.Settings
{
    /// <summary>
    /// Google Wallet 設定項目，包含發卡者資訊與金鑰檔案路徑等設定。
    /// </summary>
    public class GoogleWalletSettings
    {
        /// <summary>
        /// 發卡者 ID（Issuer ID），用來識別你的 Google Wallet 發卡帳戶。
        /// </summary>
        public string IssuerId { get; set; }

        /// <summary>
        /// 發卡者名稱（Issuer Name），通常是公司或組織名稱。
        /// </summary>
        public string IssuerName { get; set; }

        public List<string> Origins { get; set; }

        /// <summary>
        /// Service Account JSON 金鑰檔案的本機路徑，用來簽署 JWT。
        /// </summary>
        public string ServiceAccountJsonPath { get; set; }

        /// <summary>
        /// 儲存 JWT 所使用的 Google Wallet 儲存網址，可選擇寫成設定或固定使用 "https://pay.google.com/gp/v/save/"。
        /// </summary>
        //public string SaveBaseUrl { get; set; }

        /// <summary>
        /// 背景顏色 #000000。
        /// </summary>
        public string HexBackgroundColor { get; set; }

        /// <summary>
        /// 航空公司Logo的Id。
        /// </summary>
        public string LogoId { get; set; }

        /// <summary>
        /// 航空公司Logo的URI。
        /// </summary>
        public string LogoUri { get; set; }

        /// <summary>
        /// 航空公司Logo的文字說明。
        /// </summary>
        public string LogoDescription { get; set; }

        /// <summary>
        /// 航空公司主圖的Id。
        /// </summary>
        public string HeroImageId { get; set; }

        /// <summary>
        /// 航空公司主圖的URI。
        /// </summary>
        public string HeroImageUri { get; set; }

        /// <summary>
        /// 航空公司主圖的文字說明。
        /// </summary>
        public string HeroImageDescription { get; set; }

        /// <summary>
        /// Tsa PreCheck ICon Uri
        /// </summary>
        public string SecurityProgramLogo { get; set; }

        /// <summary>
        /// SkyPriority ICon Uri
        /// </summary>
        public string BoardingPrivilegeImage { get; set; }
    }
}
