using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Domain;
using NHibernate.Validator.Cfg.Loquacious;

namespace Blog.Repository.Validators
{
    public class AuthorValidator
        : ValidationDef<Author>
    {
        public AuthorValidator()
        {
            Define(c => c.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);
        }
    }
}
