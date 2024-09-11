using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Cities.Dto
{
    public class AreaInCityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }

    }
}
