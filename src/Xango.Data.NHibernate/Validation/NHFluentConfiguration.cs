using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using Xango.Data.NHibernate.Configuration;

namespace Xango.Data.NHibernate.Validation
{
    public static class NHFluentConfiguration
    {
        public static FluentConfiguration Configuration { get; private set; }

        public static void Initialize(global::NHibernate.Cfg.Configuration configuration)
        {
            XangoConfiguration xangoConfig = XangoConfigurationHelper.Get();

            Configuration = new FluentConfiguration();
            Configuration
                .SetDefaultValidatorMode(ValidatorMode.OverrideAttributeWithExternal)
                .Register(xangoConfig.ValidationAssembly
                .ValidationDefinitions())
                .IntegrateWithNHibernate
                .ApplyingDDLConstraints()
                .And
                .RegisteringListeners();

            var validatorEngine = new ValidatorEngine();
            validatorEngine.Configure(Configuration);

            new BasicSharedEngineProvider(validatorEngine,configuration).UseMe();
        }
    }
}
