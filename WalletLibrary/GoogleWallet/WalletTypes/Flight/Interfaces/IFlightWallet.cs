using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.Base.Models;

namespace WalletLibrary.GoogleWallet.WalletTypes.Flight.Interfaces
{
    /// <summary>
    /// 定義 Google Wallet 資源物件的標準操作介面，包含針對 Google Wallet 資源（如 FlightClass、FlightObject 等）的取得、建立、更新、部分更新、添加訊息與更新狀態等操作功能。<br/>
    /// 參考實作：https://github.com/google-wallet/rest-samples/blob/main/dotnet/DemoFlight.cs
    /// </summary>
    /// <typeparam name="T">Google Wallet 資源類型，例如 FlightClass、FlightObject 等。</typeparam>
    public interface IFlightWallet
    {
        /// <summary>
        /// 航班類別的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        IFlightClassRepository ClassRepository { get; }

        /// <summary>
        /// 航班對象的處理器，用於與 Google Wallet API 交互。
        /// </summary>
        IFlightObjectRepository ObjectRepository { get; }

        #region
        /// <summary>
        /// 創建新的 FlightClass 和 FlightObject 並生成 "Add to Google Wallet" 的連結。
        /// </summary>
        /// <param name="flightClass">FlightClass 資源物件。</param>
        /// <param name="flightObject">FlightObject 資源物件。</param>
        /// <returns>返回 "Add to Google Wallet" 的連結。</returns>
        Task<string> CreateJWTNewObjects(FlightClass flightClass, FlightObject flightObject);

        /// <summary>
        /// 獲取 FlightClass 和 FlightObject 並生成 "Add to Google Wallet" 的連結。
        /// </summary>
        /// <param name="flightClassId">FlightClass ResourceId。</param>
        /// <param name="flightObjectId">FlightObject ResourceId。</param>
        /// <returns>返回 "Add to Google Wallet" 的連結。</returns>
        Task<string> GetJwtToken(string flightClassId, string flightObjectId);
        #endregion
    }
}
