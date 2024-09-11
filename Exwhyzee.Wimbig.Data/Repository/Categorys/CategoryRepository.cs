using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.Categories;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Categorys
{
    public class CategoryRepository : ICategoryRepository
    {
        #region Const

        private const string CACHE_CATEGORY = "exwhyzee.wimbig.category";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields
    
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public CategoryRepository(
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

        public async Task<long> Add(Category entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spCategoryInsert @name,@description,@datecreated,@sectionId,@entityStatus";

                    entity.CategoryId = conn.ExecuteScalar<int>(sql,
                        new
                        {
                            name = entity.Name,
                            description= entity.Description,
                            dateCreated = entity.DateCreated,
                            sectionId = (long)entity.SectionId,
                            entityStatus = entity.EntityStatus
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_CATEGORY);
                return await Task.FromResult(entity.CategoryId);
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

                var category = await Get(id);

                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                category.EntityStatus = EntityStatus.Deleted;
                await Update(category);

                _signal.SignalToken(CACHE_CATEGORY);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<CategorySectionDetailsDto>>  GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_CATEGORY}.getall.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var categories = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CATEGORY));
                return _storage.UseConnection(conn =>
                {                   
                        string query = $"dbo.spCategoryGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<CategorySectionDetailsDto>(query, new
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

            var filterCount = categories.AsList().Count;
            var paggedResult = new PagedList<CategorySectionDetailsDto>(source: categories,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);         
        }

        public async Task<List<Category>> GetCategoriesBySection(long sectionId)
        {
            if (sectionId <= 0 || sectionId > long.MaxValue)
                throw new ArgumentNullException(nameof(sectionId));

            string cacheKey = $"{CACHE_CATEGORY}.getBySectionId.{sectionId}";
            var categories = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CATEGORY));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spCategoryGetAllBySectionId @sectionId";
                    return conn.Query<Category>(sql,
                        new { sectionId },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        ).AsList();
                });
            });

            return await Task.FromResult(categories);
        }

        public async Task<Category> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_CATEGORY}.getbyid:{id}";
            var category = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_CATEGORY));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spCategoryGetById @id";
                    return conn.QueryFirstOrDefault<Category>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(category);
        }

        public async Task Update(Category entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity =_storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spCategoryUpdate @categoryId,@name,@dateCreated,@description,@sectionId,@entityStatus";

                    conn.Execute(sql,
                        new
                        {
                            entity.CategoryId,
                            entity.Name,
                            entity.DateCreated,
                            entity.Description,
                            entity.SectionId,
                            entity.EntityStatus
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_CATEGORY);
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
