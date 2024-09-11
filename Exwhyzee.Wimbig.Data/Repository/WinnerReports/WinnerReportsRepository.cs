using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using Exwhyzee.Wimbig.Core.WinnerReports;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.WinnerReports
{
    public class WinnerReportsRepository : IWinnerReportsRepository
    {
        #region Const

        private const string CACHE_WINNERREPORT = "exwhyzee.wimbig.winnerReport";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields
    
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public WinnerReportsRepository(
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

        public async Task<long> Add(WinnerReport entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spWinnerReportInsert @winnerName,@winnerPhoneNumber,@winnerEmail,@winnerLocation,@amountPlayed,@raffleName,@raffleId,@ticketNumber,@itemCost,@dateCreated,"+
	  $"@dateWon,@dateDelivered,@deliveredBy,@deliveredPhone,@deliveryAddress,@totalAmountPlayed,@userId,@status";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            winnerName = entity.WinnerName,
                            winnerPhoneNumber = entity.WinnerPhoneNumber,
                            winnerEmail = entity.WinnerEmail,
                            winnerLocation = entity.WinnerLocation,
                            amountPlayed = entity.AmountPlayed,
                            raffleName = entity.RaffleName,
                            raffleId = entity.RaffleId,
                            ticketNumber = entity.TicketNumber,
                            itemCost = entity.ItemCost,
                            dateCreated = entity.DateCreated,
                            dateWon = entity.DateWon,
                            dateDelivered = entity.DateDelivered,
                            deliveredBy = entity.DeliveredBy,
                            deliveredPhone = entity.DeliveredPhone,
                            deliveryAddress = entity.DeliveryAddress,
                            totalAmountPlayed = entity.TotalAmountPlayed,
                            UserId = entity.UserId,
                            Status = entity.Status



                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_WINNERREPORT);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        

        public async Task<PagedList<WinnerReport>>  GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_WINNERREPORT}.getwinnerreportall.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var statistics = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WINNERREPORT));
                return _storage.UseConnection(conn =>
                {                   
                        string query = $"dbo.spWinnerReportGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<WinnerReport>(query, new
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
            var paggedResult = new PagedList<WinnerReport>(source: statistics,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);         
        }

       

        public async Task<WinnerReport> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_WINNERREPORT}.getwinnerreportbyid:{id}";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WINNERREPORT));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spWinnerReportGetById @id";
                    return conn.QueryFirstOrDefault<WinnerReport>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }

        public async Task Update(WinnerReport entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity =_storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spWinnerReportUpdate @id,@winnerName,@winnerPhoneNumber,@winnerEmail,@winnerLocation,@amountPlayed,@raffleName,@raffleId,@ticketNumber,@itemCost,@dateCreated," +
      $"@dateWon,@dateDelivered,@deliveredBy,@deliveredPhone,@deliveryAddress,@totalAmountPlayed,@userId,@status";
                    conn.Execute(sql,
                        new
                        {
                            entity.Id,
                             entity.WinnerName,
                             entity.WinnerPhoneNumber,
                             entity.WinnerEmail,
                             entity.WinnerLocation,
                             entity.AmountPlayed,
                             entity.RaffleName,
                             entity.RaffleId,
                             entity.TicketNumber,
                             entity.ItemCost,
                             entity.DateCreated,
                             entity.DateWon,
                             entity.DateDelivered,
                             entity.DeliveredBy,
                             entity.DeliveredPhone,
                             entity.DeliveryAddress,
                             entity.TotalAmountPlayed,
                             entity.UserId,
                             entity.Status
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_WINNERREPORT);
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
