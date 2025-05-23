using System.Runtime.Serialization;
using Google.Apis.Util;

namespace WalletLibrary.Utility
{
    public static partial class GoogleWalletUtility
    {
        public static string GetEnumMember<T>(this T barcodeType)
            where T : Enum
        {
            var type = typeof(T);
            var member = type.GetMember(barcodeType.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value ?? barcodeType.ToString();
        }
    }
}
