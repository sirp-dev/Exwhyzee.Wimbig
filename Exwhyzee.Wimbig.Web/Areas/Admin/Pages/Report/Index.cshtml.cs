using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Exwhyzee.Wimbig.Application.DailyStatistics;
using Exwhyzee.Wimbig.Application.DailyStatistics.Dto;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Report
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private IDailyStatisticsAppService _dailyStatisticsAppService;
        public IndexModel (IDailyStatisticsAppService dailyStatisticsAppService)
        {
            _dailyStatisticsAppService = dailyStatisticsAppService;
        }
        public PagedList<DailyStatisticsDto> Reports { get; set; }
        public DailyStatisticsDto Report { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Reports = await _dailyStatisticsAppService.GetAsync();
            return Page();
        }

    }
}