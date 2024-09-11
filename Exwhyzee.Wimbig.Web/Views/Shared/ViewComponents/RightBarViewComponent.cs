using Exwhyzee.Wimbig.Application.Barner;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.BarnerImage;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Core.Raffles;
using Exwhyzee.Wimbig.Core.SideBarner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class RightBarViewComponent : ViewComponent
    {
        private readonly IBarnerAppService _barnerAppService;
        private readonly IHostingEnvironment _hostingEnv;


        public RightBarViewComponent(IBarnerAppService barnerAppService, IHostingEnvironment env)
        {
            _barnerAppService = barnerAppService;
            _hostingEnv = env;
        }


        public PagedList<BarnerFile> Barners { get; set; }
        public PagedList<SideBarnerFile> SideBars { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetBarner();
            var itemsmall = await GetSmallBarner();

            if (itemsmall.FirstOrDefault() != null)
            {
                ViewBag.small = itemsmall.FirstOrDefault().Url;

            }
            return View(item);
        }

    

        private async Task<List<SideBarnerFile>> GetBarner()
        {
            var ticket = await _barnerAppService.GetBarnerFileSideBarner();


            return ticket.Source.Where(x=>x.TargetLocation == "Side Bar Big").Take(2).ToList();
        }

        private async Task<List<SideBarnerFile>> GetSmallBarner()
        {
            var ticket = await _barnerAppService.GetBarnerFileSideBarner();


            return ticket.Source.Where(x => x.TargetLocation == "Side Bar Small").Take(1).ToList();
        }

    }
}
