using System.Net;
using Google;
using Newtonsoft.Json;

namespace GoogleWalletApi.Middleware
{
    /// <summary>
    /// 全域例外處理 Middleware，統一攔截未處理例外並格式化 API 回應。
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (GoogleApiException ex)
            {
                _logger.LogError(ex, "Google API 發生錯誤");
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    $"{{\"error\":\"Google 服務異常\",\"detail\":\"{ex.Message}\"}}"
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "參數錯誤");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{\"error\":\"{ex.Message}\"}}");
            }
            catch (IOException ex)
            {
                _logger.LogWarning(ex, "通訊失敗");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{\"error\":\"{ex.Message}\"}}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "未預期的系統錯誤");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\":\"系統錯誤，請稍後再試\"}");
            }
        }
    }
}
