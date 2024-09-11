using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.MapRaffleToCategorys;
using Exwhyzee.Wimbig.Application.MapRaffleToCategorys.Dtos;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Sections;
using Exwhyzee.Wimbig.Web.Areas.Admin.Pages.MapRaffleToCategorys.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.MapRaffleToCategorys
{
    public class MapRaffleToCategoryModel : PageModel
    {
        private readonly IMapRaffleToCategoryAppService _mapRaffleToCategoryAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IRaffleAppService _raffleAppService;
        private readonly ISectionAppService _sectionAppService;

        public MapRaffleToCategoryModel(IMapRaffleToCategoryAppService mapRaffleToCategoryAppService, ICategoryAppService categoryAppService,IRaffleAppService raffleAppService, ISectionAppService sectionAppService)
        {
            _mapRaffleToCategoryAppService = mapRaffleToCategoryAppService;
            _categoryAppService = categoryAppService;
            _raffleAppService = raffleAppService;
            _sectionAppService = sectionAppService;
        }

        [BindProperty]
        public MapRaffleToCategoryViewModel MapRaffleToCategoryVM { get; set; }

        public MapRaffleToCategoryDto mapRaffleToCategoryDto { get; set; }


        public async  Task OnGet()
        {
            string msgSection, msgCategory, msgRaffle;

            var getSections = await _sectionAppService.GetAsync();

            // TODO: on two way data binding, get category by sectionId selected
            var getCategorys = await _categoryAppService.GetAsync();

            // TODO: adjust to get all raffles not mapped
            var getraffles = await _raffleAppService.GetAll();

            MapRaffleToCategoryVM.Sections = getSections.Source.AsEnumerable();
            msgSection = getSections.Source == null ? "Sections is empty" : "";

            MapRaffleToCategoryVM.Categorys = getCategorys.Source.AsEnumerable();
            msgCategory = getCategorys.Source == null ? " No Category found, There needs to be category before mapping raffle" : "";

            MapRaffleToCategoryVM.Raffles = getraffles.Source.AsEnumerable();
            msgRaffle = getraffles.Source == null ? " No Raffles that has been mapped ito this category" : "";

            ViewData["Error"] = $"{msgSection}, {msgCategory}, {msgRaffle}";
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {            
            return Page();
        }
    }
}