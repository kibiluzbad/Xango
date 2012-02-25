using Xango.Data;

namespace Blog.Domain
{
    public class Author : StampedEntity
    {
        public virtual string Name { get; set; }
    }
}