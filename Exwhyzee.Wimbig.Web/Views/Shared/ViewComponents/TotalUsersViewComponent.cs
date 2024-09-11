
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class TotalUsersViewComponent : ViewComponent
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IWalletAppService _walletAppService;



        public TotalUsersViewComponent(
            UserManager<ApplicationUser> userManager,
            IWalletAppService walletAppService,
            SignInManager<ApplicationUser> signInManager,
                        RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _walletAppService = walletAppService;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var item = await GetWinnerAsync();
            var items = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
            TempData["countusers"] = items.Count();
            return View();
        }


        public WalletDto Wallet { get; set; }


        //private async Task<List<ApplicationUser>> GetUsersAsync()
        //{
        //    var user = _userManager.GetUserAsync();
        //    if (user == null)
        //    {
        //        }
          
        //    return user.t;
        //}

    }
}
