using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.WimBank
{
   public class WimbankTransfer
    {
        public long Id { get; set; }

        public string UserId { get; set; }
        public string ReceiverId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfTransaction { get; set; }

        public TransactionTypeEnum TransactionStatus { get; set; }

        public string Username { get; set; }

        public string Note { get; set; }

        public string ReceiverPhone { get; set; }

    }
}
