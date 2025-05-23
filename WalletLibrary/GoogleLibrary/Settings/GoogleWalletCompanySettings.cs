namespace WalletLibrary.GoogleLibrary.Settings
{
    /// <summary>
    /// 包含所有公司對應的 Google Wallet 設定資訊集合，
    /// 以公司代碼為 Key，對應各自的 <see cref="GoogleWalletSettings"/>。
    /// </summary>
    public class CompanyGoogleWalletSettings
    {
        /// <summary>
        /// 各公司專屬的 Google Wallet 設定，Key 為IATA代碼（如 "CI", "AE"）。
        /// </summary>
        public Dictionary<string, GoogleWalletSettings> CompanySettings { get; set; }
    }
}
