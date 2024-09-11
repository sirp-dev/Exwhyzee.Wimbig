using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.PayOutDetails;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.PayOutDetails
{
    public class PayOutDetailsRepository : IPayOutDetailsRepository
    {
        #region Const

        private const string CACHE_PAYOUTDETAILS = "exwhyzee.wimbig.paydetails";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields
    
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public PayOutDetailsRepository(
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

        public async Task<long> Add(PayOutDetail entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spPayOutDetailsInsert @percentage,@accountNumber,@accountName,@bankName,@userId";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {

                   
                            percentage = entity.Percentage,
                            accountNumber= entity.AccountNumber,
                            accountName = entity.AccountName,
                            bankName = entity.BankName,
                            userId = entity.UserId,
                            

                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_PAYOUTDETAILS);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        

        public async Task<PagedList<PayOutDetail>>  GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_PAYOUTDETAILS}.getpayoutdetails.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var statistics = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTDETAILS));
                return _storage.UseConnection(conn =>
                {                   
                        string query = $"dbo.spPayOutDetailsGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<PayOutDetail>(query, new
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
            var paggedResult = new PagedList<PayOutDetail>(source: statistics,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);         
        }

       

        public async Task<PayOutDetail> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_PAYOUTDETAILS}.getpayoutdetailsbyid:{id}";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_PAYOUTDETAILS));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spPayOutDetailsGetById @id";
                    return conn.QueryFirstOrDefault<PayOutDetail>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }

        public async Task Update(PayOutDetail entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity =_storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spPayOutDetailsUpdate @id,@percentage,@accountNumber,@accountName,@bankName,@userId";

                    conn.Execute(sql,
                        new
                        {
                            entity.Id,
                            entity.Percentage,
                            entity.AccountNumber,
                            entity.AccountName,
                            entity.BankName,
                            entity.UserId
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_PAYOUTDETAILS);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
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
        #endregion
        #endregion
    }
}
