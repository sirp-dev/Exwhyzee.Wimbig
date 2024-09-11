using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class TotalModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;

        public TotalModel(IRaffleAppService raffleAppService)
        {
            _raffleAppService = raffleAppService;
        }

        public PagedList<Raffle> Raffles { get; set; }
        List<Raffle> raffles = new List<Raffle>();

        public async Task<IActionResult> OnGetAsync()
        {
            Raffles = await _raffleAppService.GetAllRaffles();
            
            return Page();
        }

    }
}