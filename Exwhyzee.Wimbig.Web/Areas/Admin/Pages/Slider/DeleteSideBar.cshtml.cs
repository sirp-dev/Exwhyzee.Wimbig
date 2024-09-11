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
    public class DeleteSideBarModel : PageModel
    {
        private readonly IBarnerAppService _barnerAppService;
        private readonly IHostingEnvironment _hostingEnv;


        public DeleteSideBarModel(IBarnerAppService barnerAppService, IHostingEnvironment env)
        {
            _barnerAppService = barnerAppService;
            _hostingEnv = env;
        }


        public BarnerFile Barners { get; set; }
        public SideBarnerFile SideBars { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            try
            {
                await _barnerAppService.DeleteSideBarner(Id: id);

            }
            catch (Exception e)
            {

            }
            return Redirect("/Admin/Slider/Index");
        }
    }
}