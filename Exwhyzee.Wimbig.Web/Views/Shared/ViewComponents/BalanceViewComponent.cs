using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class BalanceViewComponent : ViewComponent
    {
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BalanceViewComponent(
            UserManager<ApplicationUser> userManager,
            IWalletAppService walletAppService)
        {
           
            _userManager = userManager;
            _walletAppService = walletAppService;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetWinnerAsync();
         
            return View(item);
        }


        public WalletDto Wallet { get; set; }


        private async Task<WalletDto> GetWinnerAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                }
            try
            {
                Wallet = await _walletAppService.GetWallet(user.Id);
            }
            catch (Exception e)
            {

            }
            return Wallet;
        }

    }
}
