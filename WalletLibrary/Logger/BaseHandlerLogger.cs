using Google;
using Microsoft.Extensions.Logging;

namespace WalletLibrary.Logger
{
    /// <summary>
    /// 提供共用的日誌輸出與例外處理功能。
    /// </summary>
    public class BaseHandlerLogger<T>
    {
        private readonly ILogger<T> _logger;

        public BaseHandlerLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 將 API 回應物件序列化為 JSON 並輸出到主控台，方便除錯與監控。
        /// </summary>
        public void LogResponse(object response, string action = "response")
        {
            _logger.LogDebug("Action: {Action}, Response: {@Response}", action, response);
        }

        /// <summary>
        /// 結構化輸出 Google API 例外
        /// </summary>
        public void HandleGoogleApiException(GoogleApiException ex, string action)
        {
            _logger.LogError(
                ex,
                "Google API Error during {Action}. Code: {ErrorCode}, Message: {ErrorMessage}, Reason: {Reason}",
                action,
                ex.Error.Code,
                ex.Error.Message,
                ex.Error.Errors?.FirstOrDefault()?.Reason
            );
        }

        public void HandleIOException(IOException ex, string action)
        {
            _logger.LogError(
                ex,
                "IO error during {Action}. Message: {ErrorMessage}, ExceptionType: {ExceptionType}, HResult: {HResult}, Source: {Source}",
                action,
                ex.Message,
                ex.GetType().Name,
                ex.HResult,
                ex.Source
            );
        }

        /// <summary>
        /// 一般例外處理，支援結構化日誌輸出
        /// </summary>
        public void HandleException(Exception ex, string action)
        {
            _logger.LogError(
                ex,
                "Unexpected error during {Action}. Message: {ErrorMessage}, ExceptionType: {ExceptionType}",
                action,
                ex.Message,
                ex.GetType().Name
            );
        }
    }
}
