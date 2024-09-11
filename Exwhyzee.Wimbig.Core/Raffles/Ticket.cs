using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Raffles
{
    public class Ticket
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public long RaffleId { get; set; }

        public string RaffleName { get; set; }

        public string RaffleNumber { get; set; }

        public string TicketNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public long TransactionId { get; set; }

        public decimal Price { get; set; }

        public bool IsWinner { get; set; }

        public bool IsSentToStat { get; set; }

        public string YourPhoneNumber { get; set; }


        public TicketStatusEnum TicketStatus { get; set; }

        public DateTime? DateWon { get; set; }
        public string PlayerName { get; set; }

        public DateTime Date { get; set; }

        public int Status { get; set; }

        public bool PaidOut { get; set; }

        public string CurrentLocation { get; set; }


    }
}
