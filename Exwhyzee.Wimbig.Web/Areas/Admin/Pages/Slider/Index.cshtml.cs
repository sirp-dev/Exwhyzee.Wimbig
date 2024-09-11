using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Barner;
using Exwhyzee.Wimbig.Core.BarnerImage;
using Exwhyzee.Wimbig.Core.SideBarner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Slider
{
    public class IndexModel : PageModel
    {
        private readonly IBarnerAppService _barnerAppService;
        private readonly IHostingEnvironment _hostingEnv;


        public IndexModel(IBarnerAppService barnerAppService, IHostingEnvironment env)
        {
            _barnerAppService = barnerAppService;
            _hostingEnv = env;
        }


        public PagedList<BarnerFile> Barners { get; set; }
        public PagedList<SideBarnerFile> SideBars { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Barners = await _barnerAppService.GetBarnerFile();
            SideBars = await _barnerAppService.GetBarnerFileSideBarner();
            return Page();
        }
    }
}