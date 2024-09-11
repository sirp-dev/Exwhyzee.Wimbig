using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Barner;
using Exwhyzee.Wimbig.Application.YoutubeLink;
using Exwhyzee.Wimbig.Application.YoutubeLink.Dto;
using Exwhyzee.Wimbig.Core.BarnerImage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.YoutubeLink
{
    public class NewModel : PageModel
    {
        private readonly IYoutubeLinkAppService _youtubeLinkAppService;
        private readonly IHostingEnvironment _hostingEnv;


        public NewModel(IYoutubeLinkAppService youtubeLinkAppService, IHostingEnvironment env)
        {
            _youtubeLinkAppService = youtubeLinkAppService;
            _hostingEnv = env;
        }
        [BindProperty]
        public YoutubeLinkDto YoutubeLinkDto { get; set; }



        public async Task OnGet(string returnUrl = null)
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        YoutubeLinkDto.DateCreated = DateTime.UtcNow.AddHours(1);
                        var info = await _youtubeLinkAppService.Add(YoutubeLinkDto);
                    }catch(Exception c)
                    {

                    }
                    return RedirectToPage("Index");
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Helper Agents
        private static string CurrentYear()
        {
            var currentYear = DateTime.Now;
            return currentYear.Year.ToString();
        }


        #endregion
    }
}