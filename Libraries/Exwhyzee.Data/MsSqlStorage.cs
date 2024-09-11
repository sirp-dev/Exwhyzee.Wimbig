using EXWHYZEE.Data;
using System;
using System.Data;

namespace Exwhyzee.Data
{
    public class MsSqlStorage : IStorage
    {
        #region Fields

        private readonly IDatabaseProviderFactory _databaseProviderFactory;

        #endregion

        #region Ctor

        public MsSqlStorage(IDatabaseProviderFactory databaseProviderFactory)
        {
            _databaseProviderFactory = databaseProviderFactory;
        }
        #endregion

        #region Utils

        //private TransactionScope CreateTransaction(IsolationLevel? isolationLevel)
        //{
        //    return isolationLevel != null
        //        ? new TransactionScope(TransactionScopeOption.Required,
        //            new TransactionOptions
        //            {
        //                IsolationLevel = isolationLevel.Value,
        //                Timeout = TimeSpan.FromSeconds(DataConstants.TRANSACTION_TIMEOUT_SECONDS)
        //            })
        //        : new TransactionScope();
        //}

        #endregion

        #region Methods

        
        public void UseConnection(Action<IDbConnection> action)
        {
            UseConnection(connection =>
            {
                action(connection);
                return true;
            });
        }

        public T UseConnection<T>(Func<IDbConnection, T> func)
        {
            IDbConnection connection = null;

            try
            {
                connection = CreateAndOpenConnection();
                return func(connection);
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        public IDbConnection CreateAndOpenConnection()
        {
            var connection = _databaseProviderFactory.CreateConnection();
            connection.Open();

            return connection;
        }
        public void ReleaseConnection(IDbConnection connection)
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }

        public IDisposable AcquireDistributedLock(string resource, TimeSpan timeout)
        {
            return new SqlServerDistributedLock(this, $"{this.GetType().FullName}:{resource}", timeout);
        }

        #endregion

    }
}
