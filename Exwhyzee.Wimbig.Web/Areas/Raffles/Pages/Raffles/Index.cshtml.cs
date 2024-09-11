using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Raffles.Pages.Raffles
{
    public class IndexModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        private readonly UserManager<ApplicationUser> _userManager;


        public IndexModel(IRaffleAppService raffleAppService, UserManager<ApplicationUser> userManager)
        {
            _raffleAppService = raffleAppService;
            _userManager = userManager;

        }

        [TempData]
        public string StatusMessage { get; set; }
        public List<RaffleDto> Raffles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var Rafflesitem = await _raffleAppService.GetAll(count: 50, status: 1);
                Raffles = Rafflesitem.Source.Where(x => x.Location == null || x.Location == "Global").OrderBy(x => x.SortOrder).ToList();
                if(Raffles.Count() > 18)
                {
Raffles = Raffles.Take(18).ToList();
                }
                else
                {
                    Raffles = Raffles.ToList();

                }
            }
            return Page();
        }
    }
}