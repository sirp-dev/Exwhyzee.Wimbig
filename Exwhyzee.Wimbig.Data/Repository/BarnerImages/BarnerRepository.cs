using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.BarnerImage;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.BarnerImages
{
    public class BarnerRepository: IBarnerRepository
    {
        #region constants
        private const string CACHE_IMAGEFILE = "Exwhyzee.Wimbig.Data.Barner";
        private const int CACHE_EXPIRATION_MINUTE = 30;
        #endregion

        #region Fields
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public BarnerRepository(IStorage storage,IMemoryCache memoryCache,ISignal signal,IClock clock)
        {
            _storage = storage;
            _memoryCache = memoryCache;
            _signal = signal;
            _clock = clock;
        }

        public async Task<long> Insert(BarnerFile img)
                                                                            {
            try
            {
               if (img == null)
                throw new ArgumentNullException(nameof(img),"Model cannot be null");

                    img = _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spBarnerImageFileInsert @url,@dateCreated,@status,@isDefault";
                        img.Id = conn.ExecuteScalar<long>(sql, new
                        {
                            img.Url,
                            img.DateCreated,
                            img.Status,
                            img.IsDefault
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                        return img;
                    });
                   _signal.SignalToken(CACHE_IMAGEFILE);
                   return await Task.FromResult(img.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Upate(BarnerFile image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            image = _storage.UseConnection(conn =>
            {
                string sql = "spImagefileUpdate @id,@url,@extension,@dateCreated,@status";
                conn.Execute(sql, new
                {
                    image.Id,
                    image.Url,
                    image.Extension,
                    image.DateCreated,
                    image.Status
                }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                return image;
            });

            _signal.SignalToken(CACHE_IMAGEFILE);
            await Task.CompletedTask;
        }

        public async Task Delete(long Id)
        {
            if (Id < 1)
                throw new ArgumentOutOfRangeException(nameof(Id));

            var model = _storage.UseConnection(conn =>
            {
                string sql = "spBarnerImageFileDelete @Id";
                conn.Execute(sql, new { Id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                return true;
            });

            _signal.SignalToken(CACHE_IMAGEFILE);
             await Task.CompletedTask;
        }

        public async Task<BarnerFile> GetById(long Id)
        {
            if (Id < 1)
                throw new ArgumentOutOfRangeException(nameof(Id));

            string cacheKey = $"{CACHE_IMAGEFILE}.getById.{Id}";
            var image = _memoryCache.GetOrCreate(cacheKey, (entry) => {

                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_IMAGEFILE));
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);
              
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spBarnerImageFileGetById @Id";
                    return conn.QueryFirstOrDefault<BarnerFile>(sql, new { Id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                });
            });
            return await Task.FromResult(image);
        }

        //TODO: This should be in mapImagesToRaffle
        public async Task<PagedList<BarnerFile>> GetAllBarner(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_IMAGEFILE}.getAllBarner";
            var barnerimage = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_IMAGEFILE));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spBarnerImageFileGetAll @dateStart,@dateEnd, @startIndex, @count, @searchString";
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
                        var query = multi.Read<BarnerFile>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<BarnerFile>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(barnerimage);
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
