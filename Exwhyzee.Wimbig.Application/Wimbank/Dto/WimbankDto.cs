using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Wimbank.Dto
{
    public class WimbankDto
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public DateTime DateOfTransaction { get; set; }

        public TransactionTypeEnum TransactionStatus { get; set; }

        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string ReceiverId { get; set; }

        public string Note { get; set; }
    }
}
