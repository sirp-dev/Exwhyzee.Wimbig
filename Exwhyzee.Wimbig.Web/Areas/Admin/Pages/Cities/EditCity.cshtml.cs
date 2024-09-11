using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Cities
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class EditCityModel : PageModel
    {
        private readonly ICityAppService _cityAppService;

        public EditCityModel(ICityAppService cityAppService)
        {
            _cityAppService = cityAppService;
        }

        [BindProperty]
        public CityDto EditCityModelDto { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {

            EditCityModelDto = await _cityAppService.Get(id);
           
            if (EditCityModelDto == null)
            {
                return NotFound($"Unable to load raffle with the ID '{id}'.");
            }
         

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var city = await _cityAppService.Get(id);
            if (city == null)
            {
                return NotFound($"Unable to load Raffle with the ID '{id}'.");
            }


            city.Name = EditCityModelDto.Name;

             await _cityAppService.Update(city);

            //StatusMessage = "The Selected Raffle has been updated";
            return RedirectToPage("./AllCities");
        }

    }
}