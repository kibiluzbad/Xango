using System;
using System.Configuration;
using System.Reflection;

namespace Xango.Data.NHibernate.Configuration
{
    public static class XangoConfigurationHelper
    {
        public static XangoConfiguration Get()
        {
            var config = (XangoConfigurationSection)
                         ConfigurationManager.GetSection(
                             "xangoGroup/xangoConfiguration");

            if (null == config)
                throw new InvalidOperationException("Não foi possível encontrar uma seção de configuração do Xangô.");

            return new XangoConfiguration(Assembly.Load(config.DomainAssemblyName),
                bool.Parse(config.IsWeb),
                DatabaseFactory.Get(config.Database),
                Assembly.Load(config.ValidationAssemblyName),
                !string.IsNullOrWhiteSpace(config.OverrideMapAssemblyName) ? Assembly.Load(config.OverrideMapAssemblyName) : null);
        }
    }
}