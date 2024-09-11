using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Sections;
using Exwhyzee.Wimbig.Application.Sections.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.SectionManagement
{
    public class IndexModel : PageModel
    { 
        public readonly ISectionAppService _sectionAppService;
        public List<SectionDto> Sections { get; set; } = new List<SectionDto>();

        public IndexModel(ISectionAppService sectionAppService)
        {
            _sectionAppService = sectionAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var listOfSection = await _sectionAppService.GetAsync();
            if (listOfSection != null)
                Sections = listOfSection.Source.ToList();

            return Page();
        }
    }
}