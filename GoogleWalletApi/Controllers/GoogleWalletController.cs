using System.Security.Cryptography;
using Google.Apis.Walletobjects.v1.Data;
using Microsoft.AspNetCore.Mvc;
using WalletLibrary.Define;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.WalletTypes.Flight.Models;
using WalletLibrary.Services.Interfaces;

namespace GoogleWalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleWalletController : Controller
    {
        /// <summary>華航的 Google Wallet 服務。</summary>
        private readonly IGooglePassService ChinaairLinesService;

        /// <summary>華航的 Google Wallet 處理器。</summary>
        private readonly IGoogleWalletHandler MandarinAirlinesHandler;

        public GoogleWalletController(
            [FromKeyedServices(IATADefine.CI)] IGooglePassService chinaairLinesService,
            [FromKeyedServices(IATADefine.AE)] IGoogleWalletHandler mandarinAirlinesHandler
        )
        {
            ChinaairLinesService = chinaairLinesService;
            MandarinAirlinesHandler = mandarinAirlinesHandler;
        }

        [HttpPost("PatchOjbectIdtoClassId")]
        public async Task<IActionResult> PatchOjbectIdtoClassId(string? classId, string? objectId)
        {
            FlightObject flightObject = new FlightObject();
            flightObject.State = FlightObjectDefine.State.ACTIVE;
            flightObject.Id =
                $"3388000000022913608.P{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(objectId) ? "" : $"_{objectId}")}";
            flightObject.ClassId =
                $"3388000000022913608.C{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(classId) ? "" : $"_{classId}")}";

            return Ok(await ChinaairLinesService.PatchObjectAsync(flightObject));
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

        [HttpPost("GetJWT")]
        public async Task<IActionResult> GetJWT(string? classId, string? objectId)
        {
            return Ok(
                await ChinaairLinesService.GetJwtToken(
                    $"C{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(classId) ? "" : $"_{classId}")}",
                    $"P{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(objectId) ? "" : $"_{objectId}")}"
                )
            );
        }

        [HttpPost("GetJWTByObjectId")]
        public async Task<IActionResult> GetJWTByObjectId(string? objectId)
        {
            return Ok(
                await ChinaairLinesService.GetJwtToken(
                    $"P{System.DateTime.Now.Date.ToString("yyyyMMdd")}{(string.IsNullOrEmpty(objectId) ? "" : $"_{objectId}")}"
                )
            );
        }
    }
}
