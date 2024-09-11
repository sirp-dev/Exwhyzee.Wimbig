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
    public class UserNameViewComponent : ViewComponent
    {
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserNameViewComponent(
            UserManager<ApplicationUser> userManager,
            IWalletAppService walletAppService)
        {

            _userManager = userManager;
            _walletAppService = walletAppService;

        }

        public async Task<IViewComponentResult> InvokeAsync(string userid)
        {
            try
            {
               
                var user = await _userManager.FindByIdAsync(userid);

                ViewData["user"] = user.UserName;
            }
            catch (Exception u)
            {
              
                ViewData["user"] = "";
            }

            return View();
        }


       

    }
}
