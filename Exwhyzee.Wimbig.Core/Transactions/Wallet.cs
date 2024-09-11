using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Transactions
{
    public class Wallet
    {
        public Wallet()
        {
            Balance = 0;
            DateUpdated = DateTime.UtcNow.AddHours(1);
        }
        public long Id { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
