using System.Collections.Generic;
using Xango.Data.Query;

namespace Blog.Domain.Queries
{
    public interface IAdvancedPostSearch
        : IQuery<IEnumerable<Post>>
    {
        string AuthorName { get; set; }
        string Title { get; set; }
        Period CreatedBetween { get; set; }
    }
}