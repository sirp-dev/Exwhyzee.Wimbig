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
    public class UserIdViewComponent : ViewComponent
    {
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserIdViewComponent(
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
                var userinfo = await _userManager.GetUserAsync(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userinfo.Id);

                ViewData["user"] = user.UniqueId;
            }
            catch (Exception u)
            {
              
                ViewData["user"] = "";
            }

            return View();
        }


       

    }
}
