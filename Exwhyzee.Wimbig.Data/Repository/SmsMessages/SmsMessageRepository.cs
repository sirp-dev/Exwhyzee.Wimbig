using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.SmsMessages;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.SmsMessages
{
    public class SmsMessageRepository : ISmsMessageRepository
    {
        #region Const

        private const string CACHE_SMS = "exwhyzee.wimbig.sms";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields
    
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public SmsMessageRepository(
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

        public async Task<long> Add(SmsMessage entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spSmsMessageInsert @senderId,@dateCreated,@recipient,@message,@status,@response";

                    entity.Id = conn.ExecuteScalar<int>(sql,
                        new
                        {
                           senderId = entity.SenderId,
                           dateCreated = entity.DateCreated,
                           recipient = entity.Recipient,
                           message = entity.Message,
                           status = entity.Status,
                           response = entity.Response

                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_SMS);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        

        public async Task<PagedList<SmsMessage>>  GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_SMS}.getasyncSms.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var data = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_SMS));
                return _storage.UseConnection(conn =>
                {                   
                        string query = $"dbo.spSmsMessageGetAll @status, @dateStart, @dateEnd, @startIndex, @count, @searchString";
                        var result = conn.Query<SmsMessage>(query, new
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

            var filterCount = data.AsList().Count;
            var paggedResult = new PagedList<SmsMessage>(source: data,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);

            return await Task.FromResult(paggedResult);         
        }

       

        public async Task<SmsMessage> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_SMS}.getsmsbyid:{id}";
            var statistic = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_SMS));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spSmsMessageGetById @id";
                    return conn.QueryFirstOrDefault<SmsMessage>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(statistic);
        }



        public async Task Update(SmsMessage entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity =_storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spSmsMessageUpdate @id,@senderId,@dateCreated,@recipient,@message,@status,@response";

                    conn.Execute(sql,
                        new
                        {
                            entity.Id,
                           entity.SenderId,
                           entity.DateCreated,
                           entity.Recipient,
                           entity.Message,
                           entity.Status,
                           entity.Response
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_SMS);
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

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }



        #endregion
        #endregion
    }
}
