using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.MapImagesToRaffles
{
    public class MapImageToRaffle
    {
        public long Id { get; set; }
        public long RaffleId { get; set; }
        public long ImageId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
