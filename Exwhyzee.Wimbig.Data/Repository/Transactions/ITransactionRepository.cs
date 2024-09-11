using Exwhyzee.Wimbig.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Transactions
{
    public interface ITransactionRepository
    {
        Task<PagedList<Transaction>> GetAllTransactions(string userId = null,long? walletId = null, int? status = null,
            DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue,string searchString = null);

        Task<PagedList<Transaction>> GetAllTransactionsByReferenceId(string userId = null, long? walletId = null,
       int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0,
       int count = int.MaxValue, string searchString = null);
        Task<Transaction> InsertTransaction(Transaction transaction);

        Task<Transaction> UpdateTransaction(Transaction transaction);

        Task<Transaction> GetTransaction(long transactionId);
    }
}
