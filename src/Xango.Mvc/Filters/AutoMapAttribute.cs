using System;
using System.Web.Mvc;

namespace Xango.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoMapAttribute : ActionFilterAttribute
    {
        private readonly Type _destType;
        private readonly Type _sourceType;

        public AutoMapAttribute(Type sourceType, Type destType)
        {
            _sourceType = sourceType;
            _destType = destType;
        }

        public Type SourceType
        {
            get { return _sourceType; }
        }

        public Type DestType
        {
            get { return _destType; }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var filter = new AutoMapFilter(SourceType, DestType);

            filter.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}