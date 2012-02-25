using System;
using System.Web.Mvc;
using System.Globalization;

namespace Xango.Mvc.Binders
{
    public class CustomDateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var culture = GetUserCulture(controllerContext);

            string value = bindingContext
                               .ValueProvider
                               .GetValue(bindingContext.ModelName)
                               .ConvertTo(typeof(string)) as string;

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            try
            {
                return DateTime.Parse(value, culture.DateTimeFormat);
            }
            catch (FormatException)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Formato de data incorreto");
                return null;
            }
        }

        public CultureInfo GetUserCulture(ControllerContext context)
        {
            var request = context.HttpContext.Request;
            if (request.UserLanguages == null || request.UserLanguages.Length == 0)
                return CultureInfo.CurrentUICulture;

            return new CultureInfo(request.UserLanguages[0]);
        }
    }
}
