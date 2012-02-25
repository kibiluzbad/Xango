using System;
using System.Linq.Expressions;
using Blog.Domain;
using LinqSpecs;

namespace Blog.Repository.Queries
{
    public class PostCreatedBy : Specification<Post>
    {
        private readonly string _authorName;

        public PostCreatedBy(string authorName)
        {
            _authorName = authorName;
        }

        public override Expression<Func<Post, bool>> IsSatisfiedBy()
        {
            return c => c.Author == _authorName;
        }
    }
}