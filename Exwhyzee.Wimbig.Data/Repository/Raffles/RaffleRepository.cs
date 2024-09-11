using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Raffles
{
    public class RaffleRepository : IRaffleRepository
    {
        #region Const

        private const string CACHE_RAFFLES = "exwhyzee.wimbig.raffles";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        #endregion

        #region Ctor
        public RaffleRepository(
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

        public async Task<long> Add(Raffle entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spRaffleInsert @name,@description,@hostedBy,@numberOfTickets," +
                    $"@pricePerTicket, @deliveryType, @startDate, @endDate, @status, @dateCreated, @totalSold, @sortOrder, @location, @areaInCity";

                    entity.Id = conn.ExecuteScalar<long>(sql,
                        new
                        {
                            name = entity.Name,
                            description = entity.Description,
                            hostedBy = entity.HostedBy,
                            
                            numberOfTickets = entity.NumberOfTickets,
                            pricePerTicket = entity.PricePerTicket,
                            deliveryType = entity.DeliveryType,
                            startDate = entity.StartDate,
                            endDate = entity.EndDate,
                            status = entity.Status,
                            dateCreated = entity.DateCreated,
                            totalSold = entity.TotalSold,
                            sortOrder = entity.SortOrder,
                            location = entity.Location,
                            areaInCity = entity.AreaInCity

    },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_RAFFLES);
                return await Task.FromResult(entity.Id);
            }
            catch(Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task Delete(long id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

            var raffle = await Get(id);

            if (raffle == null)
                throw new ArgumentNullException(nameof(raffle));

            raffle.Status = EntityStatus.Deleted;
            await Update(raffle);

            _signal.SignalToken(CACHE_RAFFLES);
            await Task.CompletedTask;
        }

        public async Task<Raffle> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_RAFFLES}.rafflerepbyid:{id}";
            var raffle = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLES));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spRaffleGetById @id";
                    return conn.QueryFirstOrDefault<Raffle>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });
            _signal.SignalToken(CACHE_RAFFLES);
            return await Task.FromResult(raffle);
        }
        //get raffle all

        public async Task<PagedList<Raffle>> GetAsyncAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_RAFFLES}.GetAsyncAll123.{status}.{startIndex}.{count}.{searchString}";
            var raffles = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLES));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spRaffleGetAllRaffles @status,@dateStart,@dateEnd, @startIndex, @count, @searchString";
                    var parameters = new
                    {
                        status,
                        dateStart,
                        dateEnd,
                        startIndex,
                        count,
                        searchString
                    };

                    using (var multi = conn.QueryMultiple(sql, parameters,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                    {
                        var query = multi.Read<Raffle>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<Raffle>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(raffles);
        }

        public async Task<PagedList<Raffle>> GetRaffleByLocationAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_RAFFLES}.GetAsyncGetRaffleByLocationAll.{status}.{startIndex}.{count}.{searchString}";
            var raffles = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLES));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spRaffleByLocation @status,@dateStart,@dateEnd, @startIndex, @count, @searchString";
                    var parameters = new
                    {
                        status,
                        dateStart,
                        dateEnd,
                        startIndex,
                        count,
                        searchString
                    };

                    using (var multi = conn.QueryMultiple(sql, parameters,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                    {
                        var query = multi.Read<Raffle>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<Raffle>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(raffles);
        }


        //get raffle active
        public async Task<PagedList<Raffle>> GetAsync(int? status = null,DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_RAFFLES}.all.{status}.{startIndex}.{count}.{searchString}";
            var raffles = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLES));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spRaffleBySortOrder @status,@dateStart,@dateEnd, @startIndex, @count, @searchString";
                    var parameters = new
                    {
                        status,
                        dateStart,
                        dateEnd,
                        startIndex,
                        count,
                        searchString
                    };

                    using (var multi = conn.QueryMultiple(sql, parameters,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                    {
                        var query = multi.Read<Raffle>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<Raffle>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(raffles);
        }

        public async Task Update(Raffle entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spRaffleUpdate @id,@name,@hostedBy,@description,@numberOfTickets," +
                    $"@pricePerTicket,@deliveryType,@startDate,@endDate,@status,@dateCreated,@totalSold, @sortOrder, @paidOut,@archieved , @location, @areaInCity";

                    conn.Execute(sql,
                        new
                        {
                            id = entity.Id,
                            name = entity.Name,
                            hostedBy = entity.HostedBy,
                            description = entity.Description,
                            numberOfTickets = entity.NumberOfTickets,
                            pricePerTicket = entity.PricePerTicket,
                            deliveryType = entity.DeliveryType,
                            startDate = entity.StartDate,
                            endDate = entity.EndDate,
                            status = entity.Status,
                            dateCreated = entity.DateCreated,
                            totalSold = entity.TotalSold,
                            sortOrder = entity.SortOrder,
                            paidOut = entity.PaidOut,
                            archieved = entity.Archived,
                            
                            location = entity.Location,
                            areaInCity = entity.AreaInCity
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_RAFFLES);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task<PagedList<Raffle>> GetRafflesByHostedBy(string hostedBy, int? status = null, int startIndex = 0, int count = int.MaxValue)
        {
            string cacheKey = $"{CACHE_RAFFLES}.getRafflesByHostedBy.{hostedBy}.{status}.{startIndex}.{count}";
            var raffles = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLES));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spRaffleGetRafflesByHostedBy @hostedBy @status, @startIndex, @count";
                    var parameters = new
                    {
                        hostedBy,
                        status,
                        startIndex,
                        count
                    };

                    using (var multi = conn.QueryMultiple(sql, parameters,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                    {
                        var query = multi.Read<Raffle>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<Raffle>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(raffles);

        }

        public async Task<List<Raffle>> GetRafflesByStatus(int status, int count)
        {
            string cacheKey = $"{CACHE_RAFFLES}.getRafflesByStatus.{status}.{count}";
            var raffles = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLES));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spRaffleGetByStatus @status,@count";

                    return conn.Query<Raffle>(sql,
                     new
                    {
                        status,
                        count
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                });
            });

            return await Task.FromResult(raffles.AsList());
        }

        public async Task<bool> AddToArchieve(long id)
        {
            try
            {
                if (id < 1)
                    throw new ArgumentNullException(nameof(id));

                _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spRaffleSendArchieved @id";

                    conn.Execute(sql,
                        new
                        {
                            id = id,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);


                });

                _signal.SignalToken(CACHE_RAFFLES);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> RemoveFromArchieve(long id)
        {
            try
            {
                if (id < 1)
                    throw new ArgumentNullException(nameof(id));

                _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spRaffleRemoveArchieved @id";

                    conn.Execute(sql,
                        new
                        {
                            id = id,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);


                });

                _signal.SignalToken(CACHE_RAFFLES);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PagedList<Raffle>> GetRafflesByArchieved(bool archieved = true, int? status = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            string cacheKey = $"{CACHE_RAFFLES}.getRafflesByArchieved.{archieved}.{status}.{startIndex}.{count}.{searchString}";
            var raffles = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_RAFFLES));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spRaffleByArchieved @archieved,@status, @startIndex, @count, @searchString";
                    var parameters = new
                    {
                        archieved,
                        status,
                        startIndex,
                        count,
                        searchString
                    };

                    using (var multi = conn.QueryMultiple(sql, parameters,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                    {
                        var query = multi.Read<Raffle>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<Raffle>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(raffles);
        }
    }
}
