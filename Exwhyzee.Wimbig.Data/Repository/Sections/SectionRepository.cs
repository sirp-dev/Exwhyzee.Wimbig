using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.Sections;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Sections
{
    public class SectionRepository : ISectionRepository                                                                                                                                                                                                                                                                                                                                                                                                                                                           
    {
        #region Const

        private const string CACHE_SECTION = "exwhyzee.wimbig.section";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public SectionRepository(
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

        public async Task<long> Add(Section entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spSectionInsert @name,@description,@datecreated,@entityStatus";

                    entity.SectionId = conn.ExecuteScalar<long>(sql,
                        new
                        {
                            name = entity.Name,
                            description = entity.Description,
                            dateCreated = entity.DateCreated,
                            entityStatus = EntityStatus.Active                           
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                    return entity;
                });

                _signal.SignalToken(CACHE_SECTION);
                return await Task.FromResult(entity.SectionId);
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
                if (id == 0)
                    throw new ArgumentNullException(nameof(id));

                var section = await Get(id);

                if (section == null)
                    throw new ArgumentNullException(nameof(section));

                section.EntityStatus = Enums.EntityStatus.Deleted;
                await Update(section);

                _signal.SignalToken(CACHE_SECTION);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Section> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_SECTION}.getbyid:{id}";
            var section = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_SECTION));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spSectionGetById @id";
                    return conn.QueryFirstOrDefault<Section>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });
            return await Task.FromResult(section);
        }

        public async Task<PagedList<Section>> GetAsync(int? status = 0, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_SECTION}.getall.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var sections = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_SECTION));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spSectionGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                    var result = conn.Query<Section>(query, new
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

            var filterCount = sections.AsList().Count;
            var paggedResult = new PagedList<Section>(source: sections,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
        }

        public async Task Update(Section entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spSectionUpdate @sectionId,@name,@description,@entityStatus";
                    conn.Execute(sql,
                        new
                        {
                            entity.SectionId,
                            entity.Name,
                            entity.Description,
                            entity.EntityStatus
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                    return entity;
                });
                _signal.SignalToken(CACHE_SECTION);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }
    }
}

