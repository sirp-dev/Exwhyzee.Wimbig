using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.LotteryCommision.Pages.Dashboard
{
    public class WinnersModel : PageModel
    {

        private readonly IPurchaseTicketAppService _purchaseTicketAppService;



        public WinnersModel(IPurchaseTicketAppService purchaseTicketAppService)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public List<TicketDto> Tickets { get; set; }


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                Tickets = await _purchaseTicketAppService.GetAllWinners();
            }
            else
            {
                Tickets = await _purchaseTicketAppService.GetAllWinners();

            }
            if (Tickets == null)
                throw new ArgumentNullException(nameof(Tickets));


            return Page();
        }
    }
}