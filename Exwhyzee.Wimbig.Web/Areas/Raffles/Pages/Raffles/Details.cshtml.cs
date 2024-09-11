using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
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
    public class DetailsModel : PageModel
    {
        private readonly IWalletAppService walletAppService;
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IRaffleAppService raffleAppService;
        private readonly UserManager<ApplicationUser> userManager;


        public DetailsModel(UserManager<ApplicationUser> userManager, IWalletAppService walletAppService, IMapImageToRaffleAppService mapImageToRaffleAppService, IRaffleAppService raffleAppService)
        {
            this.mapImageToRaffleAppService = mapImageToRaffleAppService;
            this.raffleAppService = raffleAppService;
            this.walletAppService = walletAppService;
            this.userManager = userManager;
        }

        [BindProperty]
        public RaffleDto Raffle { get; set; }

        [BindProperty]
        public List<ImageOfARaffle> Images { get; set; }
        public WalletDto Wallet { get; set; }

        //[Authorize]
        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id));

            Raffle = await raffleAppService.GetById(id);

            if (Raffle == null)
                throw new ArgumentNullException(nameof(Raffle));

            var images = await mapImageToRaffleAppService.GetAllImagesOfARaffle(id);
           
            Images = images.ToList();
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
                //return RedirectToPage("Login", "Raffles", new { area = "Identity" });
                //return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Wallet = await walletAppService.GetWallet(user.Id);
            
            return Page();
        }
    }
}