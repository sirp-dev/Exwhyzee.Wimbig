using Exwhyzee.Core;
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

        public decimal PricePerTicket { get; set; }

        public long HostedBy { get; set; }

        public DeliveryTypeEnum DeliveryType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EntityStatus Status { get; set; }

        public DateTime DateCreated { get; set; }
    }


}
