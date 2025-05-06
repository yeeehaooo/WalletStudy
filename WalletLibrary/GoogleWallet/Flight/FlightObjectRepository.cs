using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.Define.Flight;
using WalletLibrary.GoogleWallet.Flight.Interfaces;
using WalletLibrary.Logger;

namespace WalletLibrary.GoogleWallet.Flight
{
    /// <summary>
    /// 提供對 Google Wallet 中 FlightObject 的操作，包括建立、取得、更新、部分更新以及新增訊息。
    /// 繼承自 BaseHandlerLogger，具備統一的日誌與例外處理機制。
    /// </summary>
    public class FlightObjectRepository : BaseHandlerLogger, IFlightObjectRepository
    {
        protected readonly WalletobjectsService _walletobjectsService;

        /// <summary>
        /// 初始化 FlightObjectHandler 實例。
        /// </summary>
        /// <param name="walletobjectsService">Google Wallet API 的服務實例，用於操作 FlightObject。</param>
        public FlightObjectRepository(WalletobjectsService walletobjectsService)
        {
            _walletobjectsService = walletobjectsService;
        }

        /// <summary>
        /// 根據資源 ID 取得指定的 FlightObject。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式為 IssuerId.ObjectId）。</param>
        /// <returns>指定的 FlightObject 物件。</returns>
        public async Task<Google.Apis.Walletobjects.v1.Data.FlightObject> GetAsync(string resourceId)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Get(resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "getting FlightObject");
                throw;
            }
        }

        /// <summary>
        /// 新增一個 FlightObject 至 Google Wallet。
        /// </summary>
        /// <param name="flightObject">要新增的 FlightObject 物件。</param>
        /// <returns>成功建立的 FlightObject。</returns>
        public async Task<Google.Apis.Walletobjects.v1.Data.FlightObject> InsertAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Insert(flightObject);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "inserting FlightObject");
                throw;
            }
        }

        /// <summary>
        /// 更新既有的 FlightObject。
        /// </summary>
        /// <param name="flightObject">包含更新內容的 FlightObject。</param>
        /// <returns>更新後的 FlightObject。</returns>
        public async Task<Google.Apis.Walletobjects.v1.Data.FlightObject> UpdateAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Update(flightObject, flightObject.Id);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "updating FlightObject");
                throw;
            }
        }

        /// <summary>
        /// 部分更新 FlightObject 的指定欄位。
        /// </summary>
        /// <param name="flightObject">只包含要更新欄位的 FlightObject。</param>
        /// <returns>部分更新後的 FlightObject。</returns>
        public async Task<Google.Apis.Walletobjects.v1.Data.FlightObject> PatchAsync(Google.Apis.Walletobjects.v1.Data.FlightObject flightObject)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Patch(flightObject, flightObject.Id);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "patching FlightObject");
                throw;
            }
        }

        /// <summary>
        /// 向指定的 FlightObject 新增訊息，例如通知或更新說明。
        /// </summary>
        /// <param name="addMessageRequest">包含訊息內容的 AddMessageRequest。</param>
        /// <param name="resourceId">資源唯一識別碼（格式為 IssuerId.ObjectId）。</param>
        /// <returns>新增訊息後的 FlightObject。</returns>
        public async Task<Google.Apis.Walletobjects.v1.Data.FlightObject> AddMessageAsync(AddMessageRequest addMessageRequest, string resourceId)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Addmessage(addMessageRequest, resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response.Resource;
            }
            catch (Exception ex)
            {
                HandleException(ex, "adding message to FlightObject");
                throw;
            }
        }

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 票券狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <returns>對應的資源類別物件。</returns>
        public async Task<Google.Apis.Walletobjects.v1.Data.FlightObject> ExpireObjectAsync(string resourceId)
        {
            try
            {
                // Patch the object, setting the pass as expired
                Google.Apis.Walletobjects.v1.Data.FlightObject patchBody = new Google.Apis.Walletobjects.v1.Data.FlightObject
                {
                    State = FlightObjectDefine.State.EXPIRED
                };
                var request = _walletobjectsService.Flightobject.Patch(patchBody, resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "expiring FlightObject");
                throw;
            }
        }

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 狀態為指定狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <param name="objectState">指定要更新的票券狀態。</param>
        /// <returns>返回更新狀態後的 FlightObject 資源物件。</returns>
        public async Task<Google.Apis.Walletobjects.v1.Data.FlightObject> UpdateObjectStateAsync(string resourceId, string objectState)
        {
            try
            {
                // Patch the object, setting the pass as expired
                Google.Apis.Walletobjects.v1.Data.FlightObject patchBody = new Google.Apis.Walletobjects.v1.Data.FlightObject
                {
                    State = objectState
                };
                var request = _walletobjectsService.Flightobject.Patch(patchBody, resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "expiring FlightObject");
                throw;
            }
        }
    }
}
