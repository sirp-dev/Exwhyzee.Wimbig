using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
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
    public class RaffleDashboardViewComponent : ViewComponent
    {
        private readonly IRaffleAppService raffleAppService;

        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public RaffleDashboardViewComponent(IRaffleAppService raffleAppService)
        {
            this.raffleAppService = raffleAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(long raffleId)
        {
            var item = await GetItemAsync();

            return View(item);
        }
        public List<RaffleDto> Raffle { get; private set; }

       
        private async Task<List<RaffleDto>> GetItemAsync()
        {
            var item = await raffleAppService.GetAllRaffles();
            var list = (from raffleitem in item.Source
                        select new RaffleDto
                        {
                            RaffleName = raffleitem.Name,
                            NumberOfTickets = raffleitem.NumberOfTickets,
                            TotalSold = raffleitem.TotalSold,
                            Status = raffleitem.Status,
                             Percentage = percentage(raffleitem.TotalSold, raffleitem.NumberOfTickets)
        }).ToList();

            Raffle = list.Where(x=>x.Status == EntityStatus.Active).OrderByDescending(x=>x.Percentage).Take(5).ToList();
            

            return Raffle;

        }

        private int percentage(int totalsold, int numberTickes)
        {
            decimal division = Convert.ToDecimal(totalsold) / Convert.ToDecimal(numberTickes);
            decimal progressinpercent = division * 100;
            int i = (int)Math.Round(progressinpercent, 0);
            return i;
        }

    private async Task<string> GetItemTotalSold(long raffleId)
    {
        var item = await raffleAppService.GetById(raffleId);
        if (item == null)
        {
            return "";
        }


        return item.TotalSold.ToString();
    }



}
}
