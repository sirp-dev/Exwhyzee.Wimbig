using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.SuperAdmin.Pages.Dashboard
{
    public class StakesModel : PageModel
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletAppService _walletAppService;


        public StakesModel(IPurchaseTicketAppService purchaseTicketAppService, IWalletAppService walletAppService, UserManager<ApplicationUser> userManager)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
            _userManager = userManager;
            _walletAppService = walletAppService;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public PagedList<TicketDto> Tickets { get; set; }
        public List<TicketDto> Listing { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {

            Tickets = await _purchaseTicketAppService.GetAllTickets();

            
            return Page();
    }
       
    }
}