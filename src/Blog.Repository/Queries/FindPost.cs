using Blog.Domain;
using Blog.Domain.Queries;
using NHibernate;
using NHibernate.Transform;
using Xango.Data.NHibernate.Queries;

namespace Blog.Repository.Queries
{
    public class FindPost
        : CriteriaQueryBase<Post>
          , IFindPost
    {
        public FindPost(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region IFindPost Members

        public long Id { get; set; }

        #endregion

        protected override ICriteria GetCriteria()
        {
            return GetPostQuery().UnderlyingCriteria;
        }

        private IQueryOver GetPostQuery()
        {
            IQueryOver<Post, Post> query = session.QueryOver<Post>();
            AddPostCriterion(query);
            return query;
        }

        private void AddPostCriterion(IQueryOver<Post, Post> query)
        {
            query.Where(c => c.Id == Id);
        }

        protected override Post Execute(ICriteria criteria)
        {
            return criteria
                .SetFetchMode("Comments", FetchMode.Eager)
                .SetResultTransformer(new DistinctRootEntityResultTransformer())
                .UniqueResult<Post>();
        }
    }
}