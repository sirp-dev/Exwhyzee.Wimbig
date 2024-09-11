using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys.Dto;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.MapRaffleToCategorys
{
    public class MapRaffleToCategoryRepository : IMapRaffleToCategoryRepository
    {
        #region Const

        private const string CACHE_RAFFLECATEGORY = "exwhyzee.wimbig.raffleCategory";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public MapRaffleToCategoryRepository(
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

        public async Task<long> Add(MapRaffleToCategory entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spMapRaffleToCategoryInsert @raffleId,@raffleName,@categoryId,@categoryName,@dateCreated";

                    entity.CategoryId = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            raffleId = entity.RaffleId,
                            raffleName = entity.RaffleName,
                            categoryId = entity.CategoryId,
                            categoryName = entity.CategoryName,
                            dateCreated = entity.DateCreated
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_RAFFLECATEGORY);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task Delete(long id)
        {
            try
            {
                if (id < 0 || id > long.MaxValue)
                    throw new ArgumentOutOfRangeException(nameof(id));

                id = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spMapRaffleToCategoryDelete @id";
                    conn.Execute(sql, new {id}, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return id;
                });

                _signal.SignalToken(CACHE_RAFFLECATEGORY);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async  Task<MapRaffleToCategory> Get(long id)
        {
            if (id <= 0 )
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_RAFFLECATEGORY}.getbyid:{id}";
            var category = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLECATEGORY));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spMapRaffleToCategoryGetById @id";
                    return conn.QueryFirstOrDefault<MapRaffleToCategory>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(category);
        }

        public async Task<PagedList<MapRaffleToCategory>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_RAFFLECATEGORY}.getall.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var mappings = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLECATEGORY));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spMapRaffleToCategoryGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                    var result = conn.Query<MapRaffleToCategory>(query, new
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

            var filterCount = mappings.AsList().Count;
            var paggedResult = new PagedList<MapRaffleToCategory>(source: mappings,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
        }

        public async Task<MapRaffleToCategory> GetByRaffleId(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_RAFFLECATEGORY}.getbyraffleid:{id}";
            var category = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLECATEGORY));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spMapRaffleToCategoryGetByRaffleId @id";
                    return conn.QueryFirstOrDefault<MapRaffleToCategory>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(category);
        }

        public async Task<PagedList<RaffleCagtegoryDto>> GetRafflesByCategory(long categoryId)
        {
            string cacheKey = $"{CACHE_RAFFLECATEGORY}.getRafflesByCategory.{categoryId}";
            var mappings = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLECATEGORY));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spMapRaffleToCategoryGetRafflesByCategoryId @categoryId";
                    var result = conn.Query<RaffleCagtegoryDto>(query, new
                    {
                       categoryId
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return result;
                });
            });

            var filterCount = mappings.AsList().Count;
            var paggedResult = new PagedList<RaffleCagtegoryDto>(source: mappings,
                                pageIndex: 1,
                                pageSize: 1,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
        }

        public async Task Update(MapRaffleToCategory entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spMapRaffleToCategoryUpdate @id,@raffleId,@raffleName,@categoryId,@categoryName,@dateCreated";

                    conn.Execute(sql,
                        new
                        {
                            id = entity.Id,
                            raffleId = entity.RaffleId,
                            raffleName = entity.RaffleName,
                            categoryId = entity.CategoryId,
                            categoryName = entity.CategoryName,
                            dateCreated = entity.DateCreated
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                    return entity;
                });

                _signal.SignalToken(CACHE_RAFFLECATEGORY);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }
    }
}
