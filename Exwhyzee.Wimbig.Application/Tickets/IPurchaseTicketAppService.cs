using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Tickets
{
    public interface IPurchaseTicketAppService
    {
        Task<long> ProcessTicket(TransactionDto transaction, WalletDto wallet, List<InsertTicketDto> tickets);

        Task<PagedList<TicketDto>> GetAllTickets(DateTime? dateStart = null, DateTime? dateEnd = null,
            int startIndex = 0, int count = int.MaxValue, string searchString = null, long? raffleId = null,
            bool? isWinner = null, bool? isSentToStat = null);
        Task<PagedList<TicketDto>> GetAllTicketsByTransactionId(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long? transactionId = null);

        Task<PagedList<TicketDto>> GetAllTicketsByReferenceId(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null, bool? isSentToStat = null);

        Task<PagedList<TicketDto>> GetAllTicketsByReferenceIdUser(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, string referenceId = null, bool? isSentToStat = null);

        Task<long> UpdateTicket(long ticketId);

        Task<long> UpdateTicketGameStat(long ticketId);

        Task<List<TicketDto>> GetAllWinners();

        Task<List<TicketDto>> GetAllByUsername(string username = null);

        Task<TicketDto> GetById(long id);

        Task<TicketDto> GetWinnerById(long id, long raffleId);

        Task<TicketDto> GetByRaffleIdTicketNumber(long id, int number);

        Task<long> Add(MessageStore entity);

    }
}
