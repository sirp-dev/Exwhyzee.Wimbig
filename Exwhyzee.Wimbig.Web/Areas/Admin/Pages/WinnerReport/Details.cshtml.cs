using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.WinnerReports;
using Exwhyzee.Wimbig.Application.WinnerReports.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.WinnerReport
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class DetailsModel : PageModel
    {
       
        private readonly IWinnerReportsAppService winnerReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public string LoggedInUser { get; set; }

        public DetailsModel(IRaffleAppService raffleAppService, UserManager<ApplicationUser> userManger,
            IWinnerReportsAppService winnerReportsAppService)
        {
            this.winnerReportsAppService = winnerReportsAppService;
            _userManager = userManger;
            
        }

        [BindProperty]
        public WinnerReportDto ReportDto { get; set; }

       

        //[TempData]
        //public string StatusMessage { get; private set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            LoggedInUser = _userManager.GetUserId(HttpContext.User);
            var entity = await winnerReportsAppService.Get(id);
            //var raffleImageid = await _mapImageToRaffleApp.GetById(raffle.Id);
          //  var raffleimage = await _imageFileAppService.GetById(raffleImageid.ImageId);
           
            if (entity == null)
            {
                return NotFound($"Unable to load raffle with the ID '{id}'.");
            }

            ReportDto = entity;

            return Page();
        }


       
    }
}