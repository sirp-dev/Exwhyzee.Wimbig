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

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.PayOutReport
{
    [Authorize]
    public class PayoutHistory : PageModel
    {
        private IPayOutReportsAppService payOutReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICountAppService _countAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;



        public PayoutHistory(RoleManager<ApplicationRole> roleManager, IPurchaseTicketAppService purchaseTicketAppService, ICountAppService countAppService, IPayOutReportsAppService payOutReportsAppService, UserManager<ApplicationUser> userManger)
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

        public async Task<IActionResult> OnGet(string id)
        {

          
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                TempData["username"] = user.UserName;
                TempData["id"] = user.Id;
                Reports = await payOutReportsAppService.GetAsync(searchString: id);
            }
            catch (Exception f)
            {

            }
            return Page();
        }

    }
}