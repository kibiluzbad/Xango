using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;

namespace Xango.Data.NHibernate.Validation
{
    public class BasicSharedEngineProvider 
        : ISharedEngineProvider
    {
        private readonly ValidatorEngine _ve;
        private readonly global::NHibernate.Cfg.Configuration _configuration;

        public BasicSharedEngineProvider(ValidatorEngine ve, global::NHibernate.Cfg.Configuration configuration)
        {
            _ve = ve;
            _configuration = configuration;
        }

        public ValidatorEngine GetEngine()
        {
            return _ve;
        }
        public void UseMe()
        {
            Environment.SharedEngineProvider = this;
            _configuration.Initialize(_ve);
        }
    }
}
