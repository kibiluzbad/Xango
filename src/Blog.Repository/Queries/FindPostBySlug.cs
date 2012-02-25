using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Domain;
using Blog.Domain.Queries;
using NHibernate;
using NHibernate.Transform;
using Xango.Data.NHibernate.Queries;

namespace Blog.Repository.Queries
{
    public class FindPostBySlug : 
        CriteriaQueryBase<Post> , 
        IFindPostBySlug
    {
        public string Slug { get; set; }
        
        public FindPostBySlug(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

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
            query.Where(c => c.Slug == Slug);
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
