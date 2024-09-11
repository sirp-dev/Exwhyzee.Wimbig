using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Cities
{
    public class AreaInCity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
    }
}
