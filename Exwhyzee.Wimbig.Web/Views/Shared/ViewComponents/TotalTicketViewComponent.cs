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
    public class TotalTicketViewComponent : ViewComponent
    {
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IHostingEnvironment hostingEnv;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;

        private readonly UserManager<ApplicationUser> _userManager;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public TotalTicketViewComponent(UserManager<ApplicationUser> userManager
, IHostingEnvironment hostingEnv, IPurchaseTicketAppService purchaseTicketAppService)
        {
            _userManager = userManager;
            this.hostingEnv = hostingEnv;
            this.purchaseTicketAppService = purchaseTicketAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetWinnerAsync();
            TempData["count"] = item;
            return View();
        }



        //private async Task<List<TicketDto>> GetItemAsync()
        //{
        //    var ticket = await GetWinnerAsync();
        //    return ticket;
        //}


        private async Task<int> GetWinnerAsync()
        {
            int count = 0;
            try
            {
var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
            }
            //var ticket = await purchaseTicketAppService.GetAllTickets(searchString: user.UserName);

            var ticket = await purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
             
                 count = ticket.Source.Count();
            }catch(Exception c) { }

            return count;
        }

    }
}
