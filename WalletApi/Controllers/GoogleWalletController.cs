using Google.Apis.Walletobjects.v1.Data;
using Microsoft.AspNetCore.Mvc;
using WalletLibrary.Base.Define;
using WalletLibrary.GoogleWallet.Base.Interfaces;
using WalletLibrary.GoogleWallet.Define.Flight;
using WalletLibrary.GoogleWallet.Services.Interfaces;
using WalletLibrary.GoogleWallet.WalletTypes.Flight.Models;

namespace GoogleWalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleWalletController : Controller
    {
        /// <summary>華航的 Google Wallet 服務。</summary>
        private readonly IBoardingPassService ChinaairLinesService;

        /// <summary>華航的 Google Wallet 處理器。</summary>
        private readonly IGoogleWalletHandler MandarinAirlinesHandler;

        public GoogleWalletController(
            [FromKeyedServices(IATADefine.CI)] IBoardingPassService chinaairLinesService,
            [FromKeyedServices(IATADefine.AE)] IGoogleWalletHandler mandarinAirlinesHandler
        )
        {
            ChinaairLinesService = chinaairLinesService;
            MandarinAirlinesHandler = mandarinAirlinesHandler;
        }

        [HttpPost("CreateClass")]
        public async Task<IActionResult> CreateClass(BoardingPassWalletModel boardingPass)
        {
            return Ok(await ChinaairLinesService.InsertClassAsync(boardingPass));
        }

        //[HttpPost("PatchClass")]
        //public async Task<IActionResult> PatchClass(string classId)
        //{
        //    var result = await ChinaairLinesService.GetClassByClassIdAsync(classId);
        //    var flightClass = result?.Data;

        //    flightClass.ClassTemplateInfo = null; // ChinaairLinesService.CreateFlightCardTemplate();

        //    return Ok(await ChinaairLinesService.PatchClassAsync(flightClass));
        //}

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

        [HttpPost("CreateObject")]
        public async Task<IActionResult> CreateObject(BoardingPassWalletModel boardingPass)
        {
            return Ok(await ChinaairLinesService.InsertObjectAsync(boardingPass));
        }

        [HttpPost("PatchObject")]
        public async Task<IActionResult> PatchClass(BoardingPassWalletModel boardingPass)
        {
            return Ok(
                await ChinaairLinesService.PatchObjectAsync(
                    ChinaairLinesService.BuildFlightObject(boardingPass)
                )
            );
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

        [HttpPost("GetClass")]
        public async Task<IActionResult> GetClass(string classId)
        {
            return Ok(await ChinaairLinesService.GetClassByClassIdAsync(classId));
        }

        [HttpPost("GetObject")]
        public async Task<IActionResult> GetObject(string objectId)
        {
            return Ok(await ChinaairLinesService.GetObjectByObjectIdAsync(objectId));
        }
    }
}
