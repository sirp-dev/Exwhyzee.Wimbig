using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.UserData
{
    [Authorize]
    public class PayoutHistoryModel : PageModel
    {
        private IPayOutReportsAppService payOutReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICountAppService _countAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;



        public PayoutHistoryModel(RoleManager<ApplicationRole> roleManager, IPurchaseTicketAppService purchaseTicketAppService, ICountAppService countAppService, IPayOutReportsAppService payOutReportsAppService, UserManager<ApplicationUser> userManger)
        {
            _userManager = userManger;
            this.payOutReportsAppService = payOutReportsAppService;
            _countAppService = countAppService;
            _roleManager = roleManager;
            _purchaseTicketAppService = purchaseTicketAppService;
        }
        public PagedList<PayOutReportDto> Reports { get; set; }
        public PayOutReportDto Reporti { get; set; }
        public PagedList<TicketDto> Tickes { get; set; }

        public async Task<IActionResult> OnGet(string status)
        {
            
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                if (status != null)
                {
                    if (status == "success")
                    {
                        TempData["msg"] = "Payout Successfull.";
                    }
                    else
                    {
                        TempData["error"] = "Payout Unsuccessfull. Try Again";
                    }
                }
                Reports = await payOutReportsAppService.GetAsync(searchString: user.Id);
            }
            catch (Exception f)
            {

            }
            return Page();
        }

       
    }
}