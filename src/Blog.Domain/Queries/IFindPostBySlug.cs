using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xango.Data.Query;

namespace Blog.Domain.Queries
{
    public interface IFindPostBySlug : IQuery<Post>
    {
        string Slug { get; set; }
    }
}
