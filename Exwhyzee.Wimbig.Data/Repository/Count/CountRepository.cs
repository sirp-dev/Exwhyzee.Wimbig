using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Raffles;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Data.Repository.Count;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Tickets
{
    public class CountRepository : ICountRepository
    {
        #region Const

        private const string CACHE_COUNTS = "exwhyzee.wimbig.counts";
       
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;

        #endregion

        #region Ctor
        public CountRepository(
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
       
        public Task<int> TotalAgents(string rolename)
        {
            //return _storage.UseConnection(conn =>
            //{
            //    string sql = $"dbo.spTransactionsReadAll @rolename";
            //    var parameters = new
            //    {
            //        rolename
            //    };

            //    using (var multi = conn.QueryMultiple(sql, parameters,
            //        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
            //    {
            //        var query = multi.Read<Transaction>();
            //        var summary = multi.ReadFirstOrDefault<dynamic>();

            //        return new PagedList<Transaction>(source: query,
            //            pageIndex: startIndex,
            //            pageSize: count,
            //            filteredCount: summary.TotalCount,
            //            totalCount: summary.FilteredCount);
            //    }
            //});

            throw new NotImplementedException();
        }

        public async Task<decimal> TotalAmount(int status = 0)
        {
           decimal entity = _storage.UseConnection(conn =>
            {
                string sql = $"dbo.spTotalWimbankDeposite @status";

                decimal result = conn.QueryFirstOrDefault<decimal>(sql, new
                {
                    status = status,
                }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                return result;
            });
            
            return await Task.FromResult(entity);
        }

        public Task<int> TotalDgas(string rolename)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalTicket()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalTicket(string userid = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalUsers()
        {
            throw new NotImplementedException();
        }

       

        public async Task<PagedList<UsersInRole>> UsersInRole(string roleid = null, int startIndex = 0, int count = int.MaxValue)
        {
            try
            {
                string cacheKey = $"{CACHE_COUNTS}.getcounts.{roleid}.{startIndex}.{count}";
                var statistics = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_COUNTS));
                    return _storage.UseConnection(conn =>
                    {
                        string query = $"dbo.spUserAndRoleGetAll @roleid, @startIndex, @count";
                        var result = conn.Query<UsersInRole>(query, new
                        {
                            roleid = roleid,
                            startIndex = startIndex,
                            count = count,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                        return result;
                    });
                });

                var filterCount = statistics.AsList().Count;
                var paggedResult = new PagedList<UsersInRole>(source: statistics,
                                    pageIndex: startIndex,
                                    pageSize: count,
                                    filteredCount: filterCount,
                                    totalCount: filterCount);
                _signal.SignalToken(CACHE_COUNTS);
                return await Task.FromResult(paggedResult);
            }catch(Exception d)
            {
                return null;
            }
        }

        public async Task<PagedList<UsersInRole>> UsersInRoleForDgaAgentSup(string roleid = null, int startIndex = 0, int count = int.MaxValue)
        {
            string cacheKey = $"{CACHE_COUNTS}.getcountsdga.{roleid}.{startIndex}.{count}";
            var statistics = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_COUNTS));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spTotalUserByRole @roleid, @startIndex, @count";
                    var result = conn.Query<UsersInRole>(query, new
                    {
                        roleid = roleid,
                        startIndex = startIndex,
                        count = count,
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return result;
                });
            });

            var filterCount = statistics.AsList().Count;
            var paggedResult = new PagedList<UsersInRole>(source: statistics,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);
            _signal.SignalToken(CACHE_COUNTS);
            return await Task.FromResult(paggedResult);
        }
    }

}
