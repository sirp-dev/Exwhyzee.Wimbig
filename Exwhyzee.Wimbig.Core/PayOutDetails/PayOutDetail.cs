using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.PayOutDetails
{
    public class PayOutDetail
    {
        public long Id { get; set; }
        public int Percentage { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string UserId { get; set; }
    }
}
