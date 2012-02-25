using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Xango.Data.NHibernate.Conventions
{
    public class CustomCollectionConvetion : ICollectionConvention
    {
        #region ICollectionConvention Members

        public void Apply(ICollectionInstance instance)
        {
            instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
            instance.AsSet();
        }

        #endregion
    }
}