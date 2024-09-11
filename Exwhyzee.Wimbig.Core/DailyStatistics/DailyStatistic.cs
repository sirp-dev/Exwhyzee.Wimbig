using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.DailyStatistics
{
   public class DailyStatistic
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalUsers { get; set; }
        public int TotalTickets { get; set; }
        public int TotalRaffle { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalWalletCash { get; set; }
    }
}
