using Google.Apis.Walletobjects.v1.Data;
using WalletLibrary.GoogleWallet.Settings;

namespace WalletLibrary.GoogleWallet.Base.Interfaces
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
        /// 生成 "Add to Google Wallet" 的連結 By Object Resource ID。
        /// </summary>
        /// <param name="objectResourceId">Object ResourceId。</param>
        /// <returns>返回 "Add to Google Wallet" 的連結。</returns>
        string GetJwtToken(string objectResourceId, string type);
    }
}
