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
    public class HomeUserViewComponent : ViewComponent
    {
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeUserViewComponent(
            UserManager<ApplicationUser> userManager,
            IWalletAppService walletAppService)
        {

            _userManager = userManager;
            _walletAppService = walletAppService;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var item = await GetWinnerAsync();
                ViewData["balance"] = "Balance: N "+item.Balance+".00";
                ViewData["space"] = "|";
                var user = await _userManager.GetUserAsync(HttpContext.User);
                string name = "";
                if (user.OtherNames == null)
                {
                   name = user.FirstName.ToUpper() + " " + user.LastName.ToUpper();
                }
                else
                {
                    name = user.FirstName.ToUpper() + " " + user.LastName.ToUpper() + " " + user.OtherNames.ToUpper();
                }
                ViewData["user"] = name;
                ViewData["username"] = user.UserName;
            }
            catch (Exception u)
            {
                ViewData["balance"] = "";
                ViewData["space"] = "";
                ViewData["user"] = "";
                ViewData["username"] = "";
            }

            return View();
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
