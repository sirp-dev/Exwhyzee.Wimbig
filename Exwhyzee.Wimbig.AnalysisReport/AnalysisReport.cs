using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;

namespace Exwhyzee.Wimbig.AnalysisReport
{
    public class AnalysisReport : IMicroService
    {
        private IMicroServiceController _controller;
        private IPayOutReportsAppService payOutReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICountAppService _countAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;


        private Timer timer = new Timer(20000);

        public AnalysisReport(IMicroServiceController controller,
           RoleManager<ApplicationRole> roleManager, IPurchaseTicketAppService purchaseTicketAppService, ICountAppService countAppService,
           IPayOutReportsAppService payOutReportsAppService,
           UserManager<ApplicationUser> userManger)
        {
            _controller = controller;
            _userManager = userManger;
            this.payOutReportsAppService = payOutReportsAppService;
            _countAppService = countAppService;
            _roleManager = roleManager;
            _purchaseTicketAppService = purchaseTicketAppService;

        }


        public void Start()
        {
            Console.WriteLine("check if last payout is on sunday");


            timer.Elapsed += GetLastPayoutDate;
            timer.Start();
        }

        private async void GetLastPayoutDate(object sender, ElapsedEventArgs e)
        {
            //Get Raffles that are complete but still Active
            Console.WriteLine("Get last payout date");
            //var payout = await GetLastPayoutReport();

            //Console.WriteLine("last payout date is" + payout.EndDate.Value.ToString("dddd dd MMM, yyyy"));

            //var AgentRoleId = await _roleManager.FindByNameAsync("Agent");
            var usersAgent = await _countAppService.UsersInRoleForDgaAgentSup(roleid: "4d8b2313-4480-4faf-ae61-49c6a3182993");
            var usersA = usersAgent.Source;
            //
            //var DgaRoleId = await _roleManager.FindByNameAsync("DGAs");
            var usersDga = await _countAppService.UsersInRoleForDgaAgentSup(roleid: "bcba67b4-e59f-4026-b7b5-3e95aaf2f5f5");
            var usersD = usersDga.Source;

            var userlist = usersA.Concat(usersD);
            int n = 0;
            Console.WriteLine(userlist.Count());
            foreach (var io in userlist)
            {
                Console.WriteLine(n++ + ": " + io.UserName);
            }



            //numbering
 int srn = 1;
            //agent and dga
            foreach (var user in userlist)
            {
                ////create record for all
                if (DateTime.UtcNow.AddHours(1).Date.Day.ToString().ToUpper() == "MONDAY")
                {

                    string iDate = "03/18/2019";
                    DateTime oDate = Convert.ToDateTime(iDate);
                    PayOutReportDto Report = new PayOutReportDto();
                    Report.Amount = 0;
                    Report.PercentageAmount = 0;
                    Report.Date = DateTime.UtcNow.AddHours(1);
                    Report.StartDate = oDate.Date;
                    Report.EndDate = oDate.Date.AddDays(6);
                    Report.Note = "wimbig interest";
                    Report.Status = Enums.PayoutEnum.Pending;
                    Report.UserId = user.Id;
                    Report.Percentage = 10;
                    var add = await payOutReportsAppService.Add(Report);
                    Console.WriteLine("number: " + srn++ + ">>" + Report.StartDate.Value + ">>>" + Report.EndDate.Value + "adding new data of " + user.UserName + " for current week");
                }
                //}


                //// get user last record
                var payout = await GetLastPayoutReport(user.Id);
                //start and end date

                //get all report with date range above
                PagedList<TicketDto> Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
                int c = Tickes.Source.Count();

                var ticketbysundays = Tickes.Source.Where(a => a.Date.Date >= payout.StartDate.Value.Date && a.Date.Date <= payout.EndDate.Value.Date);

                decimal sum = ticketbysundays.Sum(w => w.Price);

                decimal per = Convert.ToDecimal(0.1);
                decimal percentageinterest = per * sum;

                payout.Amount = sum;
                payout.PercentageAmount = percentageinterest;
                await payOutReportsAppService.Update(payout);
                Console.WriteLine("number: " + srn++ + ">>" + payout.StartDate.Value + ">>>" + payout.EndDate.Value + "updating today existing data of " + user.UserName + " for current week");

            }





        }

        public void Stop()
{

}



private async Task<PayOutReportDto> GetLastPayoutReport(string id)
{
    try
    {
        var payout = await payOutReportsAppService.GetAsync();

        var last = payout.Source.OrderByDescending(x => x.EndDate).FirstOrDefault(x => x.UserId == id); ;
        Console.WriteLine("Last Report Date " + last.EndDate);

        return last;
    }
    catch (Exception d)
    {
        return null;
    }

}


    }

}



//private async void GetLastPayoutDate(object sender, ElapsedEventArgs e)
//{
//    //Get Raffles that are complete but still Active
//    Console.WriteLine("Get last payout date");
//    var payout = await GetLastPayoutReport();

//    //Console.WriteLine("last payout date is" + payout.EndDate.Value.ToString("dddd dd MMM, yyyy"));

//    //var AgentRoleId = await _roleManager.FindByNameAsync("Agent");
//    var usersAgent = await _countAppService.UsersInRoleForDgaAgentSup(roleid: "4d8b2313-4480-4faf-ae61-49c6a3182993");
//    var usersA = usersAgent.Source;
//    //
//    //var DgaRoleId = await _roleManager.FindByNameAsync("DGAs");
//    var usersDga = await _countAppService.UsersInRoleForDgaAgentSup(roleid: "bcba67b4-e59f-4026-b7b5-3e95aaf2f5f5");
//    var usersD = usersDga.Source;

//    var userlist = usersA.Concat(usersD);
//    int n = 0;
//    Console.WriteLine(userlist.Count());
//    foreach (var io in userlist)
//    {
//        Console.WriteLine(n++ + ": " + io.UserName);
//    }

//    //start and end date
//    DateTime startDate = payout.EndDate.Value.AddDays(-6);
//    DateTime endDate = payout.EndDate.Value;

//    if (endDate.Date <= DateTime.UtcNow.Date)
//    {//new data
//     //check if the dated report has been created

//        DateTime newStartDate = endDate.AddDays(1);
//        DateTime newEndDate = newStartDate.AddDays(6);

//        var report = await payOutReportsAppService.GetAsync();
//        var check = report.Source.FirstOrDefault(x => x.EndDate.Value.Date == newEndDate.Date || x.StartDate.Value.Date == newStartDate.Date);
//        if (check == null)
//        {
//            //agent and dga
//            foreach (var user in userlist)
//            {



//                PagedList<TicketDto> Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
//                int c = Tickes.Source.Count();

//                var ticketbysundays = Tickes.Source.Where(a => a.Date.Date >= newStartDate && a.Date.Date <= newEndDate);
//                //foreach (var group in ticketbysundays)
//                //{
//                // Console.WriteLine("Period: {0}", group.Key);
//                decimal sum = ticketbysundays.Sum(w => w.Price);

//                decimal per = Convert.ToDecimal(0.1);
//                decimal percentageinterest = per * sum;

//                PayOutReportDto Report = new PayOutReportDto();
//                Report.Amount = sum;
//                Report.PercentageAmount = percentageinterest;
//                Report.Date = DateTime.UtcNow.AddHours(1);
//                Report.StartDate = newStartDate;
//                Report.EndDate = newEndDate;
//                Report.Note = "wimbig interest";
//                Report.Status = Enums.PayoutEnum.Pending;
//                Report.UserId = user.Id;
//                Report.Percentage = 10;
//                var add = await payOutReportsAppService.Add(Report);
//                Console.WriteLine("number: " + n++ + ">>" + Report.StartDate.Value + ">>>" + Report.EndDate.Value + "adding new data of " + user.UserName + " for current week");


//            }

//        }
//        else
//        {
//            //date = today
//            TimeSpan diff = endDate - startDate;

//            foreach (var user in userlist)
//            {



//                PagedList<TicketDto> Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
//                int c = Tickes.Source.Count();

//                var ticketbysundays = Tickes.Source.Where(a => a.Date.Date >= newStartDate && a.Date.Date <= newEndDate);
//                //foreach (var group in ticketbysundays)
//                //{
//                // Console.WriteLine("Period: {0}", group.Key);
//                decimal sum = ticketbysundays.Sum(w => w.Price);

//                decimal per = Convert.ToDecimal(0.1);
//                decimal percentageinterest = per * sum;
//                try
//                {


//                    PayOutReportDto Report = await payOutReportsAppService.GetByUserIdAndDateRange(user.Id, newStartDate, newEndDate);

//                    if (Report == null)
//                    {

//                        PayOutReportDto newReport = new PayOutReportDto();
//                        newReport.Amount = sum;
//                        newReport.PercentageAmount = percentageinterest;
//                        newReport.Date = DateTime.UtcNow.AddHours(1);
//                        newReport.StartDate = newStartDate;
//                        newReport.EndDate = newEndDate;
//                        newReport.Note = "wimbig interest";
//                        newReport.Status = Enums.PayoutEnum.Pending;
//                        newReport.UserId = user.Id;
//                        newReport.Percentage = 10;
//                        var add = await payOutReportsAppService.Add(newReport);
//                        Console.WriteLine(Report.StartDate.Value + ">>>" + Report.EndDate.Value + "adding new data of " + user.UserName + " for current week");
//                    }
//                    else
//                    {

//                        Report.Amount = sum;
//                        Report.PercentageAmount = percentageinterest;
//                        await payOutReportsAppService.Update(Report);
//                        Console.WriteLine(Report.StartDate.Value + ">>>" + Report.EndDate.Value + "updating today existing data of " + user.UserName + " for current week");

//                    }
//                }
//                catch (Exception ck)
//                {

//                }



//            }

//        }

//    }
//    else //update data
//    {

//        //agent and dga
//        foreach (var user in userlist)
//        {



//            PagedList<TicketDto> Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
//            int c = Tickes.Source.Count();

//            var ticketbysundays = Tickes.Source.Where(a => a.Date.Date >= startDate && a.Date.Date <= endDate);
//            //foreach (var group in ticketbysundays)
//            //{
//            // Console.WriteLine("Period: {0}", group.Key);
//            decimal sum = ticketbysundays.Sum(w => w.Price);
//            if (sum != 0)
//            {
//                decimal per = Convert.ToDecimal(0.1);
//                decimal percentageinterest = per * sum;
//                try
//                {
//                    PayOutReportDto Report = await payOutReportsAppService.GetByUserIdAndDateRange(user.Id, startDate, endDate);
//                    if (Report == null)
//                    {

//                        PayOutReportDto newReport = new PayOutReportDto();
//                        newReport.Amount = sum;
//                        newReport.PercentageAmount = percentageinterest;
//                        newReport.Date = DateTime.UtcNow.AddHours(1);
//                        newReport.StartDate = startDate;
//                        newReport.EndDate = endDate;
//                        newReport.Note = "wimbig interest";
//                        newReport.Status = Enums.PayoutEnum.Pending;
//                        newReport.UserId = user.Id;
//                        newReport.Percentage = 10;
//                        var add = await payOutReportsAppService.Add(newReport);
//                        Console.WriteLine("adding new data of " + user.UserName + " for week");
//                    }
//                    else
//                    {

//                        Report.Amount = sum;
//                        Report.PercentageAmount = percentageinterest;
//                        await payOutReportsAppService.Update(Report);
//                        Console.WriteLine("updating today existing data of " + user.UserName + " for current week");

//                    }
//                }
//                catch (Exception ck)
//                {

//                }
//            }
//            Console.WriteLine("updating existing data of " + user.UserName + " for current week");
//        }


//    }




//}
