using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.ImageFiles.RaffleImage
{
    public class MapImageToRaffleRepo : IMapImageToRaffleRepo
    {
        #region constants
        private const string CACHE_MAPIMAGETORAFFLE = "Exwhyzee.Wimbig.Data.MapImageToRaffle";
        private const int CACHE_EXPIRATION_MINUTE = 30;
        #endregion

        #region Fields
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        public MapImageToRaffleRepo(IStorage storage,IMemoryCache memoryCache,ISignal signal,IClock clock,IHttpContextAccessor contextAccessor)
        {
            _storage = storage;
            _memoryCache = memoryCache;
            _signal = signal;
            _clock = clock;
            _httpContextAccessor = contextAccessor;
        }

        #region  Insert Update Delete
        public async Task<long> InsertMap(MapImageToRaffle model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model = _storage.UseConnection(conn =>
            {
                string sql = $"[dbo].[spMapImageToRaffleInsert] @imageId,@raffleId,@datecreated";
                model.Id = conn.ExecuteScalar<long>(sql, new
                {
                    model.ImageId,
                    model.RaffleId,
                    model.DateCreated,
                });
                return model;
            });

            _signal.SignalToken(CACHE_MAPIMAGETORAFFLE);
           return await Task.FromResult(model.Id);
        }

        public async Task Update(MapImageToRaffle model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model = _storage.UseConnection(conn =>
            {
                string sql = $"spMapImageToRaffleUpdate @id,@imageId,@raffleId,@dateCreated";
                conn.Execute(sql, new
                {
                    model.Id,
                    model.ImageId,
                    model.RaffleId,
                    model.DateCreated
                }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                return model;
            });
            _signal.SignalToken(CACHE_MAPIMAGETORAFFLE);
            await Task.CompletedTask;
        }

        public async Task Delete(long id)
        {
            if (id < 1 || id > long.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(id));

            var delete = _storage.UseConnection(conn =>
            {
                string sql = $"[dbo].[spMapImageToRaffleDelete] @id";
                return conn.Execute(sql, new
                {id}, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);                
            });
            _signal.SignalToken(CACHE_MAPIMAGETORAFFLE);
            await Task.CompletedTask;
        }
        #endregion

        #region Get Methods
        //Note to get the default image of the raffle if the count parameter is 1 or null
        public async Task<IEnumerable<ImageOfARaffle>> GetAllImagesOfARaffle(long raffleId,int? count = null)
        {
            if (raffleId < 1 || raffleId > long.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(raffleId));

            string cacheKey = $"{CACHE_MAPIMAGETORAFFLE}.getAllImagesToARaffle.{raffleId}.{count}";
            var cachedResult = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AddExpirationToken(_signal.GetToken(cacheKey));
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);

                var getImages = _storage.UseConnection(conn =>
                {
                    string sql = $"spMapImageToRaffleGetImagesOfARaffle @raffleId, @count";
                    return  conn.Query<Core.RaffleImages.ImageOfARaffle>(sql, new { raffleId, count },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                   
                });

                return getImages;
            });
            _signal.SignalToken(CACHE_MAPIMAGETORAFFLE);
            return await Task.FromResult(cachedResult);
        }

        public async Task<MapImageToRaffle> GetById(long id)
        {
            string cacheKey = $"{CACHE_MAPIMAGETORAFFLE}.getById.{id}";
            var cachedResult = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AddExpirationToken(_signal.GetToken(CACHE_MAPIMAGETORAFFLE));
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);

                return _storage.UseConnection(conn =>
                {
                    string sql = $"spMapImageToRaffleGetById @id";
                    return conn.QueryFirstOrDefault<MapImageToRaffle>(sql, new { id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                });
            });
            return await Task.FromResult(cachedResult);
        }

        public async Task<MapImageToRaffle> GetByRaffleId(long id)
        {
            try
            {
                string cacheKey = $"{CACHE_MAPIMAGETORAFFLE}.getByRaffleId.{id}";
                var cachedResult = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AddExpirationToken(_signal.GetToken(CACHE_MAPIMAGETORAFFLE));
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);

                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"spMapImageToRaffleGetByRaffleId @id";
                        return conn.QueryFirstOrDefault(sql, new { id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                    });
                });
                return await Task.FromResult(cachedResult);
                

            }catch(Exception e)
            {
                throw e;
            }
        }

        #endregion

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
            GC.SuppressFinalize(this);
            Dispose(true);         
        }
        #endregion


    }
}
