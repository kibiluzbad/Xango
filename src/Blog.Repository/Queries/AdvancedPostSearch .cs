using System.Collections.Generic;
using Blog.Domain;
using Blog.Domain.Queries;
using NHibernate;
using NHibernate.Criterion;
using Xango.Data.NHibernate.Queries;

namespace Blog.Repository.Queries
{
    public class AdvancedPostSearch
        : CriteriaQueryBase<IEnumerable<Post>>, IAdvancedPostSearch
    {
        public AdvancedPostSearch(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region IAdvancedPostSearch Members

        public string AuthorName { get; set; }
        public string Title { get; set; }
        public Period CreatedBetween { get; set; }

        #endregion

        protected override ICriteria GetCriteria()
        {
            return GetPostQuery().UnderlyingCriteria;
        }

        private IQueryOver GetPostQuery()
        {
            IQueryOver<Post, Post> query = session.QueryOver<Post>();

            ICriteria cr = session.CreateCriteria<Post>();
            cr.Add(Restrictions.InsensitiveLike("Title", Title, MatchMode.Anywhere));

            AddPostCriterion(query);

            return query;
        }

        private void AddPostCriterion(IQueryOver<Post, Post> query)
        {
            if (!string.IsNullOrWhiteSpace(AuthorName))
                query
                    .WhereRestrictionOn(c => c.Author)
                    .IsInsensitiveLike(AuthorName);

            if (!string.IsNullOrWhiteSpace(Title))
                query.WhereRestrictionOn(c => c.Title)
                    .IsInsensitiveLike(Title);

            if (null != CreatedBetween && !CreatedBetween.IsNull)
                query.WhereRestrictionOn(c => c.CreatedAt)
                    .IsBetween(CreatedBetween.Start)
                    .And(CreatedBetween.End);
        }

        protected override IEnumerable<Post> Execute(ICriteria criteria)
        {
            return criteria.List<Post>();
        }
    }
}