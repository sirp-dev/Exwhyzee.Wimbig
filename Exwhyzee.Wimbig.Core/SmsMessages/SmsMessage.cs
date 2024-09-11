using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.SmsMessages
{
   public class SmsMessage
    {
        public long Id { get; set; }

        public string SenderId { get; set; }

        public DateTime DateCreated { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }


        public SmsStatusEnum Status { get; set; }

        public string Response { get; set; }

      
    }
}
