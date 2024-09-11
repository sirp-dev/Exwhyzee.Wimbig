using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers.Dtos.PuchaseTickerDtos
{
    public class PurchaseTicketDto
    {
        public TransactionDto Transaction { get; set; }

        public WalletDto Wallet { get; set; }

        public List<InsertTicketDto> Tickets { get; set; }
    }
}
