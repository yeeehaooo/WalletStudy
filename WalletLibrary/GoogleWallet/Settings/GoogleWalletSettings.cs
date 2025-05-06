namespace WalletLibrary.GoogleWallet.Settings
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

        /// <summary>
        /// Service Account JSON 金鑰檔案的本機路徑，用來簽署 JWT。
        /// </summary>
        public string ServiceAccountJsonPath { get; set; }

        /// <summary>
        /// 儲存 JWT 所使用的 Google Wallet 儲存網址，可選擇寫成設定或固定使用 "https://pay.google.com/gp/v/save/"。
        /// </summary>
        //public string SaveBaseUrl { get; set; }

        /// <summary>
        /// 票券上方的主圖 URI，通常為橫幅圖片。
        /// </summary>
        public string HeroImageUri { get; set; }

        /// <summary>
        /// 主圖的文字說明，會顯示給使用者。
        /// </summary>
        public string HeroImageDescription { get; set; }

        /// <summary>
        /// 圖片模組中的圖片 URI，會顯示在票券下方的區域。
        /// </summary>
        public string ImageModuleUri { get; set; }

        /// <summary>
        /// 圖片模組圖片的說明。
        /// </summary>
        public string ImageModuleDescription { get; set; }

        /// <summary>
        /// 連結模組的 URI 清單，例如地圖或電話連結。
        /// </summary>
        public List<LinkModuleUriSetting> LinkModuleUris { get; set; }

        /// <summary>
        /// 預設地點的緯度，用於票券的定位模組。
        /// </summary>
        public double DefaultLatitude { get; set; }

        /// <summary>
        /// 預設地點的經度，用於票券的定位模組。
        /// </summary>
        public double DefaultLongitude { get; set; }
    }

    /// <summary>
    /// 單一連結模組 URI 的設定。
    /// </summary>
    public class LinkModuleUriSetting
    {
        /// <summary>
        /// URI 的值，例如地圖網址、電話或網站。
        /// </summary>
        public string UriValue { get; set; }

        /// <summary>
        /// URI 的描述文字，會顯示給使用者。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 此 URI 模組的唯一識別 ID。
        /// </summary>
        public string Id { get; set; }
    }
}
