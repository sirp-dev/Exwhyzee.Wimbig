using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Raffles.Pages.Raffles
{
    public class ArchievedModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        

        public ArchievedModel(IRaffleAppService raffleAppService)
        {
            _raffleAppService = raffleAppService;
           
        }

        [TempData]
        public string StatusMessage { get; set; }
        public PagedList<RaffleDto> Raffles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Raffles = await _raffleAppService.GetRafflesByArchieved();

            return Page();
        }
    }
}