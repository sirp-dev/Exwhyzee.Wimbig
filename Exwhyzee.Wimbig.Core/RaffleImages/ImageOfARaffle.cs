using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.RaffleImages
{
    public class ImageOfARaffle
    {
        public long Id { get; set; }
        public long RaffleId { get; set; }
        public long ImageId { get; set; }

        public DateTime DateCreated { get; set; }

        // image properties with id = imageId
        public string Url { get; set; }
        public string Extension { get; set; }

    }
}
