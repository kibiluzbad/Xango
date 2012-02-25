using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Cfg;
using NUnit.Framework;
using Xango.Data.NHibernate.Queries;
using Xango.Tests.Model;

namespace Xango.Tests.Queries
{
    [TestFixture, Ignore]
    public class QueryTests : NHibernateFixture
    {
        private IEnumerable<Type> GetNamedQueryObjectTypes()
        {
            Type namedQueryType = typeof (INamedQuery);
            Assembly queryImplAssembly = typeof (ProductWithValue).Assembly;
            IEnumerable<Type> types = from t in queryImplAssembly.GetTypes()
                                      where namedQueryType.IsAssignableFrom(t)
                                            && t.IsClass
                                            && !t.IsAbstract
                                      select t;
            return types;
        }

        private IEnumerable<string> GetNamedQueryNames()
        {
            Configuration nhCfg = NHConfigurator.Configuration;
            IEnumerable<string> mappedQueries = nhCfg.NamedQueries.Keys
                .Union(nhCfg.NamedSQLQueries.Keys);
            return mappedQueries;
        }

        private INamedQuery GetQuery(Type queryType)
        {
            return (INamedQuery) Activator
                                     .CreateInstance(queryType,
                                                     new object[] {SessionFactory});
        }

        [Test]
        public void NamedQueryCheck()
        {
            var errors = new StringBuilder();
            IEnumerable<Type> queryObjectTypes = GetNamedQueryObjectTypes();
            IEnumerable<string> mappedQueries = GetNamedQueryNames();
            foreach (Type queryType in queryObjectTypes)
            {
                INamedQuery query = GetQuery(queryType);
                if (!mappedQueries.Contains(query.QueryName))
                {
                    errors.AppendFormat(
                        "Query object {0} references non-existent " +
                        "named query {1}.",
                        queryType, query.QueryName);
                    errors.AppendLine();
                }
            }
            if (errors.Length != 0)
                Assert.Fail(errors.ToString());
        }
    }
}