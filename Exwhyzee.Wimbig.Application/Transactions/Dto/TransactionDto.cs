using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Transactions.Dto
{
    public class TransactionDto
    {
        public long Id { get; set; }

        public long WalletId { get; set; }

        public string UserId { get; set; }

        public string Sender { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfTransaction { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }

        public EntityStatus Status { get; set; }

        public string Username { get; set; }

        public string TransactionReference { get; set; }

        public string Description { get; set; }
    }
}
