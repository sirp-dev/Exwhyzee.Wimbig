using EXWHYZEE.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Exwhyzee.Data
{
    public class MsSqlDatabaseProviderFactory : IDatabaseProviderFactory
    {
        private readonly IConnectionStringFactory _connectionStringFactory;

        private readonly DbProviderFactory _databaseProvider;

        public MsSqlDatabaseProviderFactory(IConnectionStringFactory connectionStringFactory)
        {
            _connectionStringFactory = connectionStringFactory;
            _databaseProvider = SqlClientFactory.Instance;
        }

        public IDbConnection CreateConnection()
        {
            IDbConnection connection = _databaseProvider.CreateConnection();
            if (connection != null)
            {
                connection.ConnectionString = _connectionStringFactory.GetConnectionString();
                return connection;
            }

            return null;
        }
    }
}
