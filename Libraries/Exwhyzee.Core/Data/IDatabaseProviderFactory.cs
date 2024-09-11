using System.Data;

namespace Exwhyzee.Data
{
    public interface IDatabaseProviderFactory
    {
        IDbConnection CreateConnection();
    }
}
