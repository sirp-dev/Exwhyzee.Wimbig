using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Core.WimBank;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Wimbank
{
    public class WimbankRepository : IWimbankRepository
    {
        #region Const

        private const string CACHE_WIMBANK = "exwhyzee.wimbig.wimbank";
        private const int CACHE_EXPIRATION_MINUTES = 30;
        private const string CACHE_TRANSACTION = "exwhyzee.wimbig.transactions";
        private const string CACHE_WALLETS = "exwhyzee.wimbig.wallets";
        private const string CACHE_WIMBANKTRANSFER = "exwhyzee.wimbig.wimbanktransfer";

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;

        #endregion

        #region Ctor
        public WimbankRepository(
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
        public async Task<PagedList<Core.WimBank.WimBank>> GetAllWimbank(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                string cacheKey = $"{CACHE_WIMBANK}.getAllWimbank.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
                var categories = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WIMBANK));
                    return _storage.UseConnection(conn =>
                    {
                        string query = $"dbo.spWimbankGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<WimBank>(query, new
                        {
                            status = status,
                            dateStart = dateStart,
                            dateEnd = dateEnd,
                            startIndex = startIndex,
                            count = count,
                            searchString = searchString,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                        return result;
                    });
                });

                var filterCount = categories.AsList().Count;
                var paggedResult = new PagedList<WimBank>(source: categories,
                                    pageIndex: startIndex,
                                    pageSize: count,
                                    filteredCount: filterCount,
                                    totalCount: filterCount);

                return await Task.FromResult(paggedResult);

                
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<Core.WimBank.WimBank> GetWimbank(long wimbankId)
        {
            if (wimbankId < 1)
                throw new ArgumentNullException(nameof(wimbankId));

            string cacheKey = $"{CACHE_WIMBANK}.getWimbank:{wimbankId}";
            var wimbank = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WIMBANK));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spWimbankGetById @wimbankId";
                    return conn.QueryFirstOrDefault<Core.WimBank.WimBank>(sql,
                        new { wimbankId },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });
            return await Task.FromResult(wimbank);

        }

        public async Task<Core.WimBank.WimBank>  GetWimbankLastRecord()
        {
            

          
                var store = _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spWimbankLastBalance";
                    return conn.QueryFirstOrDefault<Core.WimBank.WimBank>(sql,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
          
            return await Task.FromResult(store);
        }

        public async Task<Core.WimBank.WimBank> InsertWimbank(Core.WimBank.WimBank wimbank)
        {
            try
            {
                if (wimbank == null)
                    throw new ArgumentNullException(nameof(wimbank));

                wimbank = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spWimbankInsert @amount,@userId,@balance,@dateOfTransaction,@note,@transactionStatus,@receiverId";

                    wimbank.Id = conn.ExecuteScalar<long>(sql,
                    new
                    {
                        amount = wimbank.Amount,
                       userId = wimbank.UserId,
                       balance = wimbank.Balance,
                       dateOfTransaction = wimbank.DateOfTransaction,
                     
                        note = wimbank.Note,
                        transactionStatus = wimbank.TransactionStatus,
                        receiverId = wimbank.ReceiverId


                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return wimbank;
                });

                _signal.SignalToken(CACHE_WIMBANK);
                return await Task.FromResult(wimbank);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Core.WimBank.WimBank> UpdateWimbank(Core.WimBank.WimBank wimbank)
        {
            try
            {
                if (wimbank == null)
                    throw new ArgumentNullException(nameof(wimbank));

                wimbank = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spWimbankUpdate @id,@amount,@userId,@balance,@dateOfTransaction,@transactionStatus,@note";

                    conn.Execute(sql,
                        new
                        {
                            id = wimbank.Id,
                            amount = wimbank.Amount,
                            userId = wimbank.UserId,
                            balance = wimbank.Balance,
                            dateOfTransaction = wimbank.DateOfTransaction,
                            transactionStatus = wimbank.TransactionStatus,
                            note = wimbank.Note
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return wimbank;
                });

                _signal.SignalToken(CACHE_WIMBANK);
                return await Task.FromResult(wimbank);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion



        #region Method Tansfer
        public async Task<PagedList<Core.WimBank.WimbankTransfer>> GetAllWimbankTransfer(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                string cacheKey = $"{CACHE_WIMBANK}.getall.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
                var wimbank = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WIMBANK));
                    return _storage.UseConnection(conn =>
                    {
                        string query = $"dbo.spWimbankReadAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<WimbankTransfer>(query, new
                        {
                            status = status,
                            dateStart = dateStart,
                            dateEnd = dateEnd,
                            startIndex = startIndex,
                            count = count,
                            searchString = searchString,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                        return result;
                    });
                });

                var filterCount = wimbank.AsList().Count;
                var paggedResult = new PagedList<WimbankTransfer>(source: wimbank,
                                    pageIndex: startIndex,
                                    pageSize: count,
                                    filteredCount: filterCount,
                                    totalCount: filterCount);

                return await Task.FromResult(paggedResult);


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Core.WimBank.WimbankTransfer> GetWimbankTransfer(long transferWimbankId)
        {
            if (transferWimbankId < 1)
                throw new ArgumentNullException(nameof(transferWimbankId));

            string cacheKey = $"{CACHE_WIMBANK}.getbyid:{transferWimbankId}";
            var wimbank = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WIMBANK));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spWimbankGetById @transferWimbankId";
                    return conn.QueryFirstOrDefault<Core.WimBank.WimbankTransfer>(sql,
                        new { transferWimbankId },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });
            return await Task.FromResult(wimbank);

        }

        public async Task<Core.WimBank.WimbankTransfer> InsertWimbankTransfer(WimbankTransfer wimtransfer, WimBank wimBank, Wallet wallet, Transaction transaction)
        {
            try
            {
                if (wimtransfer == null)
                    throw new ArgumentNullException(nameof(wimtransfer));

               _storage.UseConnection(conn =>
                {
                    IDbTransaction dbTransaction = conn.BeginTransaction();


                    var transferInsert = $"dbo.spTransferWimbankInsert @amount,@userId,@receiverId,@dateOfTransaction,@transactionStatus,@note,@receiverPhone";
                    var bankInsert = $"dbo.spWimbankInsert @amount,@userId,@balance,@dateOfTransaction,@note,@transactionStatus,@receiverId";
                    var sql = $"dbo.spTransactionInsert @walletId,@userId,@amount,@transactionType,@dateOfTransaction,@status,@sender,@description";
                    var walletsql = $"dbo.spWalletUpdate @id,@userId,@balance,@dateUpdated";

                 
                    conn.Execute(transferInsert,
                        new
                        {
                            amount = wimtransfer.Amount,
                            userId = wimtransfer.UserId,
                            receiverId = wimtransfer.ReceiverId,
                            dateOfTransaction = DateTime.UtcNow.AddHours(1),
                            transactionStatus = wimtransfer.TransactionStatus,
                            note = wimtransfer.Note,
                            receiverPhone = wimtransfer.ReceiverPhone
                            
                        }, transaction: dbTransaction,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);


                    conn.Execute(bankInsert,
                        new
                        {
                            amount = wimBank.Amount,
                            userId = wimBank.UserId,
                            balance = wimBank.Balance,
                            dateOfTransaction = wimBank.DateOfTransaction,note = wimBank.Note,
                            transactionStatus = wimBank.TransactionStatus,
                            receiverId = wimBank.ReceiverId

                        }, transaction: dbTransaction,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    conn.Execute(walletsql,
                       new
                       {
                           id = wallet.Id,
                           userId = wallet.UserId,
                           balance = wallet.Balance,
                           dateUpdated = wallet.DateUpdated
                       }, transaction: dbTransaction,
                       commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    conn.Execute(sql,
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

                       }, transaction: dbTransaction,
                       commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);





                    dbTransaction.Commit();
                });
            ;
        _signal.SignalToken(CACHE_WIMBANK);
                _signal.SignalToken(CACHE_WALLETS);
                _signal.SignalToken(CACHE_TRANSACTION);
                _signal.SignalToken(CACHE_WIMBANKTRANSFER);
                return await Task.FromResult(wimtransfer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Core.WimBank.WimbankTransfer> UpdateWimbankTransfer(Core.WimBank.WimbankTransfer wimbank)
        {
            try
            {
                if (wimbank == null)
                    throw new ArgumentNullException(nameof(wimbank));

                wimbank = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spTransferWimbankUpdate @id,@amount,@userId,@receiverId,@dateOfTransaction,@transactionStatus,@note,@receiverPhone";

                    conn.Execute(sql,
                        new
                        {
                            id = wimbank.Id,
                            amount = wimbank.Amount,
                            userId = wimbank.UserId,
                            receiver = wimbank.ReceiverId,
                            dateOfTransaction = wimbank.DateOfTransaction,
                            transactionStatus = wimbank.TransactionStatus,
                            note = wimbank.Note,
                            receiverPhone = wimbank.ReceiverPhone
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return wimbank;
                });

                _signal.SignalToken(CACHE_WIMBANK);
                return await Task.FromResult(wimbank);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        #endregion

        public async Task<Transaction> WimbankUpdateUser(Wallet wallet, Transaction transaction)
        {
            try
            {
              
                _storage.UseConnection(conn =>
                {
                    IDbTransaction dbTransaction = conn.BeginTransaction();

                    var walletSql = $"dbo.spWalletUpdate @id,@userId,@balance,@dateUpdated";
                    var transactionSql = $"dbo.spTransactionInsert @walletId,@userId,@amount,@transactionType,@dateOfTransaction,@sender";



                    transaction.Id = conn.ExecuteScalar<long>(transactionSql,
                       new
                       {
                           walletId = transaction.WalletId,
                           userId = transaction.UserId,
                           amount = transaction.Amount,
                           transactionType = transaction.TransactionType,
                           dateOfTransaction = transaction.DateOfTransaction,
                           transactionReference = transaction.TransactionReference,
                           sender = transaction.Sender
                       }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                  

                    conn.Execute(walletSql,
                        new
                        {
                            id = wallet.Id,
                            userId = wallet.UserId,
                            balance = wallet.Balance,
                            dateUpdated = wallet.DateUpdated
                          
                        }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);



                    dbTransaction.Commit();
                });
                
                _signal.SignalToken(CACHE_TRANSACTION);
                _signal.SignalToken(CACHE_WALLETS);
                return await Task.FromResult(transaction);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task<Transaction> WimpayUpdate(Transaction transactionDataSender, Transaction transactionDataReceiver, Wallet walletDataSender, Wallet walletDataReceiver)
        {
            try
            {

                _storage.UseConnection(conn =>
                {
                    IDbTransaction dbTransaction = conn.BeginTransaction();

                    var walletSql = $"dbo.spWalletUpdate @id,@userId,@balance,@dateUpdated";
                    var transactionSql = $"dbo.spTransactionInsert @walletId,@userId,@amount,@transactionType,@dateOfTransaction,@status,@sender,@description";



                    transactionDataSender.Id = conn.ExecuteScalar<long>(transactionSql,
                       new
                       {
                           walletId = transactionDataSender.WalletId,
                           userId = transactionDataSender.UserId,
                           amount = transactionDataSender.Amount,
                           transactionType = transactionDataSender.TransactionType,
                           dateOfTransaction = transactionDataSender.DateOfTransaction,
                           status = transactionDataSender.Status,
                           sender = "Wimpay",
                           description = "Wimpay"
                       }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    transactionDataReceiver.Id = conn.ExecuteScalar<long>(transactionSql,
                     new
                     {
                         walletId = transactionDataReceiver.WalletId,
                         userId = transactionDataReceiver.UserId,
                         amount = transactionDataReceiver.Amount,
                         transactionType = transactionDataReceiver.TransactionType,
                         dateOfTransaction = transactionDataReceiver.DateOfTransaction,
                         status = transactionDataReceiver.Status,
                         sender = transactionDataReceiver.Sender,
                         description = "Wimpay"
                     }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);



                    conn.Execute(walletSql,
                        new
                        {
                            id = walletDataSender.Id,
                            userId = walletDataSender.UserId,
                            balance = walletDataSender.Balance,
                            dateUpdated = walletDataSender.DateUpdated

                        }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    conn.Execute(walletSql,
                      new
                      {
                          id = walletDataReceiver.Id,
                          userId = walletDataReceiver.UserId,
                          balance = walletDataReceiver.Balance,
                          dateUpdated = walletDataReceiver.DateUpdated

                      }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);



                    dbTransaction.Commit();
                });

                _signal.SignalToken(CACHE_TRANSACTION);
                _signal.SignalToken(CACHE_WALLETS);
                return await Task.FromResult(transactionDataSender);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }
    }
}
