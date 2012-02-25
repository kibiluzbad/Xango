using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Xango.Tests
{
    public abstract class NHibernateFixture : BaseFixture
    {
        protected ISessionFactory SessionFactory
        {
            get { return NHConfigurator.SessionFactory; }
        }

        protected ISession Session
        {
            get { return SessionFactory.GetCurrentSession(); }
        }

        protected override void OnSetup()
        {
            SetupNHibernateSession();
            base.OnSetup();
        }

        protected override void OnTeardown()
        {
            TearDownNHibernateSession();
            base.OnTeardown();
        }

        protected void SetupNHibernateSession()
        {
            TestConnectionProvider.CloseDatabase();
            SetupContextualSession();
            BuildSchema();
        }

        protected void TearDownNHibernateSession()
        {
            TearDownContextualSession();
            TestConnectionProvider.CloseDatabase();
        }

        private void SetupContextualSession()
        {
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        private void TearDownContextualSession()
        {
            ISessionFactory sessionFactory = NHConfigurator.SessionFactory;
            ISession session = CurrentSessionContext.Unbind(sessionFactory);
            session.Close();
        }

        private void BuildSchema()
        {
            Configuration cfg = NHConfigurator.Configuration;
            var schemaExport = new SchemaExport(cfg);
            schemaExport.Create(false, true);
        }
    }
}