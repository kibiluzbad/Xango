using System.Data;
using NHibernate.Connection;

namespace Xango.Tests
{
    public class TestConnectionProvider
        : DriverConnectionProvider
    {
        private static IDbConnection _connection;

        public override IDbConnection GetConnection()
        {
            if (_connection == null)
                _connection = base.GetConnection();
            return _connection;
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
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}