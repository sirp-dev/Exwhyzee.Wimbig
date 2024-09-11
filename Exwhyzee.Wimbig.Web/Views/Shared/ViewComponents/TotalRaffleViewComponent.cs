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
    public class TotalRaffleViewComponent:ViewComponent
    {
       
        private readonly IRaffleAppService raffleAppService;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public TotalRaffleViewComponent(IRaffleAppService raffleAppService)
        {
            this.raffleAppService = raffleAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetAsync();
         
            return View(item);
        }

    
        private async Task<List<Raffle>> GetAsync()
        {
            var ticket = await raffleAppService.GetAllRaffles();


            return ticket.Source.ToList();
        }

    }
}
