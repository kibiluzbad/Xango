using System;
using System.Linq;
using System.Web.Mvc;

namespace Xango.Mvc.Binders
{
    public class FlagEnumerationModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException("bindingContext");

            if (bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
            {
                var values = GetValue<string[]>(bindingContext, bindingContext.ModelName);

                if (null != values
                    && values.Length > 1
                    && (bindingContext.ModelType.IsEnum
                        && bindingContext.ModelType.IsDefined(typeof (FlagsAttribute), false)))
                {
                    var byteValue = values
                        .Where(v => Enum.IsDefined(bindingContext.ModelType, v))
                        .Aggregate<string, long>(0, (current, value) => current | (int) Enum.Parse(bindingContext.ModelType, value));

                    return Enum.Parse(bindingContext.ModelType, byteValue.ToString());
                }

                return base.BindModel(controllerContext, bindingContext);
            }

            return base.BindModel(controllerContext, bindingContext);
        }

        private static T GetValue<T>(ModelBindingContext bindingContext, string key)
        {
            if (bindingContext.ValueProvider.ContainsPrefix(key))
            {
                ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(key);
                if (valueResult != null)
                {
                    bindingContext.ModelState.SetModelValue(key, valueResult);
                    return (T) valueResult.ConvertTo(typeof (T));
                }
            }
            return default(T);
        }
    }
}
