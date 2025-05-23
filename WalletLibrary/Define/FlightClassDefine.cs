using System.ComponentModel;
using System.Reflection;

namespace WalletLibrary.Define
{
    public static class FlightClassDefine
    {
        /// <summary>
        /// 審核狀態，表示物件的審核過程與結果。
        /// </summary>
        public static class ReviewStatus
        {
            /// <summary>
            /// 物件正在審核中，尚未通過審核，Google 尚未批准。
            /// </summary>
            public const string UNDER_REVIEW = "UNDER_REVIEW";

            /// <summary>
            /// 物件審核通過，已經正常上線並可以使用。
            /// </summary>
            public const string APPROVED = "APPROVED";

            /// <summary>
            /// 物件審核不通過，無法使用。
            /// </summary>
            public const string REJECTED = "REJECTED";
        }
    }
}
