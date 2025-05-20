using Google;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WalletLibrary.Logger
{
    /// <summary>
    /// 提供共用的日誌輸出與例外處理功能。
    /// </summary>
    public class BaseHandlerLogger<T>
    {
        private ILogger<T> _logger;

        public BaseHandlerLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 將 API 回應物件序列化為 JSON 並輸出到主控台，方便除錯與監控。
        /// </summary>
        /// <param name="response">任意型別的 API 回應物件。</param>
        protected void LogResponse(object response)
        {
            _logger.LogDebug(JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// 輸出例外訊息與對應操作名稱，協助診斷 API 操作失敗原因。
        /// </summary>
        /// <param name="ex">捕獲的例外物件。</param>
        /// <param name="action">操作名稱，例如 "insert", "get" 等。</param>
        protected void HandleGoogleApiException(GoogleApiException ex, string action)
        {
            _logger.LogError("Error during {action}: {ErrorMessage}", action, ex.Error.Message);
        }

        /// <summary>
        /// 輸出例外訊息與對應操作名稱，協助診斷 API 操作失敗原因。
        /// </summary>
        /// <param name="ex">捕獲的例外物件。</param>
        /// <param name="action">操作名稱，例如 "insert", "get" 等。</param>
        protected void HandleException(Exception ex, string action)
        {
            _logger.LogError("Error during {action}: {ErrorMessage}", action, ex.Message);
        }
    }
}
