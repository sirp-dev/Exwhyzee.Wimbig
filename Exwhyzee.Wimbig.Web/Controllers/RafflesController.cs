using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Microsoft.AspNetCore.Mvc;

namespace Exwhyzee.Wimbig.Web.Controllers
{
    public class RafflesController : Controller
    {
        private readonly IRaffleAppService raffleAppService;

        public RafflesController(IRaffleAppService raffleAppService)
        {
            this.raffleAppService = raffleAppService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                throw new ArgumentNullException(nameof(id));

           var raffle = await raffleAppService.GetById(id);

            if (raffle == null)
                throw new ArgumentNullException(nameof(raffle));

            return View(raffle);
        }
    }
}