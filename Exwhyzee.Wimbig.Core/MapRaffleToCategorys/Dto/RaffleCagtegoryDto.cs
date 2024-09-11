using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.MapRaffleToCategorys.Dto
{
   public class RaffleCagtegoryDto
   {
        public long Id { get; set; }
        public long RaffleId { get; set; }

        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime DateCreated { get; set; }

        /// Raffle Properties

        public string RaffleName { get; set; }

        public string Description { get; set; }

        public int NumberOfTickets { get; set; }

        public decimal PricePerTicket { get; set; }

        public string HostedBy { get; set; }

        public DeliveryTypeEnum DeliveryType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EntityStatus Status { get; set; }

        public DateTime RaffleDateCreated { get; set; }
    }
}
