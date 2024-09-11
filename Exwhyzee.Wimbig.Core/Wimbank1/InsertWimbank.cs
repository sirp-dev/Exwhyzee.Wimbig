using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Wimbank
{
    public class InsertWimbank
    {
        public InsertWimbank()
        {
            DateOfTransaction = DateTime.UtcNow;
        }
      

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfTransaction { get; set; }        

        public EntityStatus Status { get; set; }

        public string Note { get; set; }
    }
}
