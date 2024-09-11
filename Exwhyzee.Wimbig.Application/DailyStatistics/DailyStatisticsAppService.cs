using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.DailyStatistics.Dto;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Categories;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using Exwhyzee.Wimbig.Data.Repository.Categorys;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Exwhyzee.Wimbig.Data.Repository.DailyStatistics;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.DailyStatistics
{
    public class DailyStatisticsAppService : IDailyStatisticsAppService
    {
        private readonly IDailyStatisticsRepository _dailyStatisticsRepository;
        private readonly IRaffleAppService _raffleAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly ICountAppService _countAppService;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletAppService _walletAppService;

        public DailyStatisticsAppService(IDailyStatisticsRepository dailyStatisticsRepository,
            IRaffleAppService raffleAppService,
            IPurchaseTicketAppService purchaseTicketAppService,
            ICountAppService countAppService,
            IWalletAppService walletAppService)
        {
            _dailyStatisticsRepository = dailyStatisticsRepository;
            _raffleAppService = raffleAppService;
            _purchaseTicketAppService = purchaseTicketAppService;
            _countAppService = countAppService;
            _walletAppService = walletAppService;
        }

        public async Task<long> AddOrUpdate()
        {
            try
            {
                DateTime StartDate = DateTime.UtcNow.Date;
                var allrport = await GetAsync();
                var chech = allrport.Source.FirstOrDefault(x => x.Date == StartDate);
                if (chech == null)
                {
var userlist = await _countAppService.UsersInRole();

                    var users = userlist.Source.Where(x =>x.DateRegistered == StartDate).Count();
                    var ticket = await _purchaseTicketAppService.GetAllTickets();
                    var ticketperday = ticket.Source.Where(x => x.Date.ToShortDateString() == StartDate.ToShortDateString()).Count();
                    var cashticketperday = ticket.Source.Where(x => x.Date.ToShortDateString() == StartDate.ToShortDateString()).Select(x=>x.Price).Sum();

                    var raffle = await _raffleAppService.GetAllRaffles();
                    var raffleperday = raffle.Source.Where(x => x.DateCreated.ToShortDateString() == StartDate.ToShortDateString()).Count();

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

                     await _dailyStatisticsRepository.Add(statistic);

                }
                else
                {
                    var userlist = await _countAppService.UsersInRole();

                    var users = userlist.Source.Where(x => x.DateRegistered == StartDate).Count();
                    var ticket = await _purchaseTicketAppService.GetAllTickets();
                    var ticketperday = ticket.Source.Where(x => x.Date.ToShortDateString() == StartDate.ToShortDateString()).Count();
                    var cashticketperday = ticket.Source.Where(x => x.Date.ToShortDateString() == StartDate.ToShortDateString()).Select(x => x.Price).Sum();

                    var raffle = await _raffleAppService.GetAllRaffles();
                    var raffleperday = raffle.Source.Where(x => x.DateCreated.ToShortDateString() == StartDate.ToShortDateString()).Count();

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

                    await _dailyStatisticsRepository.Update(statistic);
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            public async Task<long> AddChech(DailyStatisticsDto model1)
        {
            try
            {
                DateTime StartDate = DateTime.Parse("12/16/2018");
                DateTime enddate = DateTime.UtcNow;
                for (var date = StartDate; date <= enddate; date = date.AddDays(1))
                {
               
                    DailyStatisticsDto model = new DailyStatisticsDto();

                    model.Date = date.Date;
                   
                    var statistic = new DailyStatistic()
                    {
                        Date = model.Date,
                        TotalUsers = model.TotalUsers,
                        TotalTickets = model.TotalTickets,
                        TotalRaffle = model.TotalRaffle,
                        TotalCash = model.TotalCash,
                        TotalWalletCash = model.TotalWalletCash
                    };

                    await _dailyStatisticsRepository.Add(statistic);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            //ticket count
            try
            {
                var ticket = await _purchaseTicketAppService.GetAllTickets();
                var gtick = ticket.Source.GroupBy(x => x.Date.Date).ToList();
                foreach (var i in gtick)
                {
                    var dateofraffle = i.FirstOrDefault().Date.Date;
                    var allrport = await GetAsync();
                    var chech = allrport.Source.FirstOrDefault(x => x.Date == dateofraffle);
                    //var chech = allrport.Source.Select(x => x.Date).Contains(dateoftick);
                    if (chech != null)
                    {
                        DailyStatisticsDto entity = chech;
                        entity.TotalTickets = i.Count();

                        var data = new DailyStatistic
                        {
                            Id = entity.Id,
                            TotalTickets = entity.TotalTickets,
                            Date = entity.Date,
                            TotalUsers = entity.TotalUsers,
                            TotalRaffle = entity.TotalRaffle,
                            TotalCash = entity.TotalCash,
                            TotalWalletCash = entity.TotalWalletCash

                        };
                        await _dailyStatisticsRepository.Update(data);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //user count
            try
            {
                var userlist = await _countAppService.UsersInRole();

                var users = userlist.Source.Where(x => x.UserName != "mJinmcever").ToList();

               var gusers = users.GroupBy(x => x.DateRegistered.Date).ToList();
                    foreach (var i in gusers)
                    {
                        var dateofreguser = i.FirstOrDefault().DateRegistered.Date;
                        var allrport = await GetAsync();
                        var chech = allrport.Source.FirstOrDefault(x => x.Date == dateofreguser);
                        //var chech = allrport.Source.Select(x => x.Date).Contains(dateoftick);
                        if (chech != null)
                        {
                            DailyStatisticsDto entity = chech;
                            entity.TotalUsers = i.Count();
                            
                            var data = new DailyStatistic
                            {
                                Id = entity.Id,
                                TotalTickets = entity.TotalTickets,
                                Date = entity.Date,
                                TotalUsers = entity.TotalUsers,                               
                                TotalRaffle = entity.TotalRaffle,
                                TotalCash = entity.TotalCash,
                                TotalWalletCash = entity.TotalWalletCash

                            };
                            await _dailyStatisticsRepository.Update(data);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //raffle count
                try
                {
                    var raffle = await _raffleAppService.GetAll();
                    var graffle = raffle.Source.GroupBy(x => x.DateCreated.Date).ToList();
                    foreach (var i in graffle)
                    {
                        var dateofraffle = i.FirstOrDefault().DateCreated.Date;
                        var allrport = await GetAsync();
                        var chech = allrport.Source.FirstOrDefault(x => x.Date == dateofraffle);
                        //var chech = allrport.Source.Select(x => x.Date).Contains(dateoftick);
                        if (chech != null)
                        {
                            DailyStatisticsDto entity = chech;
                            entity.TotalRaffle = i.Count();

                            var data = new DailyStatistic
                            {
                                Id = entity.Id,
                                TotalTickets = entity.TotalTickets,
                                Date = entity.Date,
                                TotalUsers = entity.TotalUsers,
                                TotalRaffle = entity.TotalRaffle,
                                TotalCash = entity.TotalCash,
                                TotalWalletCash = entity.TotalWalletCash

                            };
                            await _dailyStatisticsRepository.Update(data);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //ticket cash
                try
                {
                    var ticketcash = await _purchaseTicketAppService.GetAllTickets();
                    var gtick = ticketcash.Source.GroupBy(x => x.Date.Date).ToList();
                    foreach (var i in gtick)
                    {
                        var dateoftick = i.FirstOrDefault().Date.Date;
                        var allrport = await GetAsync();
                        var chech = allrport.Source.FirstOrDefault(x => x.Date == dateoftick);
                        //var chech = allrport.Source.Select(x => x.Date).Contains(dateoftick);
                        if (chech != null)
                        {
                            DailyStatisticsDto entity = chech;
                            entity.TotalCash = i.Select(x=>x.Price).Sum();

                            var data = new DailyStatistic
                            {
                                Id = entity.Id,
                                TotalTickets = entity.TotalTickets,
                                Date = entity.Date,
                                TotalUsers = entity.TotalUsers,
                                TotalRaffle = entity.TotalRaffle,
                                TotalCash = entity.TotalCash,
                                TotalWalletCash = entity.TotalWalletCash

                            };
                            await _dailyStatisticsRepository.Update(data);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

             
                return 1;
          
        }



        public async Task<PagedList<DailyStatisticsDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            
            List<DailyStatisticsDto> statistic = new List<DailyStatisticsDto>();

            var query = await _dailyStatisticsRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);

            statistic.AddRange(query.Source.Select(x => new DailyStatisticsDto()
            {
                Date = x.Date,
                TotalUsers = x.TotalUsers,
                Id = x.Id,
                TotalTickets = x.TotalTickets,
                TotalRaffle = x.TotalRaffle,
                TotalCash = x.TotalCash,
                TotalWalletCash = x.TotalWalletCash
               
            }));

            return new PagedList<DailyStatisticsDto>(source: statistic, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<DailyStatisticsDto> Get(long id)
        {
            try
            {
                var statistic = await _dailyStatisticsRepository.Get(id);

                var data = new DailyStatisticsDto
                {
                    Date = statistic.Date,
                   TotalUsers = statistic.TotalUsers,
                   TotalTickets = statistic.TotalTickets,
                   TotalRaffle = statistic.TotalRaffle,
                   TotalCash = statistic.TotalCash,
                   TotalWalletCash = statistic.TotalWalletCash
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        public async Task Update(DailyStatisticsDto entity)
        {
            try
            {


                var data = new DailyStatistic
                {
                    Id = entity.Id,
                   Date = entity.Date,
                   TotalUsers = entity.TotalUsers,
                   TotalTickets = entity.TotalTickets,
                   TotalRaffle = entity.TotalRaffle,
                   TotalCash = entity.TotalCash,
                   TotalWalletCash = entity.TotalWalletCash

                };

               await _dailyStatisticsRepository.Update(data);


                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
