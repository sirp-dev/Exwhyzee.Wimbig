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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Cities
{
    [Authorize(Roles = "Admin,mSuperAdmin")]
    public class CreateAreaInCitiesModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICityAppService _cityAppService;

        public CreateAreaInCitiesModel(ICityAppService cityAppService)
        {
            _cityAppService = cityAppService;
        }

        [BindProperty]
        public AreaInCityDto AreaInCityModelDto { get; set; }

        [BindProperty]
        public List<SelectListItem> CityDtoList { get; set; }
        public PagedList<CityDto> CityDto { get; set; }


        public string LoggedInUser { get; set; }

        public string ReturnUrl { get; set; }



        public async Task OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            
            List<CityDto> city = new List<CityDto>();

            var query = await _cityAppService.GetAsync();

            city.AddRange(query.Source.Select(entity => new CityDto()
            {
                Id = entity.Id,
                Name = entity.Name


            }));
            //CityDtoList = CityDto.Source.ToList();
           
            CityDtoList = city.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Name
                                }).ToList();

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var city = new AreaInCityDto
                    {
                        Name = AreaInCityModelDto.Name,
                        
                        CityId = AreaInCityModelDto.CityId
                    };


                    var mapId = await _cityAppService.AddAreaInCity(city);


                    return RedirectToPage("AllAreaInCities");
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