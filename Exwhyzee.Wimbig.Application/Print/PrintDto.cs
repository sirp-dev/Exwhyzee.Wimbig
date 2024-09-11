using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Print
{
  public class PrintDto
    {

       
        public string Phone { get; set; }
       
        public DateTime TransactionDate { get; set; }
       
        public string Tickets { get; set; }
       
        public string RaffleName { get; set; }
       
        public long RaffleNumber { get; set; }
       
        public long TransactionId { get; set; }
       
        public decimal Amount { get; set; }

       
        public string AgentName { get; set; }
    }
}
