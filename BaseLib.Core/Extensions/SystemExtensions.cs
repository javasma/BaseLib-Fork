using System.ComponentModel;
using System.Linq;

namespace System
{
    public static class SystemExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            var memberName = @enum.ToString();
            var type = @enum.GetType();
            var memberInfo = type.GetMember(memberName)[0];
            var attr = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            if (attr != null)
            {
                return attr.Description;
            }

            return @enum.ToString();
        }

    }
}
