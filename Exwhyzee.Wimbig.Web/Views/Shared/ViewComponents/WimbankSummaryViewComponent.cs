using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Wimbank;
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
    public class WimbankSummaryViewComponent : ViewComponent
    {
        private readonly IHostingEnvironment hostingEnv;
        private readonly ICountAppService countAppService;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public WimbankSummaryViewComponent(ICountAppService countAppService, IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
            this.countAppService = countAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetWinnerAsync();

            ViewData["item"] = item;
            return View();
        }
        

        private async Task<decimal> GetWinnerAsync()
        {
            var ticket = await countAppService.Amount();

            return ticket;
        }

    }
}
