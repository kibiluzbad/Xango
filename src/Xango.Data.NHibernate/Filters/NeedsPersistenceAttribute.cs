using System;
using System.Text;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Validator.Exceptions;

namespace Xango.Data.NHibernate.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NeedsPersistenceAttribute
        : NHibernateSessionAttribute
    {
        protected ISession Session
        {
            get { return SessionFactory.GetCurrentSession(); }
        }

        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Session.BeginTransaction();
        }

        public override void OnActionExecuted(
            ActionExecutedContext filterContext)
        {
            ITransaction tx = Session.Transaction;

            if (tx != null && tx.IsActive)
            {
                try { Session.Transaction.Commit(); }
                catch (InvalidStateException validationException)
                {
                    var errors = validationException.GetInvalidValues();
                    var log = new StringBuilder();

                    foreach (var error in errors)
                        log.AppendLine(error.Message);
                    tx.Rollback();

                    throw new InvalidOperationException(log.ToString());
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}