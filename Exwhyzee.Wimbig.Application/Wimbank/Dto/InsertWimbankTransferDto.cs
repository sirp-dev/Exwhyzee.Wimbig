using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Wimbank.Dto
{
   public class InsertWimbankTransferDto
    {

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
