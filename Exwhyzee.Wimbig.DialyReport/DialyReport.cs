using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.DailyStatistics;
using Exwhyzee.Wimbig.Application.DailyStatistics.Dto;
using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using Exwhyzee.Wimbig.Data.Repository.DailyStatistics;
using Microsoft.AspNetCore.Identity;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Exwhyzee.Wimbig.DialyReport
{

    public class DialyReport : IMicroService
    {
        private IMicroServiceController _controller;
        private IDailyStatisticsAppService _dailyStatisticsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICountAppService _countAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly IDailyStatisticsRepository _dailyStatisticsRepository;
        private readonly IRaffleAppService _raffleAppService;
        private readonly IWalletAppService _walletAppService;

        private Timer timer = new Timer(9000);

        public DialyReport(IMicroServiceController controller,
           RoleManager<ApplicationRole> roleManager, IPurchaseTicketAppService purchaseTicketAppService, ICountAppService countAppService,
           IPayOutReportsAppService payOutReportsAppService,
           UserManager<ApplicationUser> userManger, IDailyStatisticsAppService dailyStatisticsAppService,
           IDailyStatisticsRepository dailyStatisticsRepository, IWalletAppService walletAppService,
           IRaffleAppService raffleAppService)
        {
            _controller = controller;
            _dailyStatisticsAppService = dailyStatisticsAppService;
            _dailyStatisticsRepository = dailyStatisticsRepository;
            _countAppService = countAppService;
            _walletAppService = walletAppService;
            _raffleAppService = raffleAppService;
            _purchaseTicketAppService = purchaseTicketAppService;
        }


        public void Start()
        {
            Console.WriteLine("update dially report");


            timer.Elapsed += GetUpdate;
            timer.Start();
        }

        private async void GetUpdate(object sender, ElapsedEventArgs e)
        {
            //Get Raffles that are complete but still Active
           
            await GetDialyReport();

            //Console.WriteLine("last payout date is" + payout.EndDate.Value.ToString("dddd dd MMM, yyyy"));



        }

        public void Stop()
        {

        }



        private async Task<DailyStatisticsDto> GetDialyReport()
        {
            try
            {
                DateTime StartDate = DateTime.UtcNow.Date;
                var allrport = await _dailyStatisticsAppService.GetAsync();
                var chech = allrport.Source.FirstOrDefault(x => x.Date.Date == StartDate.Date);
                if (chech == null)
                {
                    var userlist = await _countAppService.UsersInRole();

                    var users = userlist.Source.Where(x => x.DateRegistered.Date == StartDate.Date).Count();
                    var ticket = await _purchaseTicketAppService.GetAllTickets();
                    var ticketperday = ticket.Source.Where(x => x.Date.Date.ToShortDateString() == StartDate.Date.ToShortDateString()).Count();
                    var cashticketperday = ticket.Source.Where(x => x.Date.Date.ToShortDateString() == StartDate.Date.ToShortDateString()).Select(x => x.Price).Sum();

                    var raffle = await _raffleAppService.GetAllRaffles();
                    var raffleperday = raffle.Source.Where(x => x.DateCreated.Date.ToShortDateString() == StartDate.Date.ToShortDateString()).Count();

                    var wallcash = await _walletAppService.GetAllWallets();
                    var walletSum = wallcash.Source.Sum(x => x.Balance);
                    DailyStatisticsDto model = new DailyStatisticsDto();
                    var statistic = new DailyStatistic()
                    {
                        Date = StartDate,
                        TotalUsers = users,
                        TotalTickets = ticketperday,
                        TotalRaffle = raffleperday,
                        TotalCash = cashticketperday,
                        TotalWalletCash = walletSum

                    };

                    Console.WriteLine("Date "+ StartDate);
                        Console.WriteLine("TotalUsers "+ users);
                        Console.WriteLine("TotalTickets "+ ticketperday);
                        Console.WriteLine("TotalRaffle "+ raffleperday);
                        Console.WriteLine("TotalCash "+ cashticketperday);
                    Console.WriteLine("TotalWalletCash " + walletSum);
                    try
                    {
                        var chechagain = allrport.Source.FirstOrDefault(x => x.Date.Date == StartDate.Date);
                        if (chechagain == null)
                        {
                            var id = await _dailyStatisticsRepository.Add(statistic);
                            if (id > 0)
                            {
                                Console.WriteLine("create new with " + id);
                            }
                        }
                    }
                    catch(Exception c)
                    {
                        Console.WriteLine("create error" + c);
                    }
                   
                    Console.WriteLine("create no error not true");

                }
                else
                {
                    var userlist = await _countAppService.UsersInRole();

                    var users = userlist.Source.Where(x => x.DateRegistered.Date == StartDate.Date).Count();
                    var ticket = await _purchaseTicketAppService.GetAllTickets();
                    var ticketperday = ticket.Source.Where(x => x.Date.Date.ToShortDateString() == StartDate.Date.ToShortDateString()).Count();
                    var cashticketperday = ticket.Source.Where(x => x.Date.Date.ToShortDateString() == StartDate.Date.ToShortDateString()).Select(x => x.Price).Sum();

                    var raffle = await _raffleAppService.GetAllRaffles();
                    var raffleperday = raffle.Source.Where(x => x.DateCreated.Date.ToShortDateString() == StartDate.Date.ToShortDateString()).Count();

                    var wallcash = await _walletAppService.GetAllWallets();
                    var walletSum = wallcash.Source.Sum(x => x.Balance);

                    var statistic = new DailyStatistic()
                    {
                        Id = chech.Id,
                        Date = chech.Date,
                        TotalUsers = users,
                        TotalTickets = ticketperday,
                        TotalRaffle = raffleperday,
                        TotalCash = cashticketperday,
                        TotalWalletCash = walletSum
                    };
                    Console.WriteLine("Date " + StartDate);
                    Console.WriteLine("TotalUsers " + users);
                    Console.WriteLine("TotalTickets " + ticketperday);
                    Console.WriteLine("TotalRaffle " + raffleperday);
                    Console.WriteLine("TotalCash " + cashticketperday);
                    Console.WriteLine("TotalWalletCash " + walletSum);
                    await _dailyStatisticsRepository.Update(statistic);
                    Console.WriteLine("Update");
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


    }

}
