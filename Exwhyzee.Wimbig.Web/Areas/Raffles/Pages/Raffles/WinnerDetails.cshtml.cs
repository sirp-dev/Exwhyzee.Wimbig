using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Raffles.Pages.Raffles
{
    public class WinnerDetailsModel : PageModel
    {
        private readonly IWalletAppService walletAppService;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;
        private readonly IRaffleAppService raffleAppService;
        private readonly UserManager<ApplicationUser> userManager;


        public WinnerDetailsModel(UserManager<ApplicationUser> userManager, IWalletAppService walletAppService, IPurchaseTicketAppService purchaseTicketAppService, IRaffleAppService raffleAppService)
        {
            this.purchaseTicketAppService = purchaseTicketAppService;
            this.raffleAppService = raffleAppService;
            this.walletAppService = walletAppService;
            this.userManager = userManager;
        }

        [BindProperty]
        public TicketDto Ticket { get; set; }


        //[Authorize]
        public async Task<IActionResult> OnGetAsync(long id, long raffleId)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id));

            Ticket = await purchaseTicketAppService.GetWinnerById(id, raffleId);

            if (Ticket == null)
                throw new ArgumentNullException(nameof(Ticket));

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
                //return RedirectToPage("Login", "Raffles", new { area = "Identity" });
                //return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            return Page();
        }
    }
}