using System.Collections.Generic;
using Xango.Data;

namespace Blog.Domain
{
    public class Post : StampedEntity
    {
        private readonly ICollection<Comment> _comments;

        public Post()
        {
            _comments = new HashSet<Comment>();
        }

        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual string Slug { get; set; }
        public virtual string Author { get; set; }

        public virtual IEnumerable<Comment> Comments
        {
            get { return _comments; }
        }

        public virtual void Comment(string text, string author, string mail)
        {
            _comments.Add(new Comment(text, author, mail));
        }
    }
}