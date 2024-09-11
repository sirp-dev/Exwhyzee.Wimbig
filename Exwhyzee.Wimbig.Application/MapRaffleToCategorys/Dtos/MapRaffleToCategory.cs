using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.MapRaffleToCategorys.Dtos
{
    public class MapRaffleToCategoryDto
    {
        public long Id { get; set; }
        public long RaffleId { get; set; }
        public string RaffleName { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);
    }
}
