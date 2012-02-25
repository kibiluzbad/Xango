using System;
using System.Reflection;
using FluentNHibernate.Cfg.Db;

namespace Xango.Data.NHibernate.Configuration
{
    public class XangoConfiguration
    {
        public Assembly DomainAssembly { get; private set; }
        public bool IsWeb { get; private set; }
        public IPersistenceConfigurer Database { get; private set; }
        public Assembly ValidationAssembly { get; private set; }
        public Assembly OverrideMapAssembly { get; private set; }

        public XangoConfiguration(Assembly domainAssembly,
            bool isWeb,
            IPersistenceConfigurer database,
            Assembly validationAssembly,
            Assembly overrideMapAssembly)
        {
            DomainAssembly = domainAssembly;
            IsWeb = isWeb;
            Database = database;
            ValidationAssembly = validationAssembly;
            OverrideMapAssembly = overrideMapAssembly;
        }
    }
}