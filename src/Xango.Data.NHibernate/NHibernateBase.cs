using System;
using NHibernate;

namespace Xango.Data.NHibernate
{
    public abstract class NHibernateBase
    {
        protected readonly ISessionFactory _sessionFactory;

        protected NHibernateBase(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        protected virtual ISession session
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        protected virtual TResult Transact<TResult>(Func<TResult> func)
        {
            if (!session.Transaction.IsActive)
            {
                TResult result;
                using (ITransaction tx = session.BeginTransaction())
                {
                    result = func.Invoke();
                    tx.Commit();
                }
                return result;
            }

            return func.Invoke();
        }

        protected virtual void Transact(Action action)
        {
            Transact(() =>
                         {
                             action.Invoke();
                             return false;
                         });
        }
    }
}