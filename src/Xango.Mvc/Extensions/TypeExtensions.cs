using System;

namespace Xango.Mvc.Extensions
{
    public static class TypeExtensions
    {
        public static void SetId(this Type type, object obj, object value)
        {
            var property = type.GetType().GetProperty("Id");
            if (null == property)
                throw new InvalidOperationException(string.Format("{0} não possui uma propriedade Id.", type));

            property.SetValue(obj, value, null);
        }
    }
}
