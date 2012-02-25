using System.Web.Mvc;
using Xango.Mvc.ActionResults;

namespace Xango.Mvc.Extensions
{
    public static class ControllerExtensions
    {
        public static void WriteMessage(this System.Web.Mvc.Controller controller, string message)
        {
            controller.TempData["Success"] = message;
        }

        public static void WriteError(this System.Web.Mvc.Controller controller, string message)
        {
            controller.TempData["Error"] = message;
        }

        public static ActionResult JsonContract<TData>(this System.Web.Mvc.Controller controller, TData data)
        {
            return new JsonDataContractActionResult(data);
        }
    }
}