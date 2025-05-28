using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleLibrary.Settings;

namespace WalletLibrary.GoogleLibrary.Base.Interfaces
{
    public interface IGoogleWalletHandler
    {
        GoogleWalletSettings WalletSettings { get; }

        IWalletHandler<FlightClass, FlightObject> FlightWallet { get; }

        // 預留：未來如需擴充
        //IGiftCardWallet GiftCardWallet { get; }


        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By Class Resource ID 和 Object Resource ID。
        /// </summary>
        /// <param name="classId">Class ResourceId。</param>
        /// <param name="objectId">Object ResourceId。</param>
        /// <returns>返回 "Add to Google Wallet" 的連結。</returns>
        string GetJwtToken(string classId, string objectId, string type);

        /// <summary>
        /// 生成 "Add to Google Wallet" 的連結 By FlightObject ID。<br/>
        /// 建議排程預先建立 Flight Class & Flight Object<br/>
        /// </summary>
        /// <param name="objectResourceId">航班對象的 ID。</param>
        /// <returns>返回一個 "Add to Google Wallet" 的鏈接。</returns>
        string GetJwtToken(string objectResourceId, string type);
    }
}
