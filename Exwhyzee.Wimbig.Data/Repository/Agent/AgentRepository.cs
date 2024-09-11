using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.Agent;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using Exwhyzee.Wimbig.Data.Repository.Agent;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.DailyStatistics
{
    public class AgentRepository : IAgentRepository
    {
        #region Const

        private const string CACHE_AGENTS = "exwhyzee.wimbig.agent";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public AgentRepository(
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

        public async Task<long> Add(Agents entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spAgentInsert @fullname,@emailAddress,@state,@lga,@areYouNewToWimbig,@contactAddress,@dateCreated,@currentOccupation,@phoneNumber,@shopLocation,@gender,@status";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            fullname = entity.Fullname,
                            emailAddress = entity.EmailAddress,
                            state = entity.State,
                            lga = entity.LGA,
                            areYouNewToWimbig = entity.AreYouNewToWimbig,
                            contactAddress = entity.ContactAddress,
                            dateCreated = entity.DateCreated,
                            currentOccupation = entity.CurrentOccupation,
                            phoneNumber = entity.PhoneNumber,
                            shopLocation = entity.ShopLocation,
                            gender = entity.Gender,
                            status = entity.Status
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_AGENTS);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }



        public async Task<PagedList<Agents>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_AGENTS}.getallagent.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var agent = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_AGENTS));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spAgentGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                    var result = conn.Query<Agents>(query, new
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
            var paggedResult = new PagedList<Agents>(source: agent,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
        }



        public async Task<Agents> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_AGENTS}.getagentbyid:{id}";
            var agent = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_AGENTS));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spAgentGetById @id";
                    return conn.QueryFirstOrDefault<Agents>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(agent);
        }

        public async Task Update(Agents entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spAgentUpdate @id,@fullname,@emailAddress,@state,@lga,@areYouNewToWimbig,@contactAddress,@dateCreated,@currentOccupation,@phoneNumber,@shopLocation,@gender,@status";

                    conn.Execute(sql,
                        new
                        {
                            entity.Id,
                            entity.Fullname,
                            entity.EmailAddress,
                            entity.State,
                            entity.LGA,
                            entity.AreYouNewToWimbig,
                            entity.ContactAddress,
                            entity.DateCreated,
                            entity.CurrentOccupation,
                            entity.PhoneNumber,
                            entity.ShopLocation,
                            entity.Gender,
                            entity.Status
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_AGENTS);
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
