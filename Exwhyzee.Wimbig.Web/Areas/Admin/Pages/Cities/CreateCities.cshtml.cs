using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Cities
{
    [Authorize(Roles = "Admin,mSuperAdmin")]
    public class CreateCitiesModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICityAppService _cityAppService;

        public CreateCitiesModel(ICityAppService cityAppService)
        {
            _cityAppService = cityAppService;
        }

        [BindProperty]
        public CityDto CityModelDto { get; set; }



        public string LoggedInUser { get; set; }

        public string ReturnUrl { get; set; }



        public async Task OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;





        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var city = new CityDto
                    {
                        Name = CityModelDto.Name

                    };


                    var mapId = await _cityAppService.Add(city);


                    return RedirectToPage("AllCities");
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}