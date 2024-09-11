using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Transactions.Dto
{
    public class WalletDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
