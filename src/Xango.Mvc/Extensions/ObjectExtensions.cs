using System;
using System.ComponentModel;

namespace Xango.Mvc.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToSafeString(this object obj)
        {
            return "" + obj;
        }

        public static DateTime ToSafeDateTime(this object obj)
        {
            return ToSafeType<DateTime>(obj);
        }

        public static T ToSafeType<T>(this object obj)
        {
            var converter = TypeDescriptor.GetConverter(typeof (T));
            if (!string.IsNullOrWhiteSpace(obj.ToSafeString()) &&
                null != converter )
                return (T) converter.ConvertFromString(obj.ToSafeString());

            return default(T);
        }

        public static long ToSafeLong(this object obj)
        {
            return ToSafeType<long>(obj);
        }

        public static int ToSafeInt(this object obj)
        {
            return ToSafeType<int>(obj);
        }

        public static decimal ToSafeDecimal(this object obj)
        {
            return ToSafeType<decimal>(obj);
        }

        public static double ToSafeDouble(this object obj)
        {
            return ToSafeType<double>(obj);
        }
    }
}
