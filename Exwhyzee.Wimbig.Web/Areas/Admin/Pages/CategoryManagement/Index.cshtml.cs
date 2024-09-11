using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.CategoryManagement
{
    public class IndexModel : PageModel
    {
        public readonly ICategoryAppService _categoryAppService;
        public List<CategorySectionDetailsDto> Categories { get; set; } = new List<CategorySectionDetailsDto>();

        public IndexModel(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var listOfCategories = await _categoryAppService.GetAsync();
            if (listOfCategories != null)
                Categories = listOfCategories.Source.ToList();

            return Page();
        }
      
    }
}