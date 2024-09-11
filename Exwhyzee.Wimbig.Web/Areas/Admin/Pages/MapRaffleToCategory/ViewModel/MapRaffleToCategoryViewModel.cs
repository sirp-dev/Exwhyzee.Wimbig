using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Sections.Dto;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.MapRaffleToCategorys.ViewModel
{
    public class MapRaffleToCategoryViewModel
    {
        public long? SectionId { get; set; }
        public IEnumerable<SectionDto> Sections { get; set; }

        public long CategoryId { get; set; }
        public IEnumerable<CategorySectionDetailsDto> Categorys { get; set; }

        public long RaffleId { get; set; }
        public IEnumerable<RaffleDto> Raffles { get; set; }
    }
}
