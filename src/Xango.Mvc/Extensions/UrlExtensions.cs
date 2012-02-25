using System.Web.Mvc;

namespace Xango.Mvc.Extensions
{
    public static class UrlExtensions
    {
        public static string Image(this UrlHelper url, string image)
        {
            return url.Content(string.Format("~/Public/Images/{0}", image));
        }

        public static string Favicon(this UrlHelper url, string image = "favicon.ico")
        {
            return url.Content(string.Format("~/Public/{0}", image));
        }
    }
}