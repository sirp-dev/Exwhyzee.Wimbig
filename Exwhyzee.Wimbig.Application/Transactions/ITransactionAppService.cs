using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Transactions
{
    public interface ITransactionAppService
    {
        Task<PagedList<TransactionDto>> GetAllTransactions(string userId = null, long? walletId = null, int? status = null,
           DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PagedList<TransactionDto>> GetAllTransactionsByReferenceId(string userId = null, long? walletId = null, int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<TransactionDto> CreateTransaction(InsertTransactionDto transaction);

        Task<TransactionDto> UpdateTransaction(TransactionDto transaction);

        Task<TransactionDto> GetTransaction(long id);
    }
}
