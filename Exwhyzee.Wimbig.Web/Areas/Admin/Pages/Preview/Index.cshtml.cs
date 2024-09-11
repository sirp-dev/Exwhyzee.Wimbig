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

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Preview
{
    [Authorize(Roles = "Admin,mSuperAdmin")]
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

        [BindProperty]
        public string LoggedInUser { get; set; }

        public List<RaffleDto> Raffles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                TempData["userid"] = id;
                var userinfo = await _userManager.FindByIdAsync(id);

                TempData["username"] = userinfo.UserName + " (" + userinfo.CurrentCity + ")";


                if (userinfo != null)
                {
                    TempData["amUser"] = await _userManager.IsInRoleAsync(userinfo, "User");


                    if (await _userManager.IsInRoleAsync(userinfo, "DGAs") == true || await _userManager.IsInRoleAsync(userinfo, "Agent") == true)
                    {

                       
                        var userbalance = await _walletAppService.GetWallet(userinfo.Id);
                        Balance = userbalance.Balance;
                        var raffles = await _raffleAppService.GetAll();
                        var checkraffles = raffles.Source.ToList();
                        var myRaffles = checkraffles.Where(x => x.Location == userinfo.CurrentCity && x.Status == Enums.EntityStatus.Active).OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToList();
                        var myRafflesByCity = raffles.Source.Where(x => x.Location == userinfo.CurrentCity).Select(x => x.Location).ToList();
                        var countRaffles = myRaffles.Count();
                        if (countRaffles < 12)
                        {
                            int remender = 12 - countRaffles;
                            var remaingRafflecheck = checkraffles.Where(x => !myRafflesByCity.Contains(x.Location) && x.Status == Enums.EntityStatus.Active).OrderBy(x => x.SortOrder).ThenBy(x => x.Id);
                            var remainigRafflesfirst = remaingRafflecheck.Where(x => x.Location != "Global" || string.IsNullOrEmpty(x.Location)).ToList();
                            var remainigRaffles = remainigRafflesfirst.Where(x => string.IsNullOrEmpty(x.Location)).Take(remender).ToList();

                            Raffles = myRaffles.Concat(remainigRaffles).ToList();
                        }
                        else
                        {
                            Raffles = myRaffles.OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToList();
                        }

                        Raffles = Raffles.Take(12).ToList();
                        return Page();
                    }
                    else if (await _userManager.IsInRoleAsync(userinfo, "User") == true)
                    {
                        var rafflesitem = await _raffleAppService.GetAll(count: 24, status: 1);
                        var raffles = rafflesitem.Source.Where(x => x.Location == null || x.Location == "Global").OrderBy(x => x.SortOrder).ToList();
                        Raffles = raffles.Take(12).ToList();
                        return Page();
                    }
                    else if (await _userManager.IsInRoleAsync(userinfo, "Supervisors") == true)
                    {
                        return NotFound($"Unable to load supervisor.");
                    }
                }
                else
                {


                }
            }catch(Exception c)
            {
                return Redirect("/Identity/Account/Login");
            }
            return null;
        }
    }
}