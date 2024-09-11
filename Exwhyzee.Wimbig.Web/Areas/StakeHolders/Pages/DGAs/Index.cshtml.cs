using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.DGAs
{
    [Authorize(Roles = "DGAs,mSuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;


        public IndexModel(IRaffleAppService raffleAppService, IWalletAppService walletAppService, UserManager<ApplicationUser> userManger)
        {
            _raffleAppService = raffleAppService;
            _walletAppService = walletAppService;
            _userManager = userManger;
        }
        public decimal Balance { get; set; }


        public string LoggedInUser { get; set; }

        public List<RaffleDto> Raffles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            LoggedInUser = User.Identity.Name;
            var userinfo = await _userManager.FindByNameAsync(LoggedInUser);
            //if (string.IsNullOrEmpty(userinfo.ContactAddress))
            //{
            //    return Redirect("/Identity/Account/Manage/UpdateProfile");
            //}

            if (userinfo != null)
            {

                var userbalance = await _walletAppService.GetWallet(userinfo.Id);
                Balance = userbalance.Balance;
                var raffles = await _raffleAppService.GetAll();
                var checkraffles = raffles.Source.ToList();
               
                    var myRaffles = checkraffles.Where(x => x.AreaInCity == userinfo.AreaInCurrentCity && x.Status == Enums.EntityStatus.Active).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToList();
                    var myRafflesByCity = raffles.Source.Where(x => x.Location == userinfo.AreaInCurrentCity).Select(x => x.Location).ToList();
                    var countRaffles = myRaffles.Count();
                    if (countRaffles < 15)
                    {
                        int remender = 15 - countRaffles;
                        var remaingRafflecheck = checkraffles.Where(x => !myRafflesByCity.Contains(x.Location) && x.Status == Enums.EntityStatus.Active).OrderBy(x => x.SortOrder).ThenBy(x => x.Id);
                        var remainigRafflesfirst = remaingRafflecheck.Where(x => x.Location != "Global" || string.IsNullOrEmpty(x.Location)).ToList();
                        var remainigRaffles = remainigRafflesfirst.Where(x => string.IsNullOrEmpty(x.Location)).Take(remender).ToList();

                        Raffles = myRaffles.Concat(remainigRaffles).ToList();
                    }
                    else
                    {
                        Raffles = myRaffles.OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToList();
                    }
                
               
                Raffles = Raffles.Take(15).ToList();
                return Page();
            }
            else
            {

                return Redirect("/Identity/Account/Login");
            }
        }
    }
}