using IsolationLevel = System.Transactions.IsolationLevel;

namespace Exwhyzee.Data
{
    public static class DataConstants
    {
        public const int TRANSACTION_TIMEOUT_SECONDS = 60;
        public const int COMMAND_TIMEOUT_SECONDS = 25;
        public const IsolationLevel DEFAULT_ISOLATION_LEVEL = IsolationLevel.ReadUncommitted;
    }
}
