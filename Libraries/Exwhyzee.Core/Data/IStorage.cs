using System;
using System.Data;

namespace Exwhyzee.Data
{
    public interface IStorage
    {
        void UseConnection(Action<IDbConnection> action);
        T UseConnection<T>(Func<IDbConnection, T> func);
        IDbConnection CreateAndOpenConnection();
        void ReleaseConnection(IDbConnection connection);

        IDisposable AcquireDistributedLock(string resource, TimeSpan timeout);
    }
}
