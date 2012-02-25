using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace Xango.Data.NHibernate.Conventions
{
    public class CustomForeignKeyConvention
        : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            return property == null
                       ? type.Name + "Id"
                       : property.Name + "Id";
        }
    }
}
