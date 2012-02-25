using Xango.Data;

namespace Blog.Domain
{
    public class Comment : StampedEntity
    {
        protected Comment()
        {
        }

        public Comment(string text, string author, string mail)
        {
            Text = text;
            Author = author;
            Mail = mail;
        }

        public virtual string Text { get; protected set; }
        public virtual string Author { get; protected set; }
        public virtual string Mail { get; protected set; }
    }
}