using Microsoft.Practices.ServiceLocation;
using Xango.Data.Query;

namespace Xango.Data.NHibernate.Queries
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public QueryFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        #region IQueryFactory Members

        public TQuery CreateQuery<TQuery>() where TQuery : IQuery
        {
            return _serviceLocator.GetInstance<TQuery>();
        }

        #endregion
    }
}