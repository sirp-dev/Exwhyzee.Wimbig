using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class AdminPayoutActiveViewComponent : ViewComponent
    {
        private readonly IPayOutReportsAppService _payOutReportsAppService;


        private readonly UserManager<ApplicationUser> _userManager;


        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";

        public AdminPayoutActiveViewComponent(UserManager<ApplicationUser> userManager
, IPayOutReportsAppService payOutReportsAppService)
        {
            _userManager = userManager;
            _payOutReportsAppService = payOutReportsAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
 var data = await _payOutReportsAppService.PayOutReportGetLastRecord();
            var allrangedata = await _payOutReportsAppService.GetAsync(dateStart: data.StartDate, dateEnd: data.EndDate);
            var sum = allrangedata.Source.Sum(x => x.PercentageAmount);

            TempData["date"] = data.EndDate.Value.ToString("dddd dd MMM, yyyy");
            TempData["sum"] = sum.ToString();
            }catch(Exception c)
            {
                TempData["date"] = "00/00/0000";
                TempData["sum"] = "0";
            }
           
            return View();
        }

        //private async Task<decimal> ReportAsync()
        //{
        //    int count = 0;
        //    try
        //    {
        //        var user = await _userManager.GetUserAsync(HttpContext.User);
        //        if (user == null)
        //        {
        //        }
               
        //        var data = await _payOutReportsAppService.PayOutReportGetLastRecord();
        //        var allrangedata = await _payOutReportsAppService.GetAsync(dateStart: data.StartDate, dateEnd: data.EndDate);
        //        var sum = allrangedata.Source.Sum(x => x.PercentageAmount);

        //        return sum;
        //    }
        //    catch (Exception c) { }

        //    return 0;
        //}


    }
}
