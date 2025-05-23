using Microsoft.AspNetCore.Mvc;
using WalletLibrary.Define;
using WalletLibrary.GoogleLibrary.Base.Interfaces;
using WalletLibrary.Services.Interfaces;

namespace GoogleWalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleWalletController : Controller
    {
        /// <summary>華航的 Google Wallet 服務。</summary>
        private readonly IGoogleWalletService ChinaairLinesService;

        /// <summary>華航的 Google Wallet 處理器。</summary>
        private readonly IGoogleWalletHandler MandarinAirlinesHandler;

        public GoogleWalletController(
            [FromKeyedServices(AirLineIATADefine.CI)] IGoogleWalletService chinaairLinesService,
            [FromKeyedServices(AirLineIATADefine.AE)] IGoogleWalletHandler mandarinAirlinesHandler
        )
        {
            ChinaairLinesService = chinaairLinesService;
            MandarinAirlinesHandler = mandarinAirlinesHandler;
        }

        [HttpPost("CreateFlight")]
        public async Task<IActionResult> CreateFlight(string? classId, string? objectId)
        {
            var cId =
                $"C{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(classId) ? "" : $"_{classId}")}";

            return Ok(await ChinaairLinesService.CreateFlightAsync(cId));
        }

        [HttpPost("CreatePassenger")]
        public async Task<IActionResult> CreatePassenger(string? classId, string? objectId)
        {
            var cId =
                $"C{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(classId) ? "" : $"_{classId}")}";
            var oId =
                $"P{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(objectId) ? "" : $"_{objectId}")}";
            ;
            return Ok(await ChinaairLinesService.CreatePassengerAsync(cId, oId));
        }

        [HttpPost("BoardingPasses")]
        public async Task<IActionResult> GetBoardingPassesJwtToken(
            string? classId,
            string? objectId
        )
        {
            return Ok(
                ChinaairLinesService.GetBoardingPassesJwtToken(
                    $"C{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(classId) ? "" : $"_{classId}")}",
                    $"P{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(objectId) ? "" : $"_{objectId}")}"
                )
            );
        }

        [HttpPost("BoardingPassesByObjectId")]
        public async Task<IActionResult> GetBoardingPassesJwtTokenByObjectId(string? objectId)
        {
            return Ok(
                ChinaairLinesService.GetBoardingPassesJwtToken(
                    $"P{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(objectId) ? "" : $"_{objectId}")}"
                )
            );
        }

        [HttpPost("BoardingPassesExpire")]
        public async Task<IActionResult> BoardingPassesExpire(string? objectId)
        {
            return Ok(
                await ChinaairLinesService.ExpireFlightObjectAsync(
                    $"P{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(objectId) ? "" : $"_{objectId}")}"
                )
            );
        }
    }
}
