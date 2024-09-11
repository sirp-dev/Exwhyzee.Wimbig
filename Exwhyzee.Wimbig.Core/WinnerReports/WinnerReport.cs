using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.WinnerReports
{
    public class WinnerReport
    {
        public long Id { get; set; }
        public string WinnerName { get; set; }
        public string WinnerPhoneNumber { get; set; }
        public string WinnerEmail { get; set; }
        public string WinnerLocation { get; set; }
        public decimal AmountPlayed { get; set; }
        public string RaffleName { get; set; }
        public long RaffleId { get; set; }
        public int TicketNumber { get; set; }
        public decimal ItemCost { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateWon { get; set; }
        public DateTime DateDelivered { get; set; }
        public string DeliveredBy { get; set; }
        public string DeliveredPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal TotalAmountPlayed { get; set; }
        public string UserId { get; set; }

        public WinnerReportEnum Status { get; set; }
    }
}
