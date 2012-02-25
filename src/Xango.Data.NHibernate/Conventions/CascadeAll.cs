using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Xango.Data.NHibernate.Conventions
{
    public class CascadeAll : IHasOneConvention, IHasManyConvention, IReferenceConvention
    {
        #region IHasManyConvention Members

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.AllDeleteOrphan();
        }

        #endregion

        #region IHasOneConvention Members

        public void Apply(IOneToOneInstance instance)
        {
            instance.Cascade.All();
        }

        #endregion

        #region IReferenceConvention Members

        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.All();
        }

        #endregion
    }
}