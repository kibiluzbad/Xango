using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Xango.Mvc.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var result = string.Join(" ", fi
                                              .GetCustomAttributes(typeof (DescriptionAttribute), false)
                                              .Select(c => ((DescriptionAttribute) c).Description)
                                              .ToArray());

            return !string.IsNullOrWhiteSpace(result) 
                ? result 
                : value.ToString();
        }
    }
}
