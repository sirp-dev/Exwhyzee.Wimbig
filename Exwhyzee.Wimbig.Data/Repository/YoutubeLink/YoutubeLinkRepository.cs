using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.PayOutDetails;
using Exwhyzee.Wimbig.Core.YoutubeLink;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.YoutubeLink
{
    public class YoutubeLinkRepository : IYoutubeLinkRepository
    {
        #region Const

        private const string CACHE_YOUTUBE = "exwhyzee.wimbig.YoutubeLink";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields
    
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public YoutubeLinkRepository(
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

        public async Task<long> Add(YoutubeLinks entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spYoutubeLinkInsert @url,@title,@dateCreated";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {

                   
                            url = entity.Url,
                            title = entity.Title,
                            dateCreated = entity.DateCreated
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_YOUTUBE);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        

        public async Task<PagedList<YoutubeLinks>>  GetAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_YOUTUBE}.getyoutube.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var statistics = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_YOUTUBE));
                return _storage.UseConnection(conn =>
                {                   
                        string query = $"dbo.spYoutubeLinkGetAll @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<YoutubeLinks>(query, new
                        {
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
            var paggedResult = new PagedList<YoutubeLinks>(source: statistics,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);         
        }

       

        public async Task<YoutubeLinks> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_YOUTUBE}.getyoutubebyid:{id}";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_YOUTUBE));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spYoutubeLinkGetById @id";
                    return conn.QueryFirstOrDefault<YoutubeLinks>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }

        public async Task Update(YoutubeLinks entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity =_storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spPayOutDetailsUpdate @id,@url,@dateCreated";

                    conn.Execute(sql,
                        new
                        {
                            entity.Id,
                            entity.Url,
                            entity.Title,
                            entity.DateCreated
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_YOUTUBE);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }


        public async Task Delete(long Id)
        {
            if (Id < 1)
                throw new ArgumentOutOfRangeException(nameof(Id));

            var model = _storage.UseConnection(conn =>
            {
                string sql = "spYoutubeLinkDelete @Id";
                conn.Execute(sql, new { Id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                return true;
            });

            _signal.SignalToken(CACHE_YOUTUBE);
            await Task.CompletedTask;
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
