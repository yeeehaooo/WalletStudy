namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Define
{
    /// <summary>
    /// 定義 Google Wallet FlightObject 的狀態。
    /// </summary>
    public static class FlightObjectDefine
    {
        public static class State
        {
            /// <summary>
            /// 有效票券。使用者可在 Google Wallet 中查看與使用。
            /// </summary>
            public const string ACTIVE = "ACTIVE";

            /// <summary>
            /// 票券已完成，例如航班已登機。
            /// </summary>
            public const string COMPLETED = "COMPLETED";

            /// <summary>
            /// 票券已過期。
            /// </summary>
            public const string EXPIRED = "EXPIRED";

            /// <summary>
            /// 無效票券，例如被取消或停用。
            /// </summary>
            public const string INACTIVE = "INACTIVE";
        }
    }
}
