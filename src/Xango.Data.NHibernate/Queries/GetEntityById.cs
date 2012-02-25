using NHibernate;

namespace Xango.Data.NHibernate.Queries
{
    public class GetEntityById<TEntity>
         : CriteriaQueryBase<TEntity>
           , IGetEntityById<TEntity> where TEntity : Entity
    {
        public long Id { get; set; }

        public GetEntityById(ISessionFactory sessionFactory)
            : base(sessionFactory)
        { }

        protected override ICriteria GetCriteria()
        {
            return GetEntityQuery().UnderlyingCriteria;
        }

        private IQueryOver GetEntityQuery()
        {
            IQueryOver<TEntity, TEntity> query = session.QueryOver<TEntity>();
            AddEntityCriterion(query);
            return query;
        }

        private void AddEntityCriterion(IQueryOver<TEntity, TEntity> query)
        {
            query.Where(c => c.Id == Id);
        }


        protected override TEntity Execute(ICriteria criteria)
        {
            return criteria.UniqueResult<TEntity>();
        }
    }
}
