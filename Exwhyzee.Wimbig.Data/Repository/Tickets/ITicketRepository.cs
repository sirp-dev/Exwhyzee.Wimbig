using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.Raffles;
using Exwhyzee.Wimbig.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Tickets
{
    public interface ITicketRepository
    {
        Task<Transaction> CreateTicket(List<InsertTicket> tickets, Wallet wallet, Transaction transaction);

        Task<PagedList<Ticket>> GetAllTickets(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long? raffleId = null, bool? isWinner = null, bool? isSentToStat = null);
        Task<PagedList<Ticket>> GetAllTicketsByTransactionId(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long? transactionId = null);

        Task<PagedList<Ticket>> GetAllTicketsByReferenceId(DateTime? dateStart = null, DateTime? dateEnd = null,
       int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null,
       bool? isWinner = null, bool? isSentToStat = null);

        Task<PagedList<Ticket>> GetAllTicketsByReferenceIdUsers(DateTime? dateStart = null, DateTime? dateEnd = null,
  int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null,
  bool? isWinner = null, bool? isSentToStat = null);

        Task<long> UpdateTicket(long ticketId);

        Task<long> UpdateTicketStatus(long ticketId);

        Task<long> UpdateTicketGameStat(long ticketId);



        Task<List<Ticket>> GetAllWinners();

        Task<List<Ticket>> GetAllByUsername(string username = null);
        Task<Ticket> Get(long id);

        Task<Ticket> GetWinnerById(long id, long raffleId);

        Task<Ticket> GetByRaffleIdTicketNumber(long id, int number);

        Task<long> AddMessage(MessageStore entity);
    }
}
