using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
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
    public class RafflePercentViewComponent : ViewComponent
    {
        private readonly IRaffleAppService raffleAppService;
       
        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public RafflePercentViewComponent(IRaffleAppService raffleAppService)
        {
            this.raffleAppService = raffleAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(long raffleId)
        {
            var item = await GetItemAsync(raffleId);

            TempData["percentage"] = item;

          
            return View();
        }
        

        private async Task<string> GetItemAsync(long raffleId)
        {
            var item = await raffleAppService.GetById(raffleId);
            if(item == null)
            {
                return "";
            }

            decimal division = Convert.ToDecimal(item.TotalSold) / Convert.ToDecimal(item.NumberOfTickets);
            decimal progressinpercent = division * 100;
            int i = (int)Math.Round(progressinpercent, 0);
            return i.ToString();
        }




    }
}
