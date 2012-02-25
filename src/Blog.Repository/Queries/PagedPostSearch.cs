using Blog.Domain;
using Blog.Domain.Queries;
using NHibernate;
using Xango.Data.NHibernate.Queries;

namespace Blog.Repository.Queries
{
    public class PagedPostSearch
        : PagedQueryOverBase<Post>, IPagedPostSearch
    {
        public PagedPostSearch(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region IPagedPostSearch Members

        public string AuthorName { get; set; }

        public string Title { get; set; }

        public Period CreatedBetween { get; set; }

        #endregion

        protected override IQueryOver<Post, Post> GetQuery()
        {
            IQueryOver<Post, Post> query = session.QueryOver<Post>();

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

            query
                .OrderBy(c => c.CreatedAt)
                .Desc
                .List<Post>();

            return query;
        }
    }
}