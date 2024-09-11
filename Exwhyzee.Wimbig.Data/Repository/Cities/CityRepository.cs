using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.Cities;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Cities
{
    public class CityRepository : ICityRepository
    {
        #region Const

        private const string CACHE_CITY = "exwhyzee.wimbig.city";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public CityRepository(
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

        public async Task<long> Add(City entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spCityInsert @name";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            Name = entity.Name
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_CITY);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }



        public async Task<PagedList<City>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_CITY}.getallcity.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var agent = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CITY));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spCityGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                    var result = conn.Query<City>(query, new
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

            var filterCount = agent.AsList().Count;
            var paggedResult = new PagedList<City>(source: agent,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
        }



        public async Task<City> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_CITY}.getcitytbyid:{id}";
            var agent = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CITY));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spCityGetById @id";
                    return conn.QueryFirstOrDefault<City>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(agent);
        }

        public async Task Update(City entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                 {
                     var sql = $"dbo.spCityUpdate @id,@name";

                     conn.Execute(sql,
                         new
                         {
                             entity.Id,
                             entity.Name
                         },
                         commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                     return entity;
                 });

                _signal.SignalToken(CACHE_CITY);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task Delete(long id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var city = await Get(id);

            if (city == null)
                throw new ArgumentNullException(nameof(city));
            
            await Update(city);

            _signal.SignalToken(CACHE_CITY);
            await Task.CompletedTask;
        }



        public async Task<long> AddAreaInCity(AreaInCity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spAreaInCityInsert @name,@cityId";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            Name = entity.Name,
                            CityId = entity.CityId
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_CITY);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }



        public async Task<PagedList<AreaInCity>> GetAreaInCityAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_CITY}.getallAreaIncity.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var agent = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CITY));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spAreaInCityGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                    var result = conn.Query<AreaInCity>(query, new
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

            var filterCount = agent.AsList().Count;
            var paggedResult = new PagedList<AreaInCity>(source: agent,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
        }



        public async Task<AreaInCity> GetAreaInCity(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_CITY}.getcAreaInCitybyid:{id}";
            var agent = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CITY));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spAreaInCityGetById @id";
                    return conn.QueryFirstOrDefault<AreaInCity>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(agent);
        }

        public async Task UpdateAreaInCity(AreaInCity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spAreaInCityUpdate @id,@name,@cityId";

                    conn.Execute(sql,
                        new
                        {
                            entity.Id,
                            entity.Name,
                            entity.CityId
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_CITY);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task DeleteAreaInCity(long id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var city = await GetAreaInCity(id);

            if (city == null)
                throw new ArgumentNullException(nameof(city));

            await UpdateAreaInCity(city);

            _signal.SignalToken(CACHE_CITY);
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

        public async Task<PagedList<AreaInCity>> GetAreaInCityByCityIdAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long cityId = 0)
        {
            string cacheKey = $"{CACHE_CITY}.getallAreaIncitybysid.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var agent = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CITY));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spAreaCityByCityName @dateStart, @dateEnd, @startIndex, @count, @searchString, @cityId";
                    var result = conn.Query<AreaInCity>(query, new
                    {
                       
                        dateStart = dateStart,
                        dateEnd = dateEnd,
                        startIndex = startIndex,
                        count = count,
                        searchString = searchString,
                        cityId = cityId
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return result;
                });
            });

            var filterCount = agent.AsList().Count;
            var paggedResult = new PagedList<AreaInCity>(source: agent,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            _signal.SignalToken(CACHE_CITY);
            return await Task.FromResult(paggedResult);
        }
        #endregion
        #endregion
    }
}
