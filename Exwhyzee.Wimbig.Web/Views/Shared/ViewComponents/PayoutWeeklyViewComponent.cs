using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class PayoutWeeklyViewComponent : ViewComponent
    {
        private readonly IPayOutReportsAppService _payOutReportsAppService;


        private readonly UserManager<ApplicationUser> _userManager;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public PayoutWeeklyViewComponent(UserManager<ApplicationUser> userManager
, IPayOutReportsAppService payOutReportsAppService)
        {
            _userManager = userManager;
            _payOutReportsAppService = payOutReportsAppService;
        }

      
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var item = await ReportAsync(id);

            return View(item);
        }

        private async Task<PayOutReportDto> ReportAsync(string id)
        {
            int count = 0;
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                }
                //var ticket = await purchaseTicketAppService.GetAllTickets(searchString: user.UserName);

                var data = await _payOutReportsAppService.PayOutReportGetLastRecordByUserId(userId: id);

                return data;
            }
            catch (Exception c) { }

            return null;
        }

    }
}
