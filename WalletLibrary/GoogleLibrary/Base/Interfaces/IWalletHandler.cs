namespace WalletLibrary.GoogleWallet.Base.Interfaces
{
    /// <summary>
    /// 定義 Google Wallet 資源物件的標準操作介面，包含針對 Google Wallet 資源（如 FlightClass、FlightObject 等）的取得、建立、更新、部分更新、添加訊息與更新狀態等操作功能。<br/>
    /// 參考實作：https://github.com/google-wallet/rest-samples/blob/main/dotnet/DemoFlight.cs
    /// </summary>
    /// <typeparam name="TClass">Google Wallet Class 資源類型，例如 FlightClass 等。</typeparam>
    /// <typeparam name="TObject">Google Wallet Object資源類型，例如 FlightObject 等。</typeparam>
    public interface IWalletHandler<TClass, TObject>
    {
        /// <summary>
        /// 航班類別的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        IClassRepository<TClass> ClassRepository { get; }

        /// <summary>
        /// 航班對象的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        IObjectRepository<TObject> ObjectRepository { get; }

        #region JWT

        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By Class Resource ID 和 Object Resource ID。
        /// </summary>
        /// <param name="classId">Class ResourceId。</param>
        /// <param name="objectId">Object ResourceId。</param>
        /// <returns>返回 "Add to Google Wallet" 的連結。</returns>
        Task<string> GetJwtToken(string classId, string objectId);

        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By Object Resource ID。
        /// </summary>
        /// <param name="objectResourceId">Object ResourceId。</param>
        /// <returns>返回 "Add to Google Wallet" 的連結。</returns>
        Task<string> GetJwtToken(string objectResourceId);
        #endregion
    }
}
