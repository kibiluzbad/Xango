using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Xango.Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsAjaxRequest(this HttpContextBase context)
        {
            return "XMLHttpRequest".Equals(context.Request.Headers["X-Requested-With"],
                                           StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
