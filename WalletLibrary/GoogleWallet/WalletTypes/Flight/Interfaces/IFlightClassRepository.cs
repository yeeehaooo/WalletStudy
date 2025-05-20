using Google.Apis.Walletobjects.v1.Data;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Interfaces
{
    /// <summary>
    /// 定義 Google Wallet 資源類別（Class）的標準操作介面，
    /// 包括取得、建立、更新、部分更新以及新增訊息等功能。
    /// 可應用於 FlightClass 等資源。
    /// </summary>
    public interface IFlightClassRepository
    {
        /// <summary>
        /// 根據資源 ID 取得資源類別資訊。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.classId）。</param>
        /// <returns>對應的資源類別物件。</returns>
        Task<FlightClass> GetAsync(string resourceId);

        /// <summary>
        /// 建立新的資源類別。
        /// </summary>
        /// <param name="entity">要建立的資源類別物件。</param>
        /// <returns>建立後的資源類別物件。</returns>
        Task<FlightClass> InsertAsync(FlightClass entity);

        /// <summary>
        /// 完整更新指定的資源類別。
        /// </summary>
        /// <param name="entity">包含更新內容的資源類別物件。</param>
        /// <returns>更新後的資源類別物件。</returns>
        Task<FlightClass> UpdateAsync(FlightClass entity);

        /// <summary>
        /// 局部更新指定的資源類別（僅變更部分欄位）。
        /// </summary>
        /// <param name="entity">包含要更新欄位的資源類別物件。</param>
        /// <returns>局部更新後的資源類別物件。</returns>
        Task<FlightClass> PatchAsync(FlightClass entity);

        /// <summary>
        /// 為指定的資源類別新增訊息，例如通知或更新資訊。
        /// </summary>
        /// <param name="addMessageRequest">包含訊息內容的 AddMessageRequest 物件。</param>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.classId）。</param>
        /// <returns>新增訊息後的資源類別物件。</returns>
        Task<FlightClass> AddMessageAsync(AddMessageRequest addMessageRequest, string resourceId);
    }
}
