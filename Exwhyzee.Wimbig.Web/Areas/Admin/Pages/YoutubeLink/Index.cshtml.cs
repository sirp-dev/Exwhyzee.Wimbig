using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Barner;
using Exwhyzee.Wimbig.Application.YoutubeLink;
using Exwhyzee.Wimbig.Application.YoutubeLink.Dto;
using Exwhyzee.Wimbig.Core.BarnerImage;
using Exwhyzee.Wimbig.Core.SideBarner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.YoutubeLink
{
    public class IndexModel : PageModel
    {
        private readonly IYoutubeLinkAppService _youtubeLinkAppService;
        private readonly IHostingEnvironment _hostingEnv;


        public IndexModel(IYoutubeLinkAppService youtubeLinkAppService, IHostingEnvironment env)
        {
            _youtubeLinkAppService = youtubeLinkAppService;
            _hostingEnv = env;
        }


        public PagedList<YoutubeLinkDto> Youtube { get; set; }
       
        public async Task<IActionResult> OnGetAsync()
        {
            Youtube = await _youtubeLinkAppService.GetAsync();
            return Page();
        }
    }
}