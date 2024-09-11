using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Sections;
using Exwhyzee.Wimbig.Application.Sections.Dto;
using Exwhyzee.Wimbig.Core.Sections;
using Exwhyzee.Wimbig.Web.Areas.Admin.Pages.CategoryManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.CategoryManagement
{
    public class CreateCategoryModel : PageModel
    {
        private readonly ICategoryAppService _categoryService;
        private readonly ISectionAppService _sectionAppService;
        public CreateCategoryModel(ICategoryAppService categoryService, ISectionAppService sectionAppService)
        {
            _categoryService = categoryService;
            _sectionAppService = sectionAppService;
        }

        [BindProperty]
        public CreateCategoryViewModel CreateCategoryVM{ get; set; }

        public string LoggedInUser { get; set; }
       
        public string ReturnUrl { get; set; }

        public List<SectionDto> Sections { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["Error"] = "";
            this.ReturnUrl = returnUrl;
            var sections = await _sectionAppService.GetAsync();
           
            if(sections.Source== null)
                ViewData["Error"] = "No Sections at the time. Please create a section first before creating a category";

            Sections = sections.Source.ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (CreateCategoryVM != null)
                {
                    returnUrl = returnUrl ?? Url.Content("~/");
                    CreateCategoryVM.CreateCategoryDto.EntityStatus = Enums.EntityStatus.Active;


                    var insertedId = await _categoryService.Add(CreateCategoryVM.CreateCategoryDto);
                    if (insertedId > 0)
                    {
                        ViewData["Error"] = "";
                        if (returnUrl != null)
                            //return RedirectToPage(returnUrl);
                      
                        return RedirectToPage("Index");
                    }
                }

                ViewData["Error"] = "Sorry verify that your entry is correct and try again";
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Sorry an Error ocurred, please try again";
                return Page();

            }
        }
    }
}
