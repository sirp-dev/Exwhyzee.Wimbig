
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin,Admin")]

    public class AllTicketsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;

        public AllTicketsModel(UserManager<ApplicationUser> userManager,
            IPurchaseTicketAppService purchaseTicketAppService)
        {
            _userManager = userManager;
            _purchaseTicketAppService = purchaseTicketAppService;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public PagedList<TicketDto> Tickes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {


            Tickes = await _purchaseTicketAppService.GetAllTickets();
            return Page();
        }
    }
}