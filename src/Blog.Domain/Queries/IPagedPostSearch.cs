using Xango.Data.Query;

namespace Blog.Domain.Queries
{
    public interface IPagedPostSearch
        : IPagedQuery<Post>
    {
        string AuthorName { get; set; }
        string Title { get; set; }
        Period CreatedBetween { get; set; }
    }
}