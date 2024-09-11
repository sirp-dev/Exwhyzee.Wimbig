using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.WinnerReports;
using Exwhyzee.Wimbig.Application.WinnerReports.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Sections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.WinnerReport
{
    public class NewReportModel : PageModel
    {
        private readonly IWinnerReportsAppService winnerReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRaffleAppService _raffleAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;

        public NewReportModel(IWinnerReportsAppService winnerReportsAppService, IPurchaseTicketAppService _purchaseTicketAppService, UserManager<ApplicationUser> userManager, IRaffleAppService _raffleAppService)
        {
            this.winnerReportsAppService = winnerReportsAppService;
            _userManager = userManager;
            this._raffleAppService = _raffleAppService;
            this._purchaseTicketAppService = _purchaseTicketAppService;
        }

        [BindProperty]
        public WinnerReportDto ReportDto { get; set; }

        public string LoggedInUser { get; set; }
       
        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["Error"] = "";
            this.ReturnUrl = returnUrl;
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (ReportDto != null)
                {

                    var checkraffle = await _raffleAppService.GetById(ReportDto.RaffleId);
                   
                    if(checkraffle.Status != Enums.EntityStatus.Drawn)
                    {
                        TempData["status"] = "Raffle has not been Drawn. check the Raffle Id and try again";
                        return Page();
                    }
                    var winner = await _purchaseTicketAppService.GetByRaffleIdTicketNumber(ReportDto.RaffleId, ReportDto.TicketNumber);
                    var tickets = await _purchaseTicketAppService.GetAllTickets(raffleId: ReportDto.RaffleId);
                    var ticketsumprice = tickets.Source.Sum(x => x.Price);

                    var userid = User.Identity.Name;
                    var loginUser = await _userManager.FindByNameAsync(userid);
                    ReportDto.Status = Enums.WinnerReportEnum.Pending;
                    ReportDto.DateCreated = DateTime.UtcNow.AddHours(1);
                    ReportDto.RaffleId = winner.RaffleId;
                    ReportDto.RaffleName = winner.RaffleName;
                    ReportDto.TicketNumber = Convert.ToInt32(winner.TicketNumber);
                    ReportDto.WinnerEmail = winner.Email;
                    ReportDto.WinnerName = winner.PlayerName;
                    ReportDto.WinnerPhoneNumber = winner.PhoneNumber;
                    ReportDto.WinnerEmail = winner.Email;
                    ReportDto.AmountPlayed = winner.Price;
                    ReportDto.TotalAmountPlayed = ticketsumprice;
                    ReportDto.UserId = loginUser.Id;



                    var insertedId = await winnerReportsAppService.Add(ReportDto);
                    if (insertedId > 0)
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
                ViewData["Error"] = "Sorry an Error ocurred, check the Raffle Id or please try again";
                return Page();

            }
        }
    }
}
