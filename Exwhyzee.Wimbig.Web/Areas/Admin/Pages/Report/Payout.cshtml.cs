using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Report
{
    public class PayoutModel : PageModel
    {

        private IPayOutReportsAppService payOutReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;


        public PayoutModel(

           RoleManager<ApplicationRole> roleManager, IPurchaseTicketAppService purchaseTicketAppService,
           IPayOutReportsAppService payOutReportsAppService,
           UserManager<ApplicationUser> userManger)
        {

            _userManager = userManger;
            this.payOutReportsAppService = payOutReportsAppService;
            _roleManager = roleManager;
            _purchaseTicketAppService = purchaseTicketAppService;

        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //get all report with date range above
            PagedList<TicketDto> Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
            int c = Tickes.Source.Count();

            var alltickets = Tickes.Source.Where(a => a.Date.Date >= DateTime.UtcNow.AddMonths(-3).Date && a.Date.Date <= DateTime.UtcNow.Date);
            ///

            DateTime start = DateTime.Now.Date;
            DateTime end = start.AddMonths(-3).Date;
            Week week = new Week(start);
            List<string> Ld = new List<string>();

            while (week.Start > end)
            {
                Ld.Add(week.ToString());
                week = week.GetNextWeek();
            }

            ///





            var today = DateTime.Now.Date; // This can be any date.2019-01-21 08:29:01.7377178

            var dd = today.DayOfWeek;

            var day = (int)today.DayOfWeek; //Number of the day in week. (0 - Sunday, 1 - Monday... and so On)

            var df = day;
            const int totalDaysOfWeek = 7;
           

            // Number of days in a week stays constant.
            List<string> L = new List<string>();

            for (var i = -day; i < -day + totalDaysOfWeek; i++)
            {
                L.Add(today.AddDays(i).Date.ToString());
            }

            return Page();
        }
    }
}