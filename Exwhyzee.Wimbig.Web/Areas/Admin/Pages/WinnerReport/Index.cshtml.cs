using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.WinnerReports;
using Exwhyzee.Wimbig.Application.WinnerReports.Dto;
using Exwhyzee.Wimbig.Core.WinnerReports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.WinnerReport
{
    public class IndexModel : PageModel
    {
        private readonly IWinnerReportsAppService winnerReportsAppService;
        private readonly IHostingEnvironment _hostingEnv;


        public IndexModel(IWinnerReportsAppService winnerReportsAppService, IHostingEnvironment env)
        {
            this.winnerReportsAppService = winnerReportsAppService;
            _hostingEnv = env;
        }


        public PagedList<WinnerReportDto> Report { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Report = await winnerReportsAppService.GetAsync();
            return Page();
        }
    }
}