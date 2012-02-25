using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Xango.Common.String;

namespace Xango.Data.NHibernate.Conventions
{
    public class TableNamePluralizeConvention : IClassConvention
    {
        #region IClassConvention Members

        public void Apply(IClassInstance instance)
        {
            instance.Table(Inflector.Pluralize(instance.EntityType.Name));
        }

        #endregion
    }
}