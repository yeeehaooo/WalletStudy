using System.ComponentModel;
using System.Reflection;

namespace WalletLibrary.GoogleWallet.Define.Flight
{
    public static class FlightClassDefine
    {
        /// <summary>
        /// 審核狀態，表示物件的審核過程與結果。
        /// </summary>
        public enum ReviewStatus
        {
            /// <summary>
            /// 物件正在審核中，尚未通過審核，Google 尚未批准。
            /// </summary>
            [Description("UNDER_REVIEW")]
            UnderReview = 1,

            /// <summary>
            /// 物件審核通過，已經正常上線並可以使用。
            /// </summary>
            [Description("APPROVED")]
            Approved = 2,

            /// <summary>
            /// 物件審核不通過，無法使用。
            /// </summary>
            [Description("REJECTED")]
            Rejected = 3,
        }

        #region 輔助方法
        public static string GetDescription(this ReviewStatus status)
        {
            FieldInfo field = status.GetType().GetField(status.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)
                Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return attribute != null ? attribute.Description : status.ToString();
        }
        #endregion

    }
}
