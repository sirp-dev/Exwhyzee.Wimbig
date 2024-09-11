using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Exwhyzee.Wimbig.Core.SideBarner;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.SideBarImages
{
    public class SideBarImageRepository : ISideBarImageRepository
    {
        #region constants
        private const string CACHE_SIDEBAR = "Exwhyzee.Wimbig.Data.barner";
        private const int CACHE_EXPIRATION_MINUTE = 30;
        #endregion

        #region Fields
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public SideBarImageRepository(IStorage storage,IMemoryCache memoryCache,ISignal signal,IClock clock)
        {
            _storage = storage;
            _memoryCache = memoryCache;
            _signal = signal;
            _clock = clock;
        }

        public async Task<long> Insert(SideBarnerFile img)
                                                                            {
            try
            {
               if (img == null)
                throw new ArgumentNullException(nameof(img),"Model cannot be null");

                    img = _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spSideBarnerFileInsert @url,@dateCreated,@status,@isDefault,@targetLocation";
                        img.Id = conn.ExecuteScalar<long>(sql, new
                        {
                            img.Url,
                            img.DateCreated,
                            img.Status,
                            img.IsDefault,
                            img.TargetLocation
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                        return img;
                    });
                   _signal.SignalToken(CACHE_SIDEBAR);
                   return await Task.FromResult(img.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

  

        public async Task Delete(long Id)
        {
            if (Id < 1)
                throw new ArgumentOutOfRangeException(nameof(Id));

            var model = _storage.UseConnection(conn =>
            {
                string sql = "spSideBarnerFileDelete @Id";
                conn.Execute(sql, new { Id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                return true;
            });

            _signal.SignalToken(CACHE_SIDEBAR);
             await Task.CompletedTask;
        }

        public async Task<SideBarnerFile> GetById(long Id)
        {
            if (Id < 1)
                throw new ArgumentOutOfRangeException(nameof(Id));

            string cacheKey = $"{CACHE_SIDEBAR}.getByIdSlider.{Id}";
            var image = _memoryCache.GetOrCreate(cacheKey, (entry) => {

                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_SIDEBAR));
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);
              
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spSideBarnerFileGetById @Id";
                    return conn.QueryFirstOrDefault<SideBarnerFile>(sql, new { Id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                });
            });
            return await Task.FromResult(image);
        }
  

        public async Task<PagedList<SideBarnerFile>> GetAllSideBarImage(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_SIDEBAR}.getAllSideBarImage";
            var barnerimage = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_SIDEBAR));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spSideBarnerFileGetAll @dateStart,@dateEnd, @startIndex, @count, @searchString";
                    var parameters = new
                    {
                       
                        dateStart,
                        dateEnd,
                        startIndex,
                        count,
                        searchString
                    };

                    using (var multi = conn.QueryMultiple(sql, parameters,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                    {
                        var query = multi.Read<SideBarnerFile>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<SideBarnerFile>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(barnerimage);
        }
        //TODO: This should be in mapImagesToRaffle
      

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

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

     
        #endregion
        #endregion

    }
}
