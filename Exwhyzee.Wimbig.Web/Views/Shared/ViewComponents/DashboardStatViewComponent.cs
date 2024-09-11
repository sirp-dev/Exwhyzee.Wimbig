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
    public class DashboardStatViewComponent:ViewComponent
    {
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IHostingEnvironment hostingEnv;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public DashboardStatViewComponent(IMapImageToRaffleAppService mapImageToRaffleAppService, IHostingEnvironment hostingEnv, IPurchaseTicketAppService purchaseTicketAppService)
        {
            this.mapImageToRaffleAppService = mapImageToRaffleAppService;
            this.hostingEnv = hostingEnv;
            this.purchaseTicketAppService = purchaseTicketAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(long raffleId)
        {
            var item = 10;
         
            return View(item);
        }

       

        //private async Task<int> GetRaffleCOunt(string userId)
        //{
        //    var images = await purchaseTicketAppService.get.GetAllImagesOfARaffle(raffleId, 1);

        //    if(images.Count() < 1)
        //    {
        //        var path = Path.Combine(hostingEnv.WebRootPath, MainFolder,ImageFolder);
        //        return new ImageOfARaffle
        //        {
        //            DateCreated = DateTime.Now,
        //            Extension = ".png",
        //            Id = raffleId,
        //            ImageId = 0,
        //            RaffleId = raffleId,
        //            Url = $"/main/wimbig/wimbig_drum_2.png"
        //        };
        //        //return null;
        //    }
        //    return images.FirstOrDefault();
        //}


   
    }
}
