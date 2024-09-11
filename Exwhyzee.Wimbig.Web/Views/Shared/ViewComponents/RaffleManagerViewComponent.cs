using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class RaffleManagerViewComponent : ViewComponent
    {
       
        private readonly IRaffleAppService raffleAppService;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public RaffleManagerViewComponent(IRaffleAppService raffleAppService)
        {
            this.raffleAppService = raffleAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var raffle = await raffleAppService.GetAllRaffles();
            var raffleActive = raffle.Source.Where(x => x.Status == Enums.EntityStatus.Active).Count();
            var raffleDrawn = raffle.Source.Where(x => x.Status == Enums.EntityStatus.Drawn).Count();
            var raffleTargetLocation = raffle.Source.Where(x => x.Location != null).Count();
            var raffleOthers = raffle.Source.Where(x => x.Status != Enums.EntityStatus.Active && x.Status != Enums.EntityStatus.Drawn).Count();
            TempData["Active"] = raffleActive;
            TempData["Drawn"] = raffleDrawn;
            TempData["Others"] = raffleOthers;
            TempData["TargetLocation"] = raffleTargetLocation;
            TempData["Total"] = raffle.Source.Count();
            return View();
        }

    
    }
}
