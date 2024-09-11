using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class UpdateArchievedModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
      
        public string LoggedInUser { get; set; }

        public UpdateArchievedModel(IRaffleAppService raffleAppService)
        {
            _raffleAppService = raffleAppService;
           
        }

        [BindProperty]
        public RaffleDto Raffle { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
           
            var raffle = await _raffleAppService.GetById(id);
            if (raffle == null)
            {
                return NotFound($"Unable to load Raffle with the ID '{id}'.");
            }
            if(raffle.Archived == true)
            {
                await _raffleAppService.RemoveFromArchieve(raffle.Id);
            }
            else
            {
                await _raffleAppService.AddToArchieve(raffle.Id);
            }
           
           
           
            await _raffleAppService.Update(raffle);

            //StatusMessage = "The Selected Raffle has been updated";
            return RedirectToPage("./Index");
        }
    }
}