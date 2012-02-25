using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Xango.Data.NHibernate.Conventions
{
    public class HiLoIndetityStrategy : IIdConvention
    {
        #region IIdConvention Members

        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.HiLo(string.Empty);
        }

        #endregion
    }
}