using System.Collections.Generic;
using Blog.Domain;
using Blog.Domain.Queries;
using NHibernate;
using Xango.Data.NHibernate.Queries;

namespace Blog.Repository.Queries
{
    public class LastPosts
        : CriteriaQueryBase<IEnumerable<Post>>
          , ILastPosts
    {
        public LastPosts(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        protected override ICriteria GetCriteria()
        {
            IQueryOver<Post, Post> query = GetPostQuery();
            return query.UnderlyingCriteria;
        }

        private IQueryOver<Post, Post> GetPostQuery()
        {
            IQueryOver<Post, Post> query = session.QueryOver<Post>();
            
            query.Take(5);
            query.OrderBy(c => c.CreatedAt).Desc.List<Post>();

            return query;
        }

        protected override IEnumerable<Post> Execute(ICriteria criteria)
        {
            return criteria.List<Post>();
        }
    }
}