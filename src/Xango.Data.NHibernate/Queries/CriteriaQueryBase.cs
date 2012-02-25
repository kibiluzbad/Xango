using NHibernate;

namespace Xango.Data.NHibernate.Queries
{
    public abstract class CriteriaQueryBase<TResult>
        : NHibernateQueryBase<TResult>, ICriteriaQuery
    {
        protected CriteriaQueryBase(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        public override TResult Execute()
        {
            ICriteria criteria = GetCriteria();
            return Transact(() => Execute(criteria));
        }

        protected abstract ICriteria GetCriteria();

        protected abstract TResult Execute(ICriteria criteria);
    }
}