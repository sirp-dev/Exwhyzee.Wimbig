using Exwhyzee.Enums;
using System;

namespace Exwhyzee.Wimbig.Core.Raffles
{
    public class Raffle
    {
        public Raffle(){}

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NumberOfTickets { get; set; }

        public int TotalSold { get; set; }

        public decimal PricePerTicket { get; set; }

        public string HostedBy { get; set; }

        public DeliveryTypeEnum DeliveryType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EntityStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        public string Username { get; set; }

        public DateTime? DateWon { get; set; }

        public int SortOrder { get; set; }

        public bool Archived { get; set; }

        public bool PaidOut { get; set; }

        public string Location { get; set; }

        public string AreaInCity { get; set; }

    }


}
