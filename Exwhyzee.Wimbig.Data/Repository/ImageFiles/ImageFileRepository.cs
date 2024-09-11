using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.ImageFiles
{
    public class ImageFileRepository: IImageFileRepository
    {
        #region constants
        private const string CACHE_IMAGEFILE = "Exwhyzee.Wimbig.Data.ImageFile";
        private const int CACHE_EXPIRATION_MINUTE = 30;
        #endregion

        #region Fields
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public ImageFileRepository(IStorage storage,IMemoryCache memoryCache,ISignal signal,IClock clock)
        {
            _storage = storage;
            _memoryCache = memoryCache;
            _signal = signal;
            _clock = clock;
        }

        public async Task<long> Insert(ImageFile img)
                                                                            {
            try
            {
               if (img == null)
                throw new ArgumentNullException(nameof(img),"Model cannot be null");

                    img = _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spImageFileInsert @url,@dateCreated,@status,@isDefault";
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

        public async Task Upate(ImageFile image)
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
                string sql = "spImageFileDelete @Id";
                conn.Execute(sql, new { Id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                return true;
            });

            _signal.SignalToken(CACHE_IMAGEFILE);
             await Task.CompletedTask;
        }

        public async Task<ImageFile> GetById(long Id)
        {
            if (Id < 1)
                throw new ArgumentOutOfRangeException(nameof(Id));

            string cacheKey = $"{CACHE_IMAGEFILE}.getById.{Id}";
            var image = _memoryCache.GetOrCreate(cacheKey, (entry) => {

                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_IMAGEFILE));
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);
              
                return _storage.UseConnection(conn =>
                {
                    string sql = $"[dbo].[spImageFileGetById] @id";
                    return conn.QueryFirstOrDefault<ImageFile>(sql, new { Id }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                });
            });
            return await Task.FromResult(image);
        }

        //TODO: This should be in mapImagesToRaffle
        public Task<IEnumerable<ImageFile>> GetImagesOfARaffle(long reffleId)
        {
            return null;
        }

        public async Task<PagedList<ImageFile>> GetAsyncAll(string extension = null, DateTime? dateStart = null, DateTime? dateStop = null, int startIndex = 0, int count = int.MaxValue)
        {

            string cacheKey = $"{CACHE_IMAGEFILE}.GetAsyncAllimages.{extension}.{count}.{dateStart}";
            var img = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTE);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_IMAGEFILE));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spImageFileGetAll @extension,@dateStart,@dateStop, @startIndex, @count";
                 


                    var result = conn.Query<ImageFile>(sql, new
                    {
                        extension = extension,
                        dateStart = dateStart,
                        dateStop = dateStop,
                        startIndex = startIndex,
                        count = count,
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return result;
                });
            });

            var filterCount = img.AsList().Count;
            var paggedResult = new PagedList<ImageFile>(source: img,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);
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
