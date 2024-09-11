using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Category
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private ICategoryAppService _categoryAppService;
        public IndexModel (ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }
        public PagedList<CategorySectionDetailsDto> Categories { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Categories = await _categoryAppService.GetAsync();
            return Page();
        }

    }
}