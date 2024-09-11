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
    public class PreviewBalanceViewComponent : ViewComponent
    {
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PreviewBalanceViewComponent(
            UserManager<ApplicationUser> userManager,
            IWalletAppService walletAppService)
        {
           
            _userManager = userManager;
            _walletAppService = walletAppService;

        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var item = await GetWinnerAsync(id);
         
            return View(item);
        }


        public WalletDto Wallet { get; set; }


        private async Task<WalletDto> GetWinnerAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
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
