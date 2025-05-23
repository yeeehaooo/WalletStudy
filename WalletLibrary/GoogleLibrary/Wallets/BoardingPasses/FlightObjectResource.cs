using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;
using Microsoft.Extensions.Logging;
using WalletLibrary.GoogleLibrary.Base.Interfaces;
using WalletLibrary.GoogleLibrary.Defines;
using WalletLibrary.Logger;

namespace WalletLibrary.GoogleLibrary.Wallets.BoardingPasses
{
    /// <summary>
    /// 提供對 Google Wallet 中 FlightObject 的操作，包括建立、取得、更新、部分更新以及新增訊息。
    /// 繼承自 BaseHandlerLogger，具備統一的日誌與例外處理機制。
    /// </summary>
    public class FlightObjectResource
        : BaseHandlerLogger<FlightObjectResource>,
            IObjectResource<FlightObject>
    {
        protected readonly WalletobjectsService _walletobjectsService;

        /// <summary>
        /// 初始化 FlightObjectHandler 實例。
        /// </summary>
        /// <param name="walletobjectsService">Google Wallet API 的服務實例，用於操作 FlightObject。</param>
        public FlightObjectResource(
            ILogger<FlightObjectResource> logger,
            WalletobjectsService walletobjectsService
        )
            : base(logger)
        {
            _walletobjectsService = walletobjectsService;
        }

        /// <summary>
        /// 根據資源 ID 取得指定的 FlightObject。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式為 IssuerId.ObjectId）。</param>
        /// <returns>指定的 FlightObject 物件。</returns>
        public async Task<FlightObject> GetAsync(string resourceId)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Get(resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Google.GoogleApiException notFouneEx)
                when (notFouneEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // 404 Not Found，表示資源不存在
                HandleGoogleApiException(
                    notFouneEx,
                    "getting FlightObject[Google Api Not Found Error]"
                );
                throw;
            }
            catch (Google.GoogleApiException otherEx)
            {
                // 其他 Google API 錯誤
                HandleGoogleApiException(otherEx, "getting FlightObject[Google Api Other Error]");
                throw;
            }
            catch (IOException ioEx)
            {
                // IO 錯誤，例如無法讀取回應流
                HandleIOException(ioEx, "getting FlightObject[IO Error]");
                throw;
            }
            catch (Exception ex)
            {
                HandleException(ex, "getting FlightObject[Error]");
                throw;
            }
        }

        /// <summary>
        /// 新增一個 FlightObject 至 Google Wallet。
        /// </summary>
        /// <param name="flightObject">要新增的 FlightObject 物件。</param>
        /// <returns>成功建立的 FlightObject。</returns>
        public async Task<FlightObject> InsertAsync(FlightObject flightObject)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Insert(flightObject);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Google.GoogleApiException alreadyExistsEx)
                when (alreadyExistsEx.HttpStatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // 404 Not Found，表示資源不存在
                HandleGoogleApiException(
                    alreadyExistsEx,
                    "getting FlightObject[Google Api Already Exists Error]"
                );
                throw;
            }
            catch (Google.GoogleApiException otherEx)
            {
                // 其他 Google API 錯誤
                HandleGoogleApiException(otherEx, "inserting FlightObject[Google Api Other Error]");
                throw;
            }
            catch (IOException ioEx)
            {
                // IO 錯誤，例如無法讀取回應流
                HandleIOException(ioEx, "inserting FlightObject[IO Error]");
                throw;
            }
            catch (Exception ex)
            {
                HandleException(ex, "inserting FlightObject[Error]");
                throw;
            }
        }

        /// <summary>
        /// 更新既有的 FlightObject。
        /// </summary>
        /// <param name="flightObject">包含更新內容的 FlightObject。</param>
        /// <returns>更新後的 FlightObject。</returns>
        public async Task<FlightObject> UpdateAsync(FlightObject flightObject)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Update(
                    flightObject,
                    flightObject.Id
                );
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Google.GoogleApiException notFouneEx)
                when (notFouneEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // 404 Not Found，表示資源不存在
                HandleGoogleApiException(
                    notFouneEx,
                    "updating FlightObject[Google Api Not Found Error]"
                );
                throw;
            }
            catch (Google.GoogleApiException otherEx)
            {
                // 其他 Google API 錯誤
                HandleGoogleApiException(otherEx, "updating FlightObject[Google Api Other Error]");
                throw;
            }
            catch (IOException ioEx)
            {
                // IO 錯誤，例如無法讀取回應流
                HandleIOException(ioEx, "updating FlightObject[IO Error]");
                throw;
            }
            catch (Exception ex)
            {
                HandleException(ex, "updating FlightObject[Error]");
                throw;
            }
        }

        /// <summary>
        /// 部分更新 FlightObject 的指定欄位。
        /// </summary>
        /// <param name="flightObject">只包含要更新欄位的 FlightObject。</param>
        /// <returns>部分更新後的 FlightObject。</returns>
        public async Task<FlightObject> PatchAsync(FlightObject flightObject)
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Patch(
                    flightObject,
                    flightObject.Id
                );
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Google.GoogleApiException notFouneEx)
                when (notFouneEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // 404 Not Found，表示資源不存在
                HandleGoogleApiException(
                    notFouneEx,
                    "patching FlightObject[Google Api Not Found Error]"
                );
                throw;
            }
            catch (Google.GoogleApiException otherEx)
            {
                // 其他 Google API 錯誤
                HandleGoogleApiException(otherEx, "patching FlightObject[Google Api Other Error]");
                throw;
            }
            catch (IOException ioEx)
            {
                // IO 錯誤，例如無法讀取回應流
                HandleIOException(ioEx, "patching FlightObject[IO Error]");
                throw;
            }
            catch (Exception ex)
            {
                HandleException(ex, "patching FlightObject[Error]");
                throw;
            }
        }

        /// <summary>
        /// 向指定的 FlightObject 新增訊息，例如通知或更新說明。
        /// </summary>
        /// <param name="addMessageRequest">包含訊息內容的 AddMessageRequest。</param>
        /// <param name="resourceId">資源唯一識別碼（格式為 IssuerId.ObjectId）。</param>
        /// <returns>新增訊息後的 FlightObject。</returns>
        public async Task<FlightObject> AddMessageAsync(
            AddMessageRequest addMessageRequest,
            string resourceId
        )
        {
            try
            {
                var request = _walletobjectsService.Flightobject.Addmessage(
                    addMessageRequest,
                    resourceId
                );
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response.Resource;
            }
            catch (Google.GoogleApiException notFouneEx)
                when (notFouneEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // 404 Not Found，表示資源不存在
                HandleGoogleApiException(
                    notFouneEx,
                    "adding Message to FlightObject[Google Api Not Found Error]"
                );
                throw;
            }
            catch (Google.GoogleApiException otherEx)
            {
                // 其他 Google API 錯誤
                HandleGoogleApiException(
                    otherEx,
                    "adding Message to FlightObject[Google Api Other Error]"
                );
                throw;
            }
            catch (IOException ioEx)
            {
                // IO 錯誤，例如無法讀取回應流
                HandleIOException(ioEx, "adding Message to FlightObject[IO Error]");
                throw;
            }
            catch (Exception ex)
            {
                HandleException(ex, "adding Message to FlightObject[Error]");
                throw;
            }
        }

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 票券狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <returns>對應的資源類別物件。</returns>
        public async Task<FlightObject> ExpireObjectAsync(string resourceId)
        {
            try
            {
                // Patch the object, setting the pass as expired
                FlightObject patchBody = new FlightObject
                {
                    State = GoogleDefine.State.EXPIRED.ToString(),
                };
                var request = _walletobjectsService.Flightobject.Patch(patchBody, resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Google.GoogleApiException notFouneEx)
                when (notFouneEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // 404 Not Found，表示資源不存在
                HandleGoogleApiException(
                    notFouneEx,
                    "expire FlightObject[Google Api Not Found Error]"
                );
                throw;
            }
            catch (Google.GoogleApiException otherEx)
            {
                // 其他 Google API 錯誤
                HandleGoogleApiException(otherEx, "expire FlightObject[Google Api Other Error]");
                throw;
            }
            catch (IOException ioEx)
            {
                // IO 錯誤，例如無法讀取回應流
                HandleIOException(ioEx, "expire FlightObject[IO Error]");
                throw;
            }
            catch (Exception ex)
            {
                HandleException(ex, "expire FlightObject[Error]");
                throw;
            }
        }

        /// <summary>
        /// 根據資源 ID 更新 Google Wallet 狀態為指定狀態。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式通常為 issuerId.objectId）。</param>
        /// <param name="objectState">指定要更新的票券狀態。</param>
        /// <returns>返回更新狀態後的 FlightObject 資源物件。</returns>
        public async Task<FlightObject> UpdateObjectStatusAsync(
            string resourceId,
            string objectState
        )
        {
            try
            {
                // Patch the object, setting the pass as expired
                FlightObject patchBody = new FlightObject { State = objectState };
                var request = _walletobjectsService.Flightobject.Patch(patchBody, resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Google.GoogleApiException notFouneEx)
                when (notFouneEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // 404 Not Found，表示資源不存在
                HandleGoogleApiException(
                    notFouneEx,
                    "updating Status to FlightObject[Google Api Not Found Error]"
                );
                throw;
            }
            catch (Google.GoogleApiException otherEx)
            {
                // 其他 Google API 錯誤
                HandleGoogleApiException(
                    otherEx,
                    "updating Status to FlightObject[Google Api Other Error]"
                );
                throw;
            }
            catch (IOException ioEx)
            {
                // IO 錯誤，例如無法讀取回應流
                HandleIOException(ioEx, "updating Status to FlightObject[IO Error]");
                throw;
            }
            catch (Exception ex)
            {
                HandleException(ex, "updating Status to FlightObject[Error]");
                throw;
            }
        }
    }
}
