using System.Runtime.Serialization;
using Google.Apis.Util;

namespace WalletLibrary.Utility
{
    public static partial class GoogleWalletUtility
    {
        public static string GetEnumMember<T>(this T enumValue)
            where T : Enum
        {
            var type = typeof(T);
            var member = type.GetMember(enumValue.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value ?? enumValue.ToString();
        }
    }
}
