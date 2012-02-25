namespace Xango.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToStringSafe(this object value)
        {
            return "" + value;
        }
    }
}