using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.Raffles;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.Extensions.Caching.Memory;

namespace Exwhyzee.Wimbig.Data.Repository.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        #region Const

        private const string CACHE_TICKETS = "exwhyzee.wimbig.tickets";
        private const string CACHE_TRANSACTION = "exwhyzee.wimbig.transactions";
        private const string CACHE_WALLETS = "exwhyzee.wimbig.wallets";
        private const string CACHE_RAFFLES = "exwhyzee.wimbig.raffles";
        private const string CACHE_MESSAGESTORE = "exwhyzee.wimbig.messagestore";

        private const int CACHE_EXPIRATION_MINUTES = 30;

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
      

        #endregion

        #region Ctor
        public TicketRepository(
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
        public async Task<Transaction> CreateTicket(List<InsertTicket> tickets, Wallet wallet, Transaction transaction)
        {
            try
            {
                if (tickets.Count < 1)
                    throw new ArgumentNullException(nameof(tickets));

                _storage.UseConnection(conn =>
                {
                    IDbTransaction dbTransaction = conn.BeginTransaction();
                   
                    var sql = $"dbo.spTicketInsert @userId,@raffleId,@ticketNumber,@purchaseDate,@transactionId,@price,@yourPhoneNumber,@ticketStatus,@email,@playerName,@currentLocation";
                    var walletSql = $"dbo.spWalletUpdate @id,@userId,@balance,@dateUpdated";
                    var transactionSql = $"dbo.spTransactionInsert @walletId,@userId,@amount,@transactionType,@dateOfTransaction,@status,@sender,@description";
                    var raffleUpdateSql = $"dbo.spRaffleUpdate @id,@name,@description,@hostedBy,@numberOfTickets," +
                    $"@pricePerTicket,@deliveryType,@startDate,@endDate,@status,@dateCreated,@totalSold, @sortOrder, @paidOut,@archieved , @location, @areaInCity";
                    var raffleGetSql = $"dbo.spRaffleGetById @id";

                    if (tickets[0].RaffleId < 1 )
                        throw new ArgumentNullException("The process does not target a correct raffle draw, Check entry and try again");

                    var entity = conn.QueryFirstOrDefault<Raffle>(raffleGetSql,
                         new { id = tickets[0].RaffleId }, transaction: dbTransaction,
                         commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                         );

                    if ((entity.TotalSold + tickets.Count) > entity.NumberOfTickets)
                        throw new ArgumentOutOfRangeException(nameof(entity.NumberOfTickets), "Insufficient Number of Tickets.");

                    conn.Execute(raffleUpdateSql,
                        new
                        {
                            id = entity.Id,
                            name = entity.Name,
                            hostedBy = entity.Description,
                            description = entity.HostedBy,
                            numberOfTickets = entity.NumberOfTickets,
                            pricePerTicket = entity.PricePerTicket,
                            deliveryType = entity.DeliveryType,
                            startDate = entity.StartDate,
                            endDate = entity.EndDate,
                            status = entity.Status,
                            dateCreated = entity.DateCreated,
                            totalSold = entity.TotalSold + tickets.Count, 
                            sortOrder = entity.SortOrder,
                            paidOut = entity.PaidOut,
                            archieved = entity.Archived,
                            location = entity.Location,
                            areaInCity = entity.AreaInCity
                        }, transaction: dbTransaction,
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);


                    transaction.Id = conn.ExecuteScalar<long>(transactionSql,
                       new
                       {
                           walletId = transaction.WalletId,
                           userId = transaction.UserId,
                           amount = transaction.Amount,
                           transactionType = transaction.TransactionType,
                           dateOfTransaction = DateTime.UtcNow.AddHours(1),
                           transactionReference = transaction.TransactionReference,
                           status = transaction.Status,
                           sender = transaction.Sender,
                           description = transaction.Description
                       }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    foreach (var ticket in tickets)
                    {
                        //var checkticket = GetByRaffleIdTicketNumber(ticket.RaffleId, ticket.TicketNumber).Result;
                        //if (checkticket == null)
                        //{
                            long rowAffected = conn.ExecuteScalar<long>(sql,
                                 new
                                 {
                                     userId = ticket.UserId,
                                     raffleId = ticket.RaffleId,
                                     ticketNumber = ticket.TicketNumber,
                                     purchaseDate = DateTime.UtcNow.AddHours(1),
                                     transactionId = transaction.Id,
                                     price = ticket.Price,
                                     yourPhoneNumber = ticket.YourPhoneNumber,
                                     ticketStatus = ticket.TicketStatus,
                                     email = ticket.Email,
                                     playerName = ticket.PlayerName,
                                     currentLocation = ticket.CurrentLocation
                                 },
                                transaction: dbTransaction,
                                 commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
                       // }

                    }


                    conn.Execute(walletSql,
                        new
                        {
                            id = wallet.Id,
                            userId = wallet.UserId,
                            balance = wallet.Balance,
                            dateUpdated = DateTime.UtcNow.AddHours(1),
                            wallet
                        }, transaction: dbTransaction, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    dbTransaction.Commit();
                });

                _signal.SignalToken(CACHE_TICKETS);
                _signal.SignalToken(CACHE_TRANSACTION);
                _signal.SignalToken(CACHE_WALLETS);
                _signal.SignalToken(CACHE_RAFFLES);
                _signal.SignalToken(CACHE_MESSAGESTORE);
                return await Task.FromResult(transaction);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task<PagedList<Ticket>> GetAllTickets(DateTime? dateStart = null, DateTime? dateEnd = null,
            int startIndex = 0, int count = int.MaxValue, string searchString = null, long? raffleId = null,
            bool? isWinner = null, bool? isSentToStat = null)
        {
            try
            {
                string cacheKey = $"{CACHE_TICKETS}.all.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}.{raffleId}.{isSentToStat}";
                var tickets = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTicketsReadAll @dateStart,@dateEnd,@startIndex, @count, @searchString,@raffleId,@isWinner,@isSentToStat";
                        var parameters = new
                        {
                            dateStart,
                            dateEnd,
                            startIndex,
                            count,
                            searchString,
                            raffleId,
                            isWinner,
                            isSentToStat
                        };

                        using (var multi = conn.QueryMultiple(sql, parameters,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Ticket>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return new PagedList<Ticket>(source: query,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: summary.TotalCount,
                                totalCount: summary.FilteredCount);
                           
                        }
                    });
                });
                _signal.SignalToken(CACHE_TICKETS);
                return await Task.FromResult(tickets);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        ///get ticket by refernce id
        ///

        public async Task<PagedList<Ticket>> GetAllTicketsByReferenceId(DateTime? dateStart = null, DateTime? dateEnd = null,
       int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null,
       bool? isWinner = null, bool? isSentToStat = null)
        {
            try
            {
                string cacheKey = $"{CACHE_TICKETS}.getAllTicketsByReferenceId.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}.{referenceId}.{isSentToStat}";
                var tickets = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTicketsReadAllByReferenceId @dateStart,@dateEnd,@startIndex, @count, @searchString,@referenceId,@isWinner,@isSentToStat";
                        var parameters = new
                        {
                            dateStart,
                            dateEnd,
                            startIndex,
                            count,
                            searchString,
                            referenceId,
                            isWinner,
                            isSentToStat
                        };

                        using (var multi = conn.QueryMultiple(sql, parameters,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Ticket>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return new PagedList<Ticket>(source: query,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: summary.TotalCount,
                                totalCount: summary.FilteredCount);

                        }
                    });
                });
               

                return await Task.FromResult(tickets);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        ///end

        public async Task<PagedList<Ticket>> GetAllTicketsByReferenceIdUsers(DateTime? dateStart = null, DateTime? dateEnd = null,
  int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null,
  bool? isWinner = null, bool? isSentToStat = null)
        {
            try
            {
                string cacheKey = $"{CACHE_TICKETS}.getAllTicketsByReferenceIdUsers.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}.{referenceId}.{isSentToStat}";
                var tickets = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTicketsReadAllByReferenceIdUsers @dateStart,@dateEnd,@startIndex, @count, @searchString,@referenceId,@isWinner,@isSentToStat";
                        var parameters = new
                        {
                            dateStart,
                            dateEnd,
                            startIndex,
                            count,
                            searchString,
                            referenceId,
                            isWinner,
                            isSentToStat
                        };

                        using (var multi = conn.QueryMultiple(sql, parameters,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Ticket>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return new PagedList<Ticket>(source: query,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: summary.TotalCount,
                                totalCount: summary.FilteredCount);

                        }
                    });
                });

                return await Task.FromResult(tickets);
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        /// <summary>
        /// ///
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="startIndex"></param>
        /// 


        public async Task<List<Ticket>> GetAllWinners()
        {
            try
            {
                string cacheKey = $"{CACHE_TICKETS}.getAllWinners";
                var tickets = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TICKETS));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTicketsReadAllTicket";
                       

                        using (var multi = conn.QueryMultiple(sql,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Ticket>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return query;

                        }
                    });
                });
                _signal.SignalToken(CACHE_TICKETS);
                return await Task.FromResult(tickets.AsList());
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// 
        /// 
        /// <param name="count"></param>
        /// <param name="searchString"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>


        public async Task<PagedList<Ticket>> GetAllTicketsByTransactionId(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long? transactionId = null)
        {
            try
            {
                string cacheKey = $"{CACHE_TICKETS}.getAllTicketsByTransactionId.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}.{transactionId}";
                var tickets = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTicketByTransactionId @dateStart,@dateEnd,@startIndex, @count, @searchString,@transactionId";
                        var parameters = new
                        {
                            dateStart,
                            dateEnd,
                            startIndex,
                            count,
                            searchString,
                            transactionId
                        };

                        using (var multi = conn.QueryMultiple(sql, parameters,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Ticket>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return new PagedList<Ticket>(source: query,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: summary.TotalCount,
                                totalCount: summary.FilteredCount);

                        }
                    });
                });

                return await Task.FromResult(tickets);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<long> UpdateTicketStatus(long ticketId)
        {
            try
            {
                if (ticketId < 1)
                    throw new ArgumentNullException(nameof(ticketId));

                ticketId = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spTicketUpdateStatus @id";

                    conn.Execute(sql,
                        new
                        {
                            id = ticketId,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return ticketId;
                });

                _signal.SignalToken(CACHE_TRANSACTION);

                return await Task.FromResult(ticketId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<long> UpdateTicket(long ticketId)
        {
            try
            {
                if (ticketId < 1)
                    throw new ArgumentNullException(nameof(ticketId));

                ticketId = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spTicketUpdate @id";

                    conn.Execute(sql,
                        new
                        {
                            id = ticketId,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return ticketId;
                });

                _signal.SignalToken(CACHE_TRANSACTION);

                return await Task.FromResult(ticketId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<long> UpdateTicketGameStat(long ticketId)
        {
            try
            {
                if (ticketId < 1)
                    throw new ArgumentNullException(nameof(ticketId));

                ticketId = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spTicketUpdateSentGameStat @id";

                    conn.Execute(sql,
                        new
                        {
                            id = ticketId,
                        }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return ticketId;
                });

                _signal.SignalToken(CACHE_TRANSACTION);
                return await Task.FromResult(ticketId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Ticket> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_TICKETS}.getbyid:{id}";
            var ticket = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TICKETS));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spCategoryGetById @id";
                    return conn.QueryFirstOrDefault<Ticket>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(ticket);
        }

       
        public async Task<long> AddMessage(MessageStore entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spMessageStoreInsert @emailAddress,@phoneNumber,@channel,@type,@addressType,@message,@imageUrl";

                    entity.Id = conn.ExecuteScalar<long>(sql, new
                    {
                        emailAddress = entity.EmailAddress,
                        phoneNumber = entity.PhoneNumber,
                        channel = (int)entity.MessageChannel,
                        type = (int)entity.MessageType,
                        addressType = (int)entity.AddressType,
                        message = entity.Message,
                        imageUrl = entity.ImageUrl
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_MESSAGESTORE);
                return await Task.FromResult(entity.Id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task<List<Ticket>> GetAllByUsername(string username = null)
        {
            try
            {
                string cacheKey = $"{CACHE_TICKETS}.GetAllByUsername.{username}";
                var tickets = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                {
                    entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                    entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TRANSACTION));
                    return _storage.UseConnection(conn =>
                    {
                        string sql = $"dbo.spTicketsReadAllByUsername @username";

                        var parameters = new
                        {
                            username
                        };
                        using (var multi = conn.QueryMultiple(sql, parameters, null,
                            commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS))
                        {
                            var query = multi.Read<Ticket>();
                            var summary = multi.ReadFirstOrDefault<dynamic>();

                            return query;

                        }
                    });
                });
                return await Task.FromResult(tickets.AsList());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Ticket> GetByRaffleIdTicketNumber(long id, int number)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

           
                var model = _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spTicketGetByRaffleIdTicketNumber @id,@number";
                    return conn.QueryFirstOrDefault<Ticket>(sql,
                        new { id, number },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
          
            return await Task.FromResult(model);
        }

        public async Task<Ticket> GetWinnerById(long id, long raffleId)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_TICKETS}.getwinnerbyid:{id}";
            var ticket = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_TICKETS));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spTicketGetWinnerById @id,@raffleId";
                    return conn.QueryFirstOrDefault<Ticket>(sql,
                        new { id, raffleId },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });

            return await Task.FromResult(ticket);
        }
    }

}
