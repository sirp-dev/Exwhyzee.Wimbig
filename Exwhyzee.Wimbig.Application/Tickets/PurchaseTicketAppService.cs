using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.Raffles;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Data.Repository.Raffles;
using Exwhyzee.Wimbig.Data.Repository.Tickets;


namespace Exwhyzee.Wimbig.Application.Tickets
{
    public class PurchaseTicketAppService : IPurchaseTicketAppService
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IRaffleRepository raffleRepository;

        public PurchaseTicketAppService(ITicketRepository ticketRepository, IRaffleRepository raffleRepository)
        {
            this.ticketRepository = ticketRepository;
            this.raffleRepository = raffleRepository;
        }

        public async Task<PagedList<TicketDto>> GetAllTickets(DateTime? dateStart = null, DateTime? dateEnd = null,
            int startIndex = 0, int count = int.MaxValue, string searchString = null, long? raffleId = null,
            bool? isWinner = null, bool? isSentToStat = null)
        {
            List<TicketDto> tickets = new List<TicketDto>();

            var query = await ticketRepository.GetAllTickets(dateStart, dateEnd, startIndex, count, searchString, raffleId, isWinner, isSentToStat);

            tickets.AddRange(query.Source.Select(x => new TicketDto()
            {
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.YourPhoneNumber ?? x.PhoneNumber,
                Price = x.Price,
                PurchaseDate = x.PurchaseDate,
                RaffleId = x.RaffleId,
                RaffleName = x.RaffleName,
                TicketNumber = x.TicketNumber,
                TransactionId = x.TransactionId,
                UserId = x.UserId,
                UserName = x.UserName,
                IsWinner = x.IsWinner,
                IsSentToStat = x.IsSentToStat,
                Date = x.Date,
                Status = x.Status,
                PlayerName = x.PlayerName,
                PaidOut = x.PaidOut,
                CurrentLocation = x.CurrentLocation
            }));

            return new PagedList<TicketDto>(source: tickets.ToList(), pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        //get ticket by reference Id

        public async Task<PagedList<TicketDto>> GetAllTicketsByReferenceId(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null, bool? isSentToStat = null)
        {
            List<TicketDto> tickets = new List<TicketDto>();

            var query = await ticketRepository.GetAllTicketsByReferenceId(dateStart, dateEnd, startIndex, count, searchString, referenceId, isSentToStat);

            tickets.AddRange(query.Source.Select(x => new TicketDto()
            {
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.YourPhoneNumber ?? x.PhoneNumber,
                Price = x.Price,
                PurchaseDate = x.PurchaseDate,
                RaffleId = x.RaffleId,
                RaffleName = x.RaffleName,
                TicketNumber = x.TicketNumber,
                TransactionId = x.TransactionId,
                UserId = x.UserId,
                UserName = x.UserName,
                IsWinner = x.IsWinner,
                IsSentToStat = x.IsSentToStat,
                Date = x.Date,
                Status = x.Status,
                PlayerName = x.PlayerName,
                PaidOut = x.PaidOut
            }));

            return new PagedList<TicketDto>(source: tickets.ToList(), pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }

        //end refernce
        public async Task<PagedList<TicketDto>> GetAllTicketsByTransactionId(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long? transactionId = null)
        {
            List<TicketDto> tickets = new List<TicketDto>();

            var query = await ticketRepository.GetAllTicketsByTransactionId(dateStart, dateEnd, startIndex, count, searchString, transactionId);

            tickets.AddRange(query.Source.Select(x => new TicketDto()
            {
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.YourPhoneNumber ?? x.PhoneNumber,
                Price = x.Price,
                PurchaseDate = x.PurchaseDate,
                RaffleId = x.RaffleId,
                RaffleName = x.RaffleName,
                TicketNumber = x.TicketNumber,
                TransactionId = x.TransactionId,
                UserId = x.UserId,
                UserName = x.UserName,
                IsWinner = x.IsWinner,
                IsSentToStat = x.IsSentToStat,
                PlayerName = x.PlayerName,
                TicketStatus = x.TicketStatus,
                PaidOut = x.PaidOut
            }));

            return new PagedList<TicketDto>(source: tickets.ToList(), pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }

        public async Task<List<TicketDto>> GetAllWinners()
        {
            List<TicketDto> tickets = new List<TicketDto>();

            var query = await ticketRepository.GetAllWinners();

            tickets.AddRange(query.Select(x => new TicketDto()
            {
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.YourPhoneNumber ?? x.PhoneNumber,
                Price = x.Price,
                PurchaseDate = x.PurchaseDate,
                RaffleId = x.RaffleId,
                RaffleName = x.RaffleName,
                TicketNumber = x.TicketNumber,
                TransactionId = x.TransactionId,
                UserId = x.UserId,
                UserName = x.UserName,
                IsWinner = x.IsWinner,
                IsSentToStat = x.IsSentToStat,
                DateWon = x.DateWon,
                PlayerName = x.PlayerName,
                CurrentLocation = x.CurrentLocation
            }));

            return tickets.OrderByDescending(c=>c.DateWon).ToList();
        }

        public async Task<long> ProcessTicket(TransactionDto transaction, WalletDto wallet, List<InsertTicketDto> tickets)
        {
            var transactionData = new Transaction
            {
                Id = transaction.Id,
                Username = transaction.Username,
                Amount = transaction.Amount,
                DateOfTransaction = transaction.DateOfTransaction,
                Status = transaction.Status,
                TransactionType = transaction.TransactionType,
                UserId = transaction.UserId,
                WalletId = transaction.WalletId,
                TransactionReference = transaction.TransactionReference,
                Description = transaction.Description

            };

            var walletData = new Wallet
            {
                Balance = wallet.Balance,
                DateUpdated = wallet.DateUpdated,
                UserId = wallet.UserId,
                Id = wallet.Id
            };

            var ticketsData = new List<InsertTicket>();

            foreach (var item in tickets)
            {
                ticketsData.Add(new InsertTicket
                {
                    Id = item.Id,
                    Price = item.Price,
                    PurchaseDate = item.PurchaseDate,
                    RaffleId = item.RaffleId,
                    TicketNumber = item.TicketNumber,
                    TransactionId = item.TransactionId,
                    UserId = item.UserId,
                    YourPhoneNumber = item.YourPhoneNumber,
                    TicketStatus = Enums.TicketStatusEnum.Active,
                    Email = item.Email,
                    PlayerName = item.PlayerName,
                    CurrentLocation = item.CurrentLocation
                });

            }
            var result = await ticketRepository.CreateTicket(ticketsData, walletData, transactionData);
            return result.Id;
        }

        public async Task<long> UpdateTicket(long ticketId)
        {
            var result = await ticketRepository.UpdateTicket(ticketId);
            
           

                return result;
        }

        public async Task<long> UpdateTicketGameStat(long ticketId)
        {
            return await ticketRepository.UpdateTicketGameStat(ticketId);
        }

        public async Task<TicketDto> GetById(long id)
        {
            var data = await ticketRepository.Get(id);

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var ticketDto = new TicketDto
            {
                Email = data.Email,
                FullName = data.FullName,
                Id = data.Id,
                PhoneNumber = data.YourPhoneNumber ?? data.PhoneNumber,
                Price = data.Price,
                PurchaseDate = data.PurchaseDate,
                RaffleId = data.RaffleId,
                RaffleName = data.RaffleName,
                TicketNumber = data.TicketNumber,
                TransactionId = data.TransactionId,
                UserId = data.UserId,
                UserName = data.UserName,
                IsWinner = data.IsWinner,
                IsSentToStat = data.IsSentToStat,
                PlayerName = data.PlayerName,
                CurrentLocation = data.CurrentLocation
            };

            return ticketDto;
        }

        public async Task<long> Add(MessageStore entity)
        {

            entity.ImageUrl = "https://" + entity.ImageUrl;
            var item = await ticketRepository.AddMessage(entity);
            return item;

        }

        public async Task<List<TicketDto>> GetAllByUsername(string username = null)
        {
            List<TicketDto> tickets = new List<TicketDto>();

            var query = await ticketRepository.GetAllByUsername(username: username);

            tickets.AddRange(query.Select(x => new TicketDto()
            {
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.YourPhoneNumber ?? x.PhoneNumber,
                Price = x.Price,
                PurchaseDate = x.PurchaseDate,
                RaffleId = x.RaffleId,
                RaffleName = x.RaffleName,
                TicketNumber = x.TicketNumber,
                TransactionId = x.TransactionId,
                UserId = x.UserId,
                UserName = x.UserName,
                IsWinner = x.IsWinner,
                IsSentToStat = x.IsSentToStat,
                DateWon = x.DateWon,
                PlayerName = x.PlayerName,
                CurrentLocation = x.CurrentLocation
            }));

            return tickets.ToList();
        }

        public async Task<TicketDto> GetByRaffleIdTicketNumber(long id, int number)
        {
            var data = await ticketRepository.GetByRaffleIdTicketNumber(id, number);

            if (data == null)
            {
                return null;
            }
            else
            {

                var ticketDto = new TicketDto
                {
                    Email = data.Email,
                    Id = data.Id,
                    PhoneNumber = data.YourPhoneNumber,
                    Price = data.Price,
                    PurchaseDate = data.PurchaseDate,
                    RaffleId = data.RaffleId,
                    RaffleName = data.RaffleName,
                    TicketNumber = data.TicketNumber,
                    TransactionId = data.TransactionId,
                    UserId = data.UserId,
                    IsWinner = data.IsWinner,
                    IsSentToStat = data.IsSentToStat,
                    PlayerName = data.PlayerName, 
                    CurrentLocation = data.CurrentLocation
                };

                return ticketDto;
            }
        }

        public async Task<PagedList<TicketDto>> GetAllTicketsByReferenceIdUser(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null, bool? isSentToStat = null)
        {
            List<TicketDto> tickets = new List<TicketDto>();

            var query = await ticketRepository.GetAllTicketsByReferenceIdUsers(dateStart, dateEnd, startIndex, count, searchString, referenceId, isSentToStat);

            tickets.AddRange(query.Source.Select(x => new TicketDto()
            {
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.YourPhoneNumber ?? x.PhoneNumber,
                Price = x.Price,
                PurchaseDate = x.PurchaseDate,
                RaffleId = x.RaffleId,
                RaffleName = x.RaffleName,
                TicketNumber = x.TicketNumber,
                TransactionId = x.TransactionId,
                UserId = x.UserId,
                UserName = x.UserName,
                IsWinner = x.IsWinner,
                IsSentToStat = x.IsSentToStat,
                Date = x.Date,
                Status = x.Status,
                PlayerName = x.PlayerName,
                PaidOut = x.PaidOut,
                CurrentLocation = x.CurrentLocation
            }));

            return new PagedList<TicketDto>(source: tickets.ToList(), pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }

        public async Task<TicketDto> GetWinnerById(long id, long raffleId)
        {
            var data = await ticketRepository.GetWinnerById(id, raffleId);

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var ticketDto = new TicketDto
            {
                Email = data.Email,
                FullName = data.FullName,
                Id = data.Id,
                PhoneNumber = data.YourPhoneNumber ?? data.PhoneNumber,
                Price = data.Price,
                PurchaseDate = data.PurchaseDate,
                RaffleId = data.RaffleId,
                RaffleName = data.RaffleName,
                TicketNumber = data.TicketNumber,
                TransactionId = data.TransactionId,
                UserId = data.UserId,
                UserName = data.UserName,
                IsWinner = data.IsWinner,
                IsSentToStat = data.IsSentToStat,
                PlayerName = data.PlayerName, 
                CurrentLocation = data.CurrentLocation
            };

            return ticketDto;
        }
    }
}
