using System;
using System.Data;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using Xango.Data;
using Xango.Data.NHibernate.Conventions;
using Xango.Data.NHibernate.Validation;
using Xango.Tests;

namespace Blog.Tests
{
    public abstract class NHibernateBaseFixture : BaseFixture
    {
        protected ISession Session;
        protected ISessionFactory SessionFactory;

        protected override void OnFixtureSetup()
        {
            Configuration configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                              .ConnectionString(connstr =>
                                                connstr.Is("Data Source=:memory:;Version=3;New=True;"))
                              .Dialect<SQLiteDialect>()
                              .Driver<SQLite20Driver>()
                              .Provider<TestConnectionProvider>()
                              .ShowSql())
                .CurrentSessionContext("thread_static")
                .ProxyFactoryFactory<ProxyFactoryFactory>()
                .Mappings(m =>
                          m.AutoMappings
                              .Add(
                                  AutoMap.Assembly(
                                      Assembly.Load("Blog.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"))
                                      .Where(c => c.BaseType == typeof (Entity) || c.BaseType == typeof (StampedEntity))
                                      .Conventions
                                      .Add<CustomCollectionConvetion>()
                                      .Conventions
                                      .Add<CascadeAll>()
                                      .Conventions.Add<HiLoIndetityStrategy>()
                                      .Conventions.Add<TableNamePluralizeConvention>()
                                      .Conventions.Add<CustomForeignKeyConvention>()))
                                      
                .ExposeConfiguration(ConfigValidator)
                .BuildConfiguration();

            var schema = new SchemaUpdate(configuration);
            schema.Execute(true, true);

            SessionFactory = configuration.BuildSessionFactory();
        }

        private static void ConfigValidator(Configuration config)
        {
            var configuration = new NHibernate.Validator.Cfg.Loquacious.FluentConfiguration();
            configuration
                .SetDefaultValidatorMode(ValidatorMode.OverrideAttributeWithExternal)
                .Register(Assembly.Load("Blog.Repository, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                .ValidationDefinitions())
                .IntegrateWithNHibernate
                .ApplyingDDLConstraints()
                .And
                .RegisteringListeners();

            var validatorEngine = new ValidatorEngine();
            validatorEngine.Configure(configuration);

            new BasicSharedEngineProvider(validatorEngine, config).UseMe();
        }

        protected override void OnSetup()
        {
            Session = SessionFactory.OpenSession();
            Session.BeginTransaction(IsolationLevel.Serializable);
            CurrentSessionContext.Bind(Session);
        }


        protected override void OnTeardown()
        {
            Session.Transaction.Rollback();
            Session.Close();
            Session.Dispose();
        }


        protected override void OnFixtureTeardown()
        {
            SessionFactory.Dispose();
        }
    }
}