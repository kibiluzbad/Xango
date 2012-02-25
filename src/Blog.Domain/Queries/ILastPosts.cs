using System.Collections.Generic;
using Xango.Data.Query;

namespace Blog.Domain.Queries
{
    public interface ILastPosts : IQuery<IEnumerable<Post>>
    {
    }
}