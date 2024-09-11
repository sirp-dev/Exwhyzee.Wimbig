using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    public class WinnerListModel : PageModel
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;

        public WinnerListModel(IPurchaseTicketAppService purchaseTicketAppService)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
        }

        public List<TicketDto> Tickets { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Tickets = await _purchaseTicketAppService.GetAllWinners();
            return Page();
        }
    }
}