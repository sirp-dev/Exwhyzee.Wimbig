using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Main.Pages.Raffles
{
    public class IndexModel : PageModel
    {
        private IRaffleAppService _raffleAppService;

        public IndexModel(IRaffleAppService raffleAppService)
        {
            _raffleAppService = raffleAppService;
        }


        public PagedList<RaffleDto> Raffles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Raffles = await _raffleAppService.GetAll();

            return Page();
        }

       
    }
}