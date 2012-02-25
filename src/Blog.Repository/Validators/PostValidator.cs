using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Domain;
using NHibernate.Validator.Cfg.Loquacious;

namespace Blog.Repository.Validators
{
    public class PostValidator 
        : ValidationDef<Post>
    {
        public PostValidator()
        {
            Define(c => c.Title)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);

            Define(c => c.Slug)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255)
                .And.MatchWith("[a-z0-9\\-]+");

            Define(c => c.Body)
                .NotNullableAndNotEmpty()
                .And.MaxLength(8000);

            Define(c => c.Author)
                .NotNullable();

            Define(c => c.Comments)
                .HasValidElements();
        }
    }
}
