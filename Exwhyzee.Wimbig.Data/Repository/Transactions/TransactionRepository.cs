using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.Transactions;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Transactions
{
    public class TransactionRepository : ITransactionRepository
    {
        #region Const

        private const string CACHE_TRANSACTION = "exwhyzee.wimbig.transactions";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;

        #endregion

        #region Ctor
        public TransactionRepository(
            IStorage storage,
            IMemoryCache memoryCache,
            ISignal signal,
            IClock clock)
        {
            _storage = storage;
            _memoryCache = memoryCache;
            _signal = signal;
            _clock = clock;
        }

        #endregion

        #region Method
        public async Task<PagedList<Transaction>> GetAllTransactions(string userId = null, long? walletId = null, 
            int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, 
            int count = int.MaxValue, string searchString = null)
        {
            try
            {
                string cacheKey = $"{CACHE_TRANSACTION}.getAllTransactionss.{userId}.{walletId}.{status}.{startIndex}.{count}.{searchString}";
                var transactions = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTransactionsReadAll @userId,@walletId,@status,@dateStart,@dateEnd, @startIndex, @count, @searchString";
                        var parameters = new
                        {
                            userId,
                            walletId,
                            status,
                            dateStart,
                            dateEnd,
                            startIndex,
                            count,
                            searchString
                        };

                        using (var multi = conn.QueryMultiple(sql, parameters,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Transaction>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return new PagedList<Transaction>(source: query,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: summary.TotalCount,
                                totalCount: summary.FilteredCount);
                        }
                    });
                });

                return await Task.FromResult(transactions);
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// get by reference id

        public async Task<PagedList<Transaction>> GetAllTransactionsByReferenceId(string userId = null, long? walletId = null,
       int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0,
       int count = int.MaxValue, string searchString = null)
        {
            try
            {
                string cacheKey = $"{CACHE_TRANSACTION}.getAllTransactionsByReferenceId.{userId}.{walletId}.{status}.{startIndex}.{count}.{searchString}";
                var transactions = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTransactionsGetAllByReferenceId @userId,@walletId,@status,@dateStart,@dateEnd, @startIndex, @count, @searchString";
                        var parameters = new
                        {
                            userId,
                            walletId,
                            status,
                            dateStart,
                            dateEnd,
                            startIndex,
                            count,
                            searchString
                        };

                        using (var multi = conn.QueryMultiple(sql, parameters,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Transaction>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return new PagedList<Transaction>(source: query,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: summary.TotalCount,
                                totalCount: summary.FilteredCount);
                        }
                    });
                });

                return await Task.FromResult(transactions);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <returns></returns>
        public async Task<Transaction> GetTransaction(long transactionId)
        {
            if (transactionId < 1)
                throw new ArgumentNullException(nameof(transactionId));

            string cacheKey = $"{CACHE_TRANSACTION}.getTransaction:{transactionId}";
            var transaction = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spTransactionGetById @transactionId";
                    return conn.QueryFirstOrDefault<Transaction>(sql,
                        new { transactionId },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });
            return await Task.FromResult(transaction);

        }

        public async Task<Transaction> InsertTransaction(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                    throw new ArgumentNullException(nameof(transaction));

                transaction = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spTransactionInsert @walletId,@userId,@amount,@transactionType,@dateOfTransaction,@status,@sender,@description";

                transaction.Id = conn.ExecuteScalar<long>(sql,
                    new
                    {
                        walletId = transaction.WalletId,
                        userId = transaction.UserId,
                        amount = transaction.Amount,
                        transactionType = transaction.TransactionType,
                        dateOfTransaction = transaction.DateOfTransaction,
                        status = transaction.Status,
                        sender = transaction.Sender,
                        description = transaction.Description
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return transaction;
                });

                _signal.SignalToken(CACHE_TRANSACTION);
                return await Task.FromResult(transaction);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                    throw new ArgumentNullException(nameof(transaction));

                transaction = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spTransactionUpdate @id,@walletId,@userId,@amount,@transactionType,@dateOfTransaction,@transactionreference,@sender,@status,@description";
                    
                    conn.Execute(sql,
                        new
                        {
                            id = transaction.Id,
                            walletId = transaction.WalletId,
                            userId = transaction.UserId,
                            amount = transaction.Amount,
                            transactionType = transaction.TransactionType,
                            dateOfTransaction = transaction.DateOfTransaction,
                            transactionReference = transaction.TransactionReference,
                            sender = transaction.Sender,
                            status = transaction.Status,
                            description = transaction.Description
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return transaction;
                });

                _signal.SignalToken(CACHE_TRANSACTION);
                return await Task.FromResult(transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}
