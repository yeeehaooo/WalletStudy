using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.WalletTypes.Flight.Models;

namespace WalletLibrary.Services.Interfaces
{
    public interface IGooglePassService
    {
        public Task<string> GetJwtToken(string flightClassId, string flightObjectId);
        public Task<string> GetJwtToken(string flightObjectId);

        public Task<string> CreateFlightAsync(string classId);
        public Task<string> CreatePassengerAsync(string classId, string objectId);

        /// <summary>
        /// 新增 FlightClass 資源物件。
        /// </summary>
        /// <param name="flightInfo">要新增的航班資訊。</param>
        /// <returns>返回新增的 FlightClass 資源物件。</returns>
        public Task<FlightClass> InsertFlightInfoAsync(FlightInfo flightInfo);

        /// <summary>
        /// 新增 FlightClass 資源物件。
        /// </summary>
        /// <param name="passengerInfo">要新增的航班旅客資訊。</param>
        /// <returns>返回新增的 FlightClass 資源物件。</returns>
        public Task<FlightObject> InsertPassengerInfoAsync(PassengerInfo passengerInfo);

        #region 操作 Class Resource
        /// <summary>
        /// 獲取特定的 FlightClass 資源物件。
        /// </summary>
        /// <param name="classId"></param>
        /// <returns>返回獲取的 FlightClass 資源物件。</returns>
        public Task<FlightClass> GetClassByClassIdAsync(string classId);

        /// <summary>
        /// 獲取特定的 FlightClass 資源物件。
        /// </summary>
        /// <param name="resourceId">格式為 "{IssuerId}.{classSuffix}"。</param>
        /// <returns>返回獲取的 FlightClass 資源物件。</returns>
        public Task<FlightClass> GetClassByResourceIdAsync(string resourceId);

        /// <summary>
        /// 新增 FlightClass 資源物件。
        /// </summary>
        /// <param name="flightClass">要新增的 FlightClass 資源物件。</param>
        /// <returns>返回新增的 FlightClass 資源物件。</returns>
        public Task<FlightClass> InsertClassAsync(FlightClass flightClass);

        /// <summary>
        /// 更新現有的 FlightClass 資源物件。
        /// </summary>
        /// <param name="flightClass">要更新的 FlightClass 資源物件。</param>
        /// <returns>返回更新後的 FlightClass 資源物件。</returns>
        public Task<FlightClass> UpdateClassAsync(FlightClass flightClass);

        /// <summary>
        /// 部分更新現有的 FlightClass 資源物件。
        /// </summary>
        /// <param name="flightClass">要部分更新的 FlightClass 資源物件。</param>
        /// <returns>返回部分更新後的 FlightClass 資源物件。</returns>
        public Task<FlightClass> PatchClassAsync(FlightClass flightClass);

        /// <summary>
        /// 向指定的 FlightClass 資源物件添加訊息。
        /// </summary>
        /// <param name="addMessageRequest">訊息請求物件。</param>
        /// <param name="resourceId">格式為 "IssuerId.classSuffix"。</param>
        /// <returns>返回添加訊息後的 FlightClass 資源物件。</returns>
        public Task<FlightClass> AddClassMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        );
        #endregion

        #region 操作 Object Resource
        /// <summary>
        /// 獲取特定的 FlightObject 資源物件。
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns>返回獲取的 FlightObject 資源物件。</returns>
        public Task<FlightObject> GetObjectByObjectIdAsync(string objectId);

        /// <summary>
        /// 獲取特定的 FlightObject 資源物件。
        /// </summary>
        /// <param name="resourceId">格式為 "{IssuerId}.{objectSuffix}"。</param>
        /// <returns>返回獲取的 FlightObject 資源物件。</returns>
        public Task<FlightObject> GetObjectByResourceIdAsync(string resourceId);

        /// <summary>
        /// 新增 FlightObject 資源物件。
        /// </summary>
        /// <param name="flightObject">要新增的 FlightObject 資源物件。</param>
        /// <returns>返回新增的 FlightObject 資源物件。</returns>
        public Task<FlightObject> InsertObjectAsync(FlightObject flightObject);

        /// <summary>
        /// 更新現有的 FlightObject 資源物件。
        /// </summary>
        /// <param name="flightObject">要更新的 FlightObject 資源物件。</param>
        /// <returns>返回更新後的 FlightObject 資源物件。</returns>
        public Task<FlightObject> UpdateObjectAsync(FlightObject flightObject);

        /// <summary>
        /// 部分更新現有的 FlightObject 資源物件。
        /// </summary>
        /// <param name="flightObject">要部分更新的 FlightObject 資源物件。</param>
        /// <returns>返回部分更新後的 FlightObject 資源物件。</returns>
        public Task<FlightObject> PatchObjectAsync(FlightObject flightObject);

        /// <summary>
        /// 向指定的 FlightObject 資源物件添加訊息。
        /// </summary>
        /// <param name="addMessageRequest">訊息請求物件。</param>
        /// <param name="resourceId">格式為 "IssuerId.objectSuffix"。</param>
        /// <returns>返回添加訊息後的 FlightObject 資源物件。</returns>
        public Task<FlightObject> AddObjectMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        );

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 為票券已過期(EXPIRED) 狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <returns>返回更新狀態後的 FlightObject 資源物件。</returns>
        public Task<FlightObject> ExpireObjectAsync(string resourceId);

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 狀態為指定狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <param name="objectState">指定要更新的票券狀態。</param>
        /// <returns>返回更新狀態後的 FlightObject 資源物件。</returns>
        public Task<FlightObject> UpdateObjectStatusAsync(string resourceId, string objectState);
        #endregion
    }
}
