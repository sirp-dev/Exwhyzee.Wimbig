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
    [Authorize(Roles = "mSuperAdmin,Admin")]
    public class EditReportModel : PageModel
    {
       
        private readonly IWinnerReportsAppService winnerReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public string LoggedInUser { get; set; }

        public EditReportModel(IRaffleAppService raffleAppService, UserManager<ApplicationUser> userManger,
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
         

            ReportDto = new WinnerReportDto
            {
                WinnerName = entity.WinnerName,
                WinnerPhoneNumber = entity.WinnerPhoneNumber,
                WinnerEmail = entity.WinnerEmail,
                WinnerLocation = entity.WinnerLocation,
                AmountPlayed = entity.AmountPlayed,
                RaffleName = entity.RaffleName,
                RaffleId = entity.RaffleId,
                TicketNumber = entity.TicketNumber,
                ItemCost = entity.ItemCost,
               
                DeliveredBy = entity.DeliveredBy,
                DeliveredPhone = entity.DeliveredPhone,
                DeliveryAddress = entity.DeliveryAddress,
                TotalAmountPlayed = entity.TotalAmountPlayed,
               
                Status = entity.Status


            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var report = await winnerReportsAppService.Get(id);
            if (report == null)
            {
                return NotFound($"Unable to load Raffle with the ID '{id}'.");
            }

            report.WinnerName = ReportDto.WinnerName;
                report.WinnerPhoneNumber = ReportDto.WinnerPhoneNumber;
                report.WinnerEmail = ReportDto.WinnerEmail;
                report.WinnerLocation = ReportDto.WinnerLocation;
                report.AmountPlayed = ReportDto.AmountPlayed;
                report.RaffleName = ReportDto.RaffleName;
                report.RaffleId = ReportDto.RaffleId;
                report.TicketNumber = ReportDto.TicketNumber;
                report.ItemCost = ReportDto.ItemCost;
               
                report.DeliveredBy = ReportDto.DeliveredBy;
                report.DeliveredPhone = ReportDto.DeliveredPhone;
                report.DeliveryAddress = ReportDto.DeliveryAddress;
                report.TotalAmountPlayed = ReportDto.TotalAmountPlayed;

            report.Status = ReportDto.Status;

            

            await winnerReportsAppService.Update(report);

            //StatusMessage = "The Selected Raffle has been updated";
            return RedirectToPage("./Index");
        }

    }
}