using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.UserManagement.Pages.Users
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin,Agent,DGAs,Supervisors,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUser Person { get; set; }
 private readonly IWalletAppService _walletAppService;


        public DetailsModel(UserManager<ApplicationUser> userManger, IWalletAppService walletAppService)
        {
            _userManager = userManger;
            Person = new ApplicationUser();
            _walletAppService = walletAppService;
        }
       
      

        public decimal Balance { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            Person = await _userManager.FindByIdAsync(userId);
            var userbalance = await _walletAppService.GetWallet(userId);
            Balance = userbalance.Balance;

            if (Person == null)
            {
                return RedirectToPage("/Index", new  { message= "Error User Not Found"});
            }
        
            return Page();
        }
    }
}