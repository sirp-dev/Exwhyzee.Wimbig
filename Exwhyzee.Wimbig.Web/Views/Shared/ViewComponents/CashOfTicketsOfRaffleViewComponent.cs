using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class CashOfTicketsOfRaffleViewComponent : ViewComponent
    {
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IHostingEnvironment hostingEnv;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;

        private readonly UserManager<ApplicationUser> _userManager;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public CashOfTicketsOfRaffleViewComponent(UserManager<ApplicationUser> userManager
, IHostingEnvironment hostingEnv, IPurchaseTicketAppService purchaseTicketAppService)
        {
            _userManager = userManager;
            this.hostingEnv = hostingEnv;
            this.purchaseTicketAppService = purchaseTicketAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(long raffleId)
        {
            var item = await GetWinnerAsync(raffleId);
            TempData["casheq"] = item;
            return View();
        }


        private async Task<decimal> GetWinnerAsync(long raffleId)
        {

            var ticket = await purchaseTicketAppService.GetAllTickets(raffleId: raffleId);
            var ticketprice = ticket.Source.Select(x => x.Price).Sum();
            return ticketprice;
        }

    }
}
