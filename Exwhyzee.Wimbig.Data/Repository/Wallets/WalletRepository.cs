using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.Transactions;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Wallets
{
    public class WalletRepository : IWalletRepository
    {
        #region Const

        private const string CACHE_WALLETS = "exwhyzee.wimbig.wallets";
        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;

        #endregion

        #region Ctor
        public WalletRepository(
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


        public async Task<PagedList<Wallet>> GetAllWallets(int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {

            string cacheKey = $"{CACHE_WALLETS}.getAllWallets.{startIndex}.{count}.{searchString}";
            var transactions = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WALLETS));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spWalletReadAll @startIndex, @count, @searchString";
                    var parameters = new
                    {
                        startIndex,
                        count,
                        searchString
                    };

                    using (var multi = conn.QueryMultiple(sql, parameters,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                    {
                        var query = multi.Read<Wallet>();
                        var summary = multi.ReadFirstOrDefault<dynamic>();

                        return new PagedList<Wallet>(source: query,
                            pageIndex: startIndex,
                            pageSize: count,
                            filteredCount: summary.TotalCount,
                            totalCount: summary.FilteredCount);
                    }
                });
            });

            return await Task.FromResult(transactions);
        }

        public async Task<Wallet> GetWallet(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            string cacheKey = $"{CACHE_WALLETS}.byuserid:{userId}";
            var wallet = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_WALLETS));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spWalletReadByUserId @userId";
                    return conn.QueryFirstOrDefault<Wallet>(sql,
                        new { userId },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(wallet);
        }

        public async Task<Wallet> InsertWallet(Wallet wallet)
        {
            try
            {
                if (wallet == null)
                    throw new ArgumentNullException(nameof(wallet));

                wallet = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spWalletInsert @userId,@balance,@dateUpdated";

                    wallet.Id = conn.ExecuteScalar<long>(sql,
                        new
                        {
                            userId = wallet.UserId,
                            balance = wallet.Balance,
                            dateUpdated = wallet.DateUpdated
                    
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return wallet;
                });

                _signal.SignalToken(CACHE_WALLETS);
                return await Task.FromResult(wallet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Wallet> UpdateWallet(Wallet wallet)
        {
            try
            {
                if (wallet == null)
                    throw new ArgumentNullException(nameof(wallet));

                wallet = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spWalletUpdate @id,@userId,@balance,@dateUpdated";

                    conn.Execute(sql,
                        new
                        {
                            id = wallet.Id,
                            userId = wallet.UserId,
                            balance = wallet.Balance,
                            dateUpdated = wallet.DateUpdated
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return wallet;
                });

                _signal.SignalToken(CACHE_WALLETS);
                return await Task.FromResult(wallet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

      
    }
}
