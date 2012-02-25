using Xango.Data.Query;

namespace Blog.Domain.Queries
{
    public interface IFindPost
        : IQuery<Post>
    {
        long Id { get; set; }
    }
}