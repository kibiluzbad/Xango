using NHibernate;
using Xango.Data.Query;

namespace Xango.Data.NHibernate.Queries
{
    public abstract class NHibernateQueryBase<TResult>
        : NHibernateBase, IQuery<TResult>
    {
        protected NHibernateQueryBase(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region IQuery<TResult> Members

        public abstract TResult Execute();

        #endregion
    }
}