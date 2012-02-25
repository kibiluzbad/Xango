using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Validator.Cfg;
using Xango.Data.NHibernate.Conventions;
using NH = NHibernate;

namespace Xango.Data.NHibernate.Configuration
{
    public static class NHFluentConfiguration
    {
        private static global::NHibernate.Cfg.Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        private static readonly object _syncRoot = new object();

        public static void Init(Action<global::NHibernate.Cfg.Configuration> exposedConfiguration = null)
        {
            lock (_syncRoot)
            {
                var automappingConfiguration = new XangoAutomapping();

                XangoConfiguration xangoConfig = XangoConfigurationHelper.Get();

                _configuration = Fluently.Configure()
                    .Database(xangoConfig.Database)
                    .ProxyFactoryFactory<ProxyFactoryFactory>()
                    .CurrentSessionContext(xangoConfig.IsWeb ? "web" : "thread_static")
                    .Mappings(mappings => mappings.AutoMappings.Add(AutoMap.Assembly(xangoConfig.DomainAssembly, automappingConfiguration)
                                                                        .UseOverridesFromAssembly(xangoConfig.OverrideMapAssembly ?? xangoConfig.DomainAssembly)
                                                                        .Conventions.Setup(conv =>
                                                                                               {
                                                                                                   conv.Add
                                                                                                       <
                                                                                                           CustomCollectionConvetion
                                                                                                           >();
                                                                                                   conv.Add<CascadeAll>();
                                                                                                   conv.Add
                                                                                                       <
                                                                                                           HiLoIndetityStrategy
                                                                                                           >();
                                                                                                   conv.Add
                                                                                                       <
                                                                                                           TableNamePluralizeConvention
                                                                                                           >();
                                                                                                   conv.Add
                                                                                                       <
                                                                                                           CustomForeignKeyConvention
                                                                                                           >();
                                                                                               })))
                    .ExposeConfiguration(Validation.NHFluentConfiguration.Initialize)
                    .ExposeConfiguration(exposedConfiguration)
                    .BuildConfiguration();

#if DEBUG
                var schema = new SchemaUpdate(_configuration);
                schema.Execute(true, true);
#endif

                _sessionFactory = _configuration.BuildSessionFactory();
            }
        }

        public static global::NHibernate.Cfg.Configuration Configuration
        {
            get { return _configuration; }
        }

        public static ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }
    }
}