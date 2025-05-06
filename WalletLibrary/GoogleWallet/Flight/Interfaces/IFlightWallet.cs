using Google.Apis.Walletobjects.v1.Data;

namespace WalletLibrary.GoogleWallet.Flight.Interfaces
{
    /// <summary>
    /// 定義 Google Wallet 資源物件的標準操作介面，包含針對 Google Wallet 資源（如 FlightClass、FlightObject 等）的取得、建立、更新、部分更新、添加訊息與更新狀態等操作功能。<br/>
    /// 參考實作：https://github.com/google-wallet/rest-samples/blob/main/dotnet/DemoFlight.cs
    /// </summary>
    /// <typeparam name="T">Google Wallet 資源類型，例如 FlightClass、FlightObject 等。</typeparam>
    public interface IFlightWallet
    {
        /// <summary>
        /// 航班類別的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        IFlightClassRepository ClassRepository { get; }
        /// <summary>
        /// 航班對象的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        IFlightObjectRepository ObjectRepository { get; }
        #region
        /// <summary>
        /// 創建新的 FlightClass 和 FlightObject 並生成 "Add to Google Wallet" 的連結。
        /// </summary>
        /// <param name="flightClass">FlightClass 資源物件。</param>
        /// <param name="flightObject">FlightObject 資源物件。</param>
        /// <returns>返回 "Add to Google Wallet" 的連結。</returns>
        Task<string> CreateJWTNewObjects(
                    FlightClass flightClass,
                    FlightObject flightObject
                );
        #endregion

        #region 操作 Class Resource
        /// <summary>
        /// 獲取特定的 FlightClass 資源物件。
        /// </summary>
        /// <param name="resourceId">格式為 "{IssuerId}.{classSuffix}"。</param>
        /// <returns>返回獲取的 FlightClass 資源物件。</returns>
        Task<FlightClass> GetClassAsync(string resourceId);

        /// <summary>
        /// 新增 FlightClass 資源物件。
        /// </summary>
        /// <param name="flightClass">要新增的 FlightClass 資源物件。</param>
        /// <returns>返回新增的 FlightClass 資源物件。</returns>
        Task<FlightClass> InsertClassAsync(FlightClass flightClass);

        /// <summary>
        /// 更新現有的 FlightClass 資源物件。
        /// </summary>
        /// <param name="flightClass">要更新的 FlightClass 資源物件。</param>
        /// <returns>返回更新後的 FlightClass 資源物件。</returns>
        Task<FlightClass> UpdateClassAsync(FlightClass flightClass);

        /// <summary>
        /// 部分更新現有的 FlightClass 資源物件。
        /// </summary>
        /// <param name="flightClass">要部分更新的 FlightClass 資源物件。</param>
        /// <returns>返回部分更新後的 FlightClass 資源物件。</returns>
        Task<FlightClass> PatchClassAsync(FlightClass flightClass);

        /// <summary>
        /// 向指定的 FlightClass 資源物件添加訊息。
        /// </summary>
        /// <param name="addMessageRequest">訊息請求物件。</param>
        /// <param name="resourceId">格式為 "IssuerId.classSuffix"。</param>
        /// <returns>返回添加訊息後的 FlightClass 資源物件。</returns>
        Task<FlightClass> AddClassMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        );
        #endregion

        #region 操作 Object Resource
        /// <summary>
        /// 獲取特定的 FlightObject 資源物件。
        /// </summary>
        /// <param name="resourceId">格式為 "{IssuerId}.{objectSuffix}"。</param>
        /// <returns>返回獲取的 FlightObject 資源物件。</returns>
        Task<Google.Apis.Walletobjects.v1.Data.FlightObject> GetObjectAsync(string resourceId);

        /// <summary>
        /// 新增 FlightObject 資源物件。
        /// </summary>
        /// <param name="flightObject">要新增的 FlightObject 資源物件。</param>
        /// <returns>返回新增的 FlightObject 資源物件。</returns>
        Task<Google.Apis.Walletobjects.v1.Data.FlightObject> InsertObjectAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject);

        /// <summary>
        /// 更新現有的 FlightObject 資源物件。
        /// </summary>
        /// <param name="flightObject">要更新的 FlightObject 資源物件。</param>
        /// <returns>返回更新後的 FlightObject 資源物件。</returns>
        Task<Google.Apis.Walletobjects.v1.Data.FlightObject> UpdateObjectAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject);

        /// <summary>
        /// 部分更新現有的 FlightObject 資源物件。
        /// </summary>
        /// <param name="flightObject">要部分更新的 FlightObject 資源物件。</param>
        /// <returns>返回部分更新後的 FlightObject 資源物件。</returns>
        Task<Google.Apis.Walletobjects.v1.Data.FlightObject> PatchObjectAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject);

        /// <summary>
        /// 向指定的 FlightObject 資源物件添加訊息。
        /// </summary>
        /// <param name="addMessageRequest">訊息請求物件。</param>
        /// <param name="resourceId">格式為 "IssuerId.objectSuffix"。</param>
        /// <returns>返回添加訊息後的 FlightObject 資源物件。</returns>
        Task<Google.Apis.Walletobjects.v1.Data.FlightObject> AddObjectMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        );

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 為票券已過期(EXPIRED) 狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <returns>返回更新狀態後的 FlightObject 資源物件。</returns>
        Task<Google.Apis.Walletobjects.v1.Data.FlightObject> ExpireObjectAsync(string resourceId);

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 狀態為指定狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <param name="objectState">指定要更新的票券狀態。</param>
        /// <returns>返回更新狀態後的 FlightObject 資源物件。</returns>
        Task<Google.Apis.Walletobjects.v1.Data.FlightObject> UpdateObjectStateAsync(string resourceId, string objectState);
        #endregion
    }
}
