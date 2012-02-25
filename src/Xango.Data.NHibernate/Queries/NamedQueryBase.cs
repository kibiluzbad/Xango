using NHibernate;

namespace Xango.Data.NHibernate.Queries
{
    public abstract class NamedQueryBase<TResult>
        : NHibernateQueryBase<TResult>, INamedQuery
    {
        protected NamedQueryBase(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region INamedQuery Members

        public virtual string QueryName
        {
            get { return GetType().Name; }
        }

        #endregion

        public override TResult Execute()
        {
            IQuery query = GetNamedQuery();
            return Transact(() => Execute(query));
        }

        protected abstract TResult Execute(IQuery query);

        protected virtual IQuery GetNamedQuery()
        {
            IQuery query = session.GetNamedQuery(((INamedQuery) this).QueryName);
            SetParameters(query);
            return query;
        }

        protected abstract void SetParameters(IQuery query);
    }
}