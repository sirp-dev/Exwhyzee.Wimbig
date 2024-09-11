using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.MapImagesToRaffle
{
    public class MapImagesToRaffle
    {
        public long MapImagesToRaffleId { get; set; }
        public long RaffleId { get; set; }
        public long ImageId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
