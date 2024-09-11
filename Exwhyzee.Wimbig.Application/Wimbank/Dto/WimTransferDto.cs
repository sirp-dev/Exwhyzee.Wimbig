using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Wimbank.Dto
{
   public class WimTransferDto
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string ReceiverId { get; set; }

        public decimal Amount { get; set; }

       
        public string Note { get; set; }

        public string ReceiverPhone { get; set; }
    }
}
