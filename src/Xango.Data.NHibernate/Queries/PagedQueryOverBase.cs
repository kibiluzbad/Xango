using System.Collections.Generic;
using NHibernate;
using Xango.Data.Query;

namespace Xango.Data.NHibernate.Queries
{
    public abstract class PagedQueryOverBase<T>
        : NHibernateQueryBase<PagedResult<T>>, IPagedQuery<T>
    {
        protected PagedQueryOverBase(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region IPagedQuery<T> Members

        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public override PagedResult<T> Execute()
        {
            IQueryOver<T, T> query = GetQuery();
            SetPaging(query);
            return Transact(() => Execute(query));
        }

        #endregion

        protected abstract IQueryOver<T, T> GetQuery();

        protected virtual void SetPaging(IQueryOver<T, T> query)
        {
            int maxResults = ItemsPerPage;
            int firstResult = (PageNumber - 1)*ItemsPerPage;

            query.Skip(firstResult).Take(maxResults);
        }

        protected virtual PagedResult<T> Execute(IQueryOver<T, T> query)
        {
            IEnumerable<T> results = query.Future<T>();
            IFutureValue<int> count = query.ToRowCountQuery().FutureValue<int>();

            return new PagedResult<T>
                       {
                           PageOfResults = results,
                           TotalItems = count.Value
                       };
        }
    }
}