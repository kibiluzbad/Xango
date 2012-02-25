using System;
using System.Collections.Generic;
using System.Data;
using FluentNHibernate.Cfg.Db;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Xango.Data.NHibernate.Configuration
{
    public static class DatabaseFactory
    {
        private static IDictionary<string, IPersistenceConfigurer> _dbMap = new Dictionary
            <string, IPersistenceConfigurer>
                                                                                {
                                                                                    {"mysql",MySQLConfiguration.Standard 
                                                                                        .Dialect<MySQL5Dialect>()
                                                                                        .Driver<MySqlDataDriver>()
                                                                                        .ConnectionString(connstr =>
                                                                                                          connstr.FromConnectionStringWithKey("default"))
                                                                                        .AdoNetBatchSize(100)},
                                                                                    {"sqlite",SQLiteConfiguration.Standard 
                                                                                        .Dialect<SQLiteDialect>()
                                                                                        .Driver<SQLite20Driver>()
                                                                                        .ConnectionString(connstr =>
                                                                                                          connstr.FromConnectionStringWithKey("default"))
                                                                                        .AdoNetBatchSize(100)},
                                                                                    {"sqlserver",MsSqlConfiguration.MsSql2008
                                                                                        .Dialect<MsSql2008Dialect>()
                                                                                        .Driver<SqlClientDriver>()
                                                                                        .ConnectionString(connstr =>
                                                                                                          connstr.FromConnectionStringWithKey("default"))
                                                                                        .AdoNetBatchSize(100)},
                                                                                    {"oracle",OracleClientConfiguration.Oracle10
                                                                                        .Dialect<Oracle10gDialect>()
                                                                                        .Driver<OracleClientDriver>()
                                                                                        .ConnectionString(connstr =>
                                                                                                          connstr.FromConnectionStringWithKey("default"))
                                                                                        .AdoNetBatchSize(100)},
                                                                                        {"sqlite_in_memory",SQLiteConfiguration.Standard 
                                                                                        .Dialect<SQLiteDialect>()
                                                                                        .Driver<SQLite20Driver>()
                                                                                        .ConnectionString(connstr =>
                                                                                                          connstr.FromConnectionStringWithKey("default"))
                                                                                        .Provider<TestConnectionProvider>()
                                                                                        .AdoNetBatchSize(100)},

                                                                                };

        public static IPersistenceConfigurer Get(string database)
        {
            if (!_dbMap.ContainsKey(database.ToLower().Trim()))
                throw new InvalidOperationException(
                    string.Format("Não foi possível identificar uma configuração de bando de dados para o banco {0}.",database));

            return _dbMap[database];
        }
    }

    internal class TestConnectionProvider
        : DriverConnectionProvider
    {
        private static IDbConnection _connection;

        public override IDbConnection GetConnection()
        {
            return _connection ?? (_connection = base.GetConnection());
        }

        public override void CloseConnection(IDbConnection conn)
        {
        }

        /// <summary>
        /// Destroys the connection that is kept open in order to 
        /// keep the in-memory database alive.  Destroying
        /// the connection will destroy all of the data stored in 
        /// the mock database.  Call this method when the
        /// test is complete.
        /// </summary>
        public static void CloseDatabase()
        {
            if (_connection == null) return;

            _connection.Close();
            _connection = null;
        }
    }
}