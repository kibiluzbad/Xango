using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Context;

namespace Xango.Data.NHibernate.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NHibernateSessionAttribute
        : ActionFilterAttribute
    {
        public NHibernateSessionAttribute()
        {
            Order = 100;
        }

        protected ISessionFactory SessionFactory
        {
            get { return ServiceLocator.Current.GetInstance<ISessionFactory>(); }
        }

        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        public override void OnActionExecuted(
            ActionExecutedContext filterContext)
        {
            ISession session = CurrentSessionContext.Unbind(SessionFactory);
            session.Close();
        }
    }
}