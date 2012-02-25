using System.Web.Mvc;

namespace Xango.Mvc.Filters
{
    public abstract class BaseActionFilter : IActionFilter, IResultFilter
    {
        #region IActionFilter Members

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        #endregion

        #region IResultFilter Members

        public virtual void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public virtual void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        #endregion
    }
}