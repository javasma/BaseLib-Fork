using System.ComponentModel;

namespace System
{
    public static class SystemExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            var enumString = @enum.ToString();
            var type = @enum.GetType();

            // Handle Flags enums with multiple values
            var memberNames = enumString.Contains(',')
                ? enumString.Split(new[] { ", " }, StringSplitOptions.None)
                : [enumString];

            return string.Join(", ", memberNames.Select(memberName =>
            {
                var memberInfo = type.GetMember(memberName)[0];
                if (memberInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute attr)
                {
                    return attr.Description;
                }

                return @enum.ToString();
            }));
        }

    }
}
