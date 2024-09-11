using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Tickets.Dtos
{
    public class InsertTicketDto
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public long RaffleId { get; set; }

        public string TicketNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal Price { get; set; }

        public long TransactionId { get; set; }

        public string YourPhoneNumber { get; set; }

        public TicketStatusEnum TicketStatus { get; set; }
        public string Email { get; set; }
        public string PlayerName { get; set; }

        public string CurrentLocation { get; set; }

    }
}
