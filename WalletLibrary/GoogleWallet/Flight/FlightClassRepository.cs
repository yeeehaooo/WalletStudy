using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.Flight.Interfaces;
using WalletLibrary.Logger;

namespace WalletLibrary.GoogleWallet.Flight
{
    /// <summary>
    /// 提供對 Google Wallet 中 FlightClass 的操作，包括建立、取得、更新、部分更新以及新增訊息。
    /// 繼承自 BaseHandlerLogger，具備統一的日誌與例外處理機制。
    /// </summary>
    public class FlightClassRepository : BaseHandlerLogger, IFlightClassRepository
    {
        private readonly WalletobjectsService _walletobjectsService;
        /// <summary>
        /// 初始化 FlightClassHandler 實例。
        /// </summary>
        /// <param name="walletobjectsService">Google Wallet API 的服務實例，用於操作 FlightClass。</param>
        public FlightClassRepository(WalletobjectsService walletobjectsService)
        {
            _walletobjectsService = walletobjectsService;
        }

        /// <summary>
        /// 根據資源 ID 取得指定的 FlightClass。
        /// </summary>
        /// <param name="resourceId">資源唯一識別碼（格式為 IssuerId.ClassId）。</param>
        /// <returns>指定的 FlightClass 物件。</returns>
        public async Task<FlightClass> GetAsync(string resourceId)
        {
            try
            {
                var request = _walletobjectsService.Flightclass.Get(resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "getting FlightClass");
                throw;
            }
        }

        /// <summary>
        /// 新增一個 FlightClass 至 Google Wallet。
        /// </summary>
        /// <param name="flightClass">要新增的 FlightClass 物件。</param>
        /// <returns>成功建立的 FlightClass。</returns>
        public async Task<FlightClass> InsertAsync(FlightClass flightClass)
        {
            try
            {
                var request = _walletobjectsService.Flightclass.Insert(flightClass);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "inserting FlightClass");
                throw;
            }
        }

        /// <summary>
        /// 更新既有的 FlightClass。
        /// </summary>
        /// <param name="flightClass">包含更新內容的 FlightClass。</param>
        /// <returns>更新後的 FlightClass。</returns>
        public async Task<FlightClass> UpdateAsync(FlightClass flightClass)
        {
            try
            {
                var request = _walletobjectsService.Flightclass.Update(flightClass, flightClass.Id);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "updating FlightClass");
                throw;
            }
        }

        /// <summary>
        /// 部分更新 FlightClass 的指定欄位。
        /// </summary>
        /// <param name="flightClass">只包含要更新欄位的 FlightClass。</param>
        /// <returns>部分更新後的 FlightClass。</returns>
        public async Task<FlightClass> PatchAsync(FlightClass flightClass)
        {
            try
            {
                var request = _walletobjectsService.Flightclass.Patch(flightClass, flightClass.Id);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response;
            }
            catch (Exception ex)
            {
                HandleException(ex, "patching FlightClass");
                throw;
            }
        }

        /// <summary>
        /// 向指定的 FlightClass 新增訊息，例如通知或更新說明。
        /// </summary>
        /// <param name="addMessageRequest">包含訊息內容的 AddMessageRequest。</param>
        /// <param name="resourceId">資源唯一識別碼（格式為 IssuerId.ClassId）。</param>
        /// <returns>新增訊息後的 FlightClass。</returns>
        public async Task<FlightClass> AddMessageAsync(AddMessageRequest addMessageRequest, string resourceId)
        {
            try
            {
                var request = _walletobjectsService.Flightclass.Addmessage(addMessageRequest, resourceId);
                var response = await request.ExecuteAsync();
                LogResponse(response);
                return response.Resource;
            }
            catch (Exception ex)
            {
                HandleException(ex, "adding message to FlightClass");
                throw;
            }
        }
    }
}
