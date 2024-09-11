using Exwhyzee.Wimbig.Application.RaffleImages;
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
    public class WinnersCountViewComponent : ViewComponent
    {
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IHostingEnvironment hostingEnv;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public WinnersCountViewComponent(IMapImageToRaffleAppService mapImageToRaffleAppService, IHostingEnvironment hostingEnv, IPurchaseTicketAppService purchaseTicketAppService)
        {
            this.mapImageToRaffleAppService = mapImageToRaffleAppService;
            this.hostingEnv = hostingEnv;
            this.purchaseTicketAppService = purchaseTicketAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetWinnerAsync();
         
            return View(item);
        }
        

        private async Task<List<TicketDto>> GetWinnerAsync()
        {
            var ticket = await purchaseTicketAppService.GetAllWinners();


            return ticket.ToList();
        }

    }
}
