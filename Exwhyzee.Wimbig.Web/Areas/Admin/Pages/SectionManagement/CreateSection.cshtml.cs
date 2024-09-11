using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Sections;
using Exwhyzee.Wimbig.Application.Sections.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.SectionManagement
{
    public class CreateSectionModel : PageModel
    {
        private readonly ISectionAppService _sectionAppService;
        public CreateSectionModel(ISectionAppService sectionAppService)
        {
            _sectionAppService = sectionAppService;
        }

        [BindProperty]
        public CreateSectionDto CreateSectionDto { get; set; }

        public string LoggedInUser { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ViewData["Error"] = "";
            this.ReturnUrl = returnUrl;

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (CreateSectionDto != null)
                {                 
                    returnUrl = returnUrl ?? Url.Content("~/");
                    CreateSectionDto.EntityStatus = Enums.EntityStatus.Active;
                    CreateSectionDto.DateCreated = DateTime.UtcNow.AddHours(1);

                    var insertedId = await _sectionAppService.Add(CreateSectionDto);
                    if(insertedId > 0)
                    {
                        
                        ViewData["Error"] = "";
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