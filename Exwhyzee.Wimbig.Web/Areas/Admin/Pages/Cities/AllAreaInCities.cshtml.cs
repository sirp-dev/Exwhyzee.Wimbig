
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Cities
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin,Admin")]

    public class AllAreaInCitiesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICityAppService _cityAppService;

        public AllAreaInCitiesModel(UserManager<ApplicationUser> userManager,
            ICityAppService cityAppService)
        {
            _userManager = userManager;
            _cityAppService = cityAppService;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public PagedList<AreaInCityDto> Cities { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {


            Cities = await _cityAppService.GetAreaInCityAsync();
            return Page();
        }
    }
}