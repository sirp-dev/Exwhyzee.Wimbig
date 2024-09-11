using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.WimBank
{
   public class InsertWimbankTransfer
    {
        public InsertWimbankTransfer()
        {
            DateOfTransaction = DateTime.UtcNow.AddHours(1);
        }


        public string UserId { get; set; }
        public string ReceiverId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfTransaction { get; set; }

        public TransactionTypeEnum Status { get; set; }

        public string Note { get; set; }
        public string ReceiverPhone { get; set; }
    }
}
