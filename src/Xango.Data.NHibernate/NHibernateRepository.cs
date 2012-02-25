using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LinqSpecs;
using NHibernate;
using NHibernate.Linq;
using Xango.Data.Query;
using IQuery = Xango.Data.Query.IQuery;

namespace Xango.Data.NHibernate
{
    public class NHibernateRepository<TEntity> : NHibernateBase, IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly IQueryFactory _queryFactory;

        public NHibernateRepository(ISessionFactory sessionFactory,
                                    IQueryFactory queryFactory)
            : base(sessionFactory)
        {
            _queryFactory = queryFactory;
        }

        #region IRepository<TEntity> Members

        public void Add(TEntity item)
        {
            Transact(() => session.Save(item));
        }

        public bool Contains(TEntity item)
        {
            if (item.Id == default(long))
                return false;
            return Transact(() => session.Get<TEntity>(item.Id)) != null;
        }

        public int Count
        {
            get { return Transact(() => session.Query<TEntity>().Count()); }
        }

        public bool Remove(TEntity item)
        {
            Transact(() => session.Delete(item));
            return true;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Transact(() => session.Query<TEntity>()
                                      .Take(1000).GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Transact(() => GetEnumerator());
        }

        public IEnumerable<TEntity> FindAll(Specification<TEntity> specification)
        {
            IQueryable<TEntity> query = GetQuery(specification);
            return Transact(() => query.ToList());
        }

        public TEntity FindOne(Specification<TEntity> specification)
        {
            IQueryable<TEntity> query = GetQuery(specification);
            return Transact(() => query.SingleOrDefault());
        }

        public TQuery CreateQuery<TQuery>() where TQuery : IQuery
        {
            return _queryFactory.CreateQuery<TQuery>();
        }

        #endregion

        private IQueryable<TEntity> GetQuery(
            Specification<TEntity> specification)
        {
            return session.Query<TEntity>()
                .Where(specification.IsSatisfiedBy());
        }
    }
}