using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.DailyStatistics
{
    public class DailyStatisticsRepository : IDailyStatisticsRepository
    {
        #region Const

        private const string CACHE_STATISTIC = "exwhyzee.wimbig.dailyStatistics";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields
    
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public DailyStatisticsRepository(
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

        public async Task<long> Add(DailyStatistic entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spDailyStatisticsInsert @date,@totalUsers,@totalTickets,@totalRaffle,@totalCash,@totalWalletCash";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            date = entity.Date,
                            totalUsers= entity.TotalUsers,
                            totalTickets = entity.TotalTickets,
                            totalRaffle = entity.TotalRaffle,
                            totalCash = entity.TotalCash,
                            totalWalletCash = entity.TotalWalletCash

                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_STATISTIC);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        

        public async Task<PagedList<DailyStatistic>>  GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_STATISTIC}.getstatisticall.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var statistics = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_STATISTIC));
                return _storage.UseConnection(conn =>
                {                   
                        string query = $"dbo.spDailyStatisticsGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<DailyStatistic>(query, new
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
            var paggedResult = new PagedList<DailyStatistic>(source: statistics,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);
            _signal.SignalToken(CACHE_STATISTIC);
            return await Task.FromResult(paggedResult);         
        }

       

        public async Task<DailyStatistic> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_STATISTIC}.getstatbyid:{id}";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_STATISTIC));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spDailyStatisticsGetById @id";
                    return conn.QueryFirstOrDefault<DailyStatistic>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }

        public async Task Update(DailyStatistic entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity =_storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spDailyStatisticsUpdate @id,@date,@totalUsers,@totalTickets,@totalRaffle,@totalCash,@totalWalletCash";

                    conn.Execute(sql,
                        new
                        {
                            entity.Id,
                            entity.Date,
                            entity.TotalUsers,
                            entity.TotalTickets,
                            entity.TotalRaffle,
                            entity.TotalCash,
                            entity.TotalWalletCash
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_STATISTIC);
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
