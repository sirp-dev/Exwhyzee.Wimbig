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
    public class IndexModel : PageModel
    {
        private IPayOutReportsAppService payOutReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICountAppService _countAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;



        public IndexModel(RoleManager<ApplicationRole> roleManager, IPurchaseTicketAppService purchaseTicketAppService, ICountAppService countAppService, IPayOutReportsAppService payOutReportsAppService, UserManager<ApplicationUser> userManger)
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

        public async Task<IActionResult> OnGet()
        {
            #region initial data. dont uncomment


            ////dgas and agents
            //var AgentRoleId = await _roleManager.FindByNameAsync("Agent");
            //var usersAgent = await _countAppService.UsersInRoleForDgaAgentSup(roleid: AgentRoleId.Id);
            //var usersA = usersAgent.Source;
            ////
            //var DgaRoleId = await _roleManager.FindByNameAsync("DGAs");
            //var usersDga = await _countAppService.UsersInRoleForDgaAgentSup(roleid: DgaRoleId.Id);
            //var usersD = usersDga.Source;

            //var userlist = usersA.Concat(usersD);


            //string format = "yyyy.MM.dd HH:mm:ss:ffff";

            //string startdatetime = "2018.12.17 00:00:00:0000";
            //string enddatetime = DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss:ffff");
            //DateTime startDate = DateTime.ParseExact(startdatetime, format, System.Globalization.CultureInfo.InvariantCulture);
            //DateTime endDate = DateTime.ParseExact(enddatetime, format, System.Globalization.CultureInfo.InvariantCulture);


            //TimeSpan diff = endDate - startDate;
            //int days = diff.Days;
            //for (var i = 0; i <= days; i++)
            //{
            //    var testDate = startDate.AddDays(i);
            //    TempData["datee"] = testDate.ToString("yyyy.MM.dd HH:mm:ss:ffff");
            //    string srt = TempData["datee"].ToString();
            //    lock (srt)
            //    {
            //    }
            //    DateTime st = DateTime.ParseExact(srt, format, System.Globalization.CultureInfo.InvariantCulture);

            //    switch (testDate.DayOfWeek)
            //    {

            //        case DayOfWeek.Sunday:
            //            //agent and dga
            //            foreach (var user in userlist)
            //            {



            //                Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
            //                int c = Tickes.Source.Count();

            //                var ticketbysundays = Tickes.Source.Where(a => a.Date.Date >= testDate.AddDays(-6) && a.Date.Date <= testDate);
            //                //foreach (var group in ticketbysundays)
            //                //{
            //                // Console.WriteLine("Period: {0}", group.Key);
            //                decimal sum = ticketbysundays.Sum(e => e.Price);
            //                if (sum != 0)
            //                {
            //                    decimal per = Convert.ToDecimal(0.1);
            //                    decimal percentageinterest = per * sum;

            //                    PayOutReportDto Report = new PayOutReportDto();
            //                    Report.Amount = sum;
            //                    Report.PercentageAmount = percentageinterest;
            //                    Report.Date = DateTime.UtcNow.AddHours(1);
            //                    Report.StartDate = testDate.AddDays(-6);
            //                    Report.EndDate = testDate;
            //                    Report.Note = "wimbig interest";
            //                    Report.Status = Enums.PayoutEnum.Pending;
            //                    Report.UserId = user.Id;
            //                    Report.Percentage = 10;
            //                    var add = await payOutReportsAppService.Add(Reportwww);
            //                }

            //            }


            //            break;
            //    }




            //}

            #endregion
            try
            {

                Reports = await payOutReportsAppService.GetAsync();
            }
            catch (Exception f)
            {

            }
            return Page();
        }

        public DateTime Weekly(DateTime day)
        {
            DateTime.Parse(day.ToString()).ToString("yyyy.MM.dd HH:mm:ss:ffff");
            string format = "yyyy.MM.dd HH:mm:ss:ffff";

            string eventdatetime = "2019.01.07 00:00:04:0000";
            DateTime x = DateTime.ParseExact(eventdatetime, format, System.Globalization.CultureInfo.InvariantCulture);
            int dayOfWeek = (int)x.DayOfWeek;
            DateTime nextSunday = x.AddDays(7 - dayOfWeek).Date;
            return nextSunday;
        }
    }
}