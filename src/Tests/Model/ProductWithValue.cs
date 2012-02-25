using NHibernate;
using Xango.Data.NHibernate.Queries;

namespace Xango.Tests.Model
{
    public class ProductWithValue : NamedQueryBase<Product>, IProductWithValue
    {
        public ProductWithValue(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region IProductWithValue Members

        public decimal Value { get; set; }

        #endregion

        protected override Product Execute(IQuery query)
        {
            return query.UniqueResult<Product>();
        }

        protected override void SetParameters(IQuery query)
        {
            query.SetParameter("value", Value);
        }
    }
}