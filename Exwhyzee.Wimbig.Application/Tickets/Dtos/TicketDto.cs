using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Tickets.Dtos
{
    public class TicketDto
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public long RaffleId { get; set; }

        public string RaffleName { get; set; }

        public string RaffleNumber {

            get
            {
               
                var number = DateTime.UtcNow.AddHours(1).ToString("ddMMyyyyHHmmss") + Id;
                return number.ToString();
            }


        }

        public string TicketNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public long TransactionId { get; set; }

        public decimal Price { get; set; }

        public bool IsWinner { get; set; }

        public DateTime? DateWon { get; set; }


        public TicketStatusEnum TicketStatus { get; set; }

        public bool IsSentToStat { get; set; }
        
        public string PlayerName { get; set; }

        public DateTime Date { get; set; }

        public int Status { get; set; }

        public bool PaidOut { get; set; }


        public string CurrentLocation { get; set; }
        public decimal WalletBal { get; set; }

    }
}
