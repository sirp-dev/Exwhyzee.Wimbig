using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using Exwhyzee.Wimbig.Core.PayOutReports;
using Exwhyzee.Wimbig.Core.Transactions;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.PayOutReports
{
    public class PayOutReportRepository : IPayOutReportRepository
    {
        #region Const

        private const string CACHE_PAYOUTREPORT = "exwhyzee.wimbig.payotreport";
        private const string CACHE_TRANSACTION = "exwhyzee.wimbig.transactions";

        private const string CACHE_WALLETS = "exwhyzee.wimbig.wallets";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public PayOutReportRepository(
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

        #region Methods

        public async Task<long> Add(PayOutReport entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spPayOutReportInsert @date,@amount,@percentageAmount,@percentage,@reference,@status,@note,@startDate,@endDate,@userId";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            date = entity.Date,
                            amount = entity.Amount,
                            percentageAmount = entity.PercentageAmount,
                            percentage = entity.Percentage,
                            reference = entity.Reference,
                            status = entity.Status,
                            note = entity.Note,
                            startDate = entity.StartDate,
                            endDate = entity.EndDate,
                            userId = entity.UserId

                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_PAYOUTREPORT);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }



        public async Task<PagedList<PayOutReport>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_PAYOUTREPORT}.getpayoutreport.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var statistics = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTREPORT));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spPayOutReportGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                    var result = conn.Query<PayOutReport>(query, new
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

            var filterCount = statistics.AsList().Count;
            var paggedResult = new PagedList<PayOutReport>(source: statistics,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
        }



        public async Task<PayOutReport> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_PAYOUTREPORT}.getpayoutreportbyid:{id}";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTREPORT));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spPayOutReportGetById @id";
                    return conn.QueryFirstOrDefault<PayOutReport>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }
        public async Task<PayOutReport> GetByUserIdAndDateRange(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (userId == null || userId == "")
                throw new ArgumentNullException(nameof(userId));

            string cacheKey = $"{CACHE_PAYOUTREPORT}.getpayoutreportbyuseridanddaterange:{userId}";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTREPORT));
                return _storage.UseConnection(conn =>
                {

                    string sql = $"dbo.spPayOutReportGetByUserIdAndDate @userId,@startDate,@endDate";
                    return conn.QueryFirstOrDefault<PayOutReport>(sql,
                        new { userId, startDate, endDate },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }

        public async Task<PayOutReport> GetByDateRange(DateTime? startDate = null, DateTime? endDate = null)
        {

            string cacheKey = $"{CACHE_PAYOUTREPORT}.getpayoutreportbydaterange";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTREPORT));
                return _storage.UseConnection(conn =>
                {

                    string sql = $"dbo.spPayOutReportGetByDate @startDate,@endDate";
                    return conn.QueryFirstOrDefault<PayOutReport>(sql,
                        new { startDate, endDate },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }


        public async Task Update(PayOutReport entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                 {
                     var sql = $"dbo.spPayOutReportUpdate @id,@date,@amount,@percentageAmount,@percentage,@reference,@status,@note,@startDate,@endDate,@userId";

                     conn.Execute(sql,
                         new
                         {
                             entity.Id,
                             entity.Date,
                             entity.Amount,
                             entity.PercentageAmount,
                             entity.Percentage,
                             entity.Reference,
                             entity.Status,
                             entity.Note,
                             entity.StartDate,
                             entity.EndDate,
                             entity.UserId
                         },
                         commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                     return entity;
                 });

                _signal.SignalToken(CACHE_PAYOUTREPORT);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }
        public async Task<PayOutReport> PayOutReportGetLastRecordByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                if (userId == null || userId == "")
                    throw new ArgumentNullException(nameof(userId));

                string cacheKey = $"{CACHE_PAYOUTREPORT}.payOutReportGetLastRecordByUserId:{userId}";
                var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTREPORT));
                    return _storage.UseConnection(conn =>
                    {

                        string sql = $"dbo.spPayOutReportGetLastRecordByUserId @userId,@startDate,@endDate";
                        return conn.QueryFirstOrDefault<PayOutReport>(sql,
                            new { userId, startDate, endDate },
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                            );
                    });
                });
                _signal.SignalToken(CACHE_PAYOUTREPORT);
                return await Task.FromResult(statistic);
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public async Task<PayOutReport> PayOutReportGetLastRecord(DateTime? startDate = null, DateTime? endDate = null)
        {


            string cacheKey = $"{CACHE_PAYOUTREPORT}.payOutReportGetLastRecord:";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTREPORT));
                return _storage.UseConnection(conn =>
                {

                    string sql = $"dbo.spPayOutReportGetLastRecord @startDate,@endDate";
                    return conn.QueryFirstOrDefault<PayOutReport>(sql,
                        new { startDate, endDate },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });
            _signal.SignalToken(CACHE_PAYOUTREPORT);
            return await Task.FromResult(statistic);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task UpdatePayout(PayOutReport data, Wallet walletData, Transaction transactionData)
        {
            try
            {
               
                _storage.UseConnection(conn =>
                {
                    IDbTransaction dbTransaction = conn.BeginTransaction();

                   var walletSql = $"dbo.spWalletUpdate @id,@userId,@balance,@dateUpdated";
                    var payoutsql = $"dbo.spPayOutReportUpdate @id,@date,@amount,@percentageAmount,@percentage,@reference,@status,@note,@startDate,@endDate,@userId";
                    var transactionSql = $"dbo.spTransactionInsert @walletId,@userId,@amount,@transactionType,@dateOfTransaction,@status,@sender,@description";



                    conn.Execute(payoutsql,
                        new
                        {
                            data.Id,
                            data.Date,
                            data.Amount,
                            data.PercentageAmount,
                            data.Percentage,
                            data.Reference,
                            data.Status,
                            data.Note,
                            data.StartDate,
                            data.EndDate,
                            data.UserId
                        }, transaction: dbTransaction,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                    
                    conn.Execute(walletSql,
                       new
                       {
                           id = walletData.Id,
                           userId = walletData.UserId,
                           balance = walletData.Balance,
                           dateUpdated = DateTime.UtcNow.AddHours(1),
                           walletData
                       }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    transactionData.Id = conn.ExecuteScalar<long>(transactionSql,
                      new
                      {
                          walletId = transactionData.WalletId,
                          userId = transactionData.UserId,
                          amount = transactionData.Amount,
                          transactionType = transactionData.TransactionType,
                          dateOfTransaction = DateTime.UtcNow.AddHours(1),
                          transactionReference = transactionData.TransactionReference,
                          status = transactionData.Status,
                          sender = transactionData.Sender,
                          description = transactionData.Description
                      }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                    dbTransaction.Commit();
                   
                });

                _signal.SignalToken(CACHE_WALLETS);
                _signal.SignalToken(CACHE_PAYOUTREPORT);

                _signal.SignalToken(CACHE_TRANSACTION);
                await Task.FromResult(data);

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }

        }



        #endregion
        #endregion
    }
}
