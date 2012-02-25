using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xango.Data;

namespace Xango.Mvc.Extensions
{
    public static class ValidationExtension
    {
        public static void Validate<TEnt>(this System.Web.Mvc.Controller controller, TEnt entity) 
            where TEnt : Entity
        {
            var validatorEngine = NHibernate.Validator.Cfg.Environment.SharedEngineProvider.GetEngine();
            foreach (var error in validatorEngine.Validate(entity))
            {
                controller.ModelState.AddModelError(error.PropertyName,error.Message);
            }
        }
    }
}
