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
    public class TicketsModel : PageModel
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;



        public TicketsModel(IPurchaseTicketAppService purchaseTicketAppService)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public PagedList<TicketDto> Tickets { get; set; }


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id));
            if(id == null)
            {
                Tickets = await _purchaseTicketAppService.GetAllTickets();

            }
            else
            {
                Tickets = await _purchaseTicketAppService.GetAllTickets(raffleId: id);

            }
            
            if (Tickets == null)
                throw new ArgumentNullException(nameof(Tickets));


            return Page();
        }

    }
}