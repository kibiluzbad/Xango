using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Domain;
using NHibernate.Validator.Cfg.Loquacious;

namespace Blog.Repository.Validators
{
    public class CommentValidator
        : ValidationDef<Comment>
    {
        public CommentValidator()
        {
            Define(c => c.Author)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);

            Define(c => c.Mail)
                .IsEmail();

            Define(c => c.Text)
                .NotNullableAndNotEmpty()
                .And.MaxLength(2000);
        }
    }
}
