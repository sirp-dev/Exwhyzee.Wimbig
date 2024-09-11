using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Core.Raffles;
using Exwhyzee.Wimbig.Data.Repository.Raffles;
using Exwhyzee.Wimbig.Data.Repository.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Raffles
{
    public class RaffleAppService : IRaffleAppService
    {
        private readonly IRaffleRepository _raffleRepository;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
 private readonly ITicketRepository ticketRepository;

        public RaffleAppService(IRaffleRepository raffleRepository, 
         IPurchaseTicketAppService purchaseTicketAppService, ITicketRepository ticketRepository)
        {
            _raffleRepository = raffleRepository;
            _purchaseTicketAppService = purchaseTicketAppService;
            this.ticketRepository = ticketRepository;
        }

       

        public async Task<long> Add (RaffleDto entity)
        {
            var data = new Raffle
            {

                Name = entity.Name,               
                Description = entity.Description,
                HostedBy = entity.HostedBy,
                NumberOfTickets = entity.NumberOfTickets,
                PricePerTicket = entity.PricePerTicket,
                DeliveryType = entity.DeliveryType,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                DateCreated = entity.DateCreated,
                TotalSold = entity.TotalSold,
                SortOrder = entity.SortOrder,
                Location = entity.Location,
                AreaInCity = entity.AreaInCity
            };

           return await _raffleRepository.Add(data);    
        }

        public async Task Delete(long id)
        {
            await _raffleRepository.Delete(id);
        }

        public async Task<PagedList<Raffle>> GetAllRaffles(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<Raffle> raffles = new List<Raffle>();
            var paggedRaffles = await _raffleRepository.GetAsyncAll(status, dateStart, dateEnd, startIndex, count, searchString);

            var paggedSource = paggedRaffles.Source.ToList();


            raffles.AddRange(paggedSource.Select(x => new Raffle()
            {
                DeliveryType = x.DeliveryType,
                Description = x.Description,
                EndDate = x.EndDate,
                HostedBy = x.HostedBy,
                Id = x.Id,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets,
                PricePerTicket = x.PricePerTicket,
                StartDate = x.StartDate,
                Status = x.Status,
                Username = x.Username,
                DateCreated = x.DateCreated,
                TotalSold = x.TotalSold,
                SortOrder = x.SortOrder,
                Archived = x.Archived,
                PaidOut = x.PaidOut,
                Location = x.Location,
                AreaInCity = x.AreaInCity

            }));

            return new PagedList<Raffle>(source: raffles, pageIndex: paggedRaffles.PageIndex,
                                            pageSize: paggedRaffles.PageSize, filteredCount: paggedRaffles.FilteredCount, totalCount:
                                            paggedRaffles.TotalCount);


        }

        public async Task<PagedList<RaffleDto>> GetRaffleByLocationAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<RaffleDto> raffles = new List<RaffleDto>();
            var paggedRaffles = await _raffleRepository.GetRaffleByLocationAll(status, dateStart, dateEnd, startIndex, count, searchString);

            var paggedSource = paggedRaffles.Source.ToList();


            raffles.AddRange(paggedSource.Select(x => new RaffleDto()
            {
                DeliveryType = x.DeliveryType,
                Description = x.Description,
                EndDate = x.EndDate,
                HostedBy = x.HostedBy,
                Id = x.Id,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets,
                PricePerTicket = x.PricePerTicket,
                StartDate = x.StartDate,
                Status = x.Status,
                Username = x.Username,
                DateCreated = x.DateCreated,
                TotalSold = x.TotalSold,
                SortOrder = x.SortOrder,
                Archived = x.Archived,
                PaidOut = x.PaidOut,
                Location = x.Location,
                AreaInCity = x.AreaInCity

            }));

            return new PagedList<RaffleDto>(source: raffles, pageIndex: paggedRaffles.PageIndex,
                                            pageSize: paggedRaffles.PageSize, filteredCount: paggedRaffles.FilteredCount, totalCount:
                                            paggedRaffles.TotalCount);


        }

        public async Task<PagedList<RaffleDto>> GetAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            
            List<RaffleDto> raffles = new List<RaffleDto>();
            var paggedRaffles = await _raffleRepository.GetAsync(status,dateStart,dateEnd,startIndex, count, searchString);
            var paggedSource = paggedRaffles.Source.ToList();


            raffles.AddRange(paggedSource.Select(x => new RaffleDto()
            {
                DeliveryType = x.DeliveryType,
                Description = x.Description,
                EndDate = x.EndDate,
                HostedBy = x.HostedBy,
                Id = x.Id,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets,
                PricePerTicket = x.PricePerTicket,
                StartDate = x.StartDate,
                Status = x.Status,
                Username = x.Username,
                DateCreated = x.DateCreated,
                TotalSold = x.TotalSold,
                SortOrder = x.SortOrder,
                Archived = x.Archived,
                PaidOut = x.PaidOut,
                Location = x.Location,
                AreaInCity = x.AreaInCity

            }));

            return new PagedList<RaffleDto>(source: raffles, pageIndex: paggedRaffles.PageIndex,
                                            pageSize: paggedRaffles.PageSize, filteredCount: paggedRaffles.FilteredCount, totalCount:
                                            paggedRaffles.TotalCount);
        }

        public async Task<RaffleDto> GetById(long id)
        {
            var data = await _raffleRepository.Get(id);

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var raffleDto = new RaffleDto
            {
               DeliveryType = data.DeliveryType,
               Description = data.Description,
               EndDate = data.EndDate,
               HostedBy = data.HostedBy,
               Id = data.Id,
               Name = data.Name,
               NumberOfTickets = data.NumberOfTickets,
               PricePerTicket = data.PricePerTicket,
               StartDate = data.StartDate,
               Status = data.Status,
                DateCreated = data.DateCreated,
                TotalSold = data.TotalSold,
                SortOrder = data.SortOrder,
                Archived = data.Archived,
                PaidOut = data.PaidOut,
                Location = data.Location,
                AreaInCity = data.AreaInCity

            };

            return raffleDto;
        }

        public async Task<List<RaffleDto>> GetRaffleByStatus(int status, int count)
        {
            List<RaffleDto> raffles = new List<RaffleDto>();
             var query = await _raffleRepository.GetRafflesByStatus(status,count);

            raffles.AddRange(query.Select(x => new RaffleDto()
            {
                DeliveryType = x.DeliveryType,
                Description = x.Description,
                EndDate = x.EndDate,
                HostedBy = x.HostedBy,
                Id = x.Id,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets,
                PricePerTicket = x.PricePerTicket,
                StartDate = x.StartDate,
                Status = x.Status,
                DateCreated = x.DateCreated,
                TotalSold = x.TotalSold,
                Archived = x.Archived,
                PaidOut = x.PaidOut,
                Location = x.Location,
                AreaInCity = x.AreaInCity

            }));

            return raffles;
        }

        public async Task<PagedList<RaffleDto>> GetRafflesByHostedBy(string hostedBy, int? status = null, int startIndex = 0, int count = int.MaxValue)
        {
            List<RaffleDto> raffles = new List<RaffleDto>();
            var query = await _raffleRepository.GetRafflesByHostedBy(hostedBy,status,startIndex,count);
            var querySource = query.Source.ToList(); 

            raffles.AddRange(querySource.Select(x => new RaffleDto()
            {
                DeliveryType = x.DeliveryType,
                Description = x.Description,
                EndDate = x.EndDate,
                HostedBy = x.HostedBy,
                Id = x.Id,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets,
                PricePerTicket = x.PricePerTicket,
                StartDate = x.StartDate,
                Status = x.Status,
                DateCreated = x.DateCreated,
                TotalSold = x.TotalSold,
                Archived = x.Archived,
                PaidOut = x.PaidOut,
                Location = x.Location,
                AreaInCity = x.AreaInCity
            }));


            return new PagedList<RaffleDto>(source: raffles, pageIndex: startIndex,
                                            pageSize: count, filteredCount: query.FilteredCount, totalCount:
                                            query.TotalCount);
        }

        public async Task Update(RaffleDto entity)
        {
            var data = new Raffle
            {
                Id = entity.Id,
                DeliveryType = entity.DeliveryType,
                Description = entity.Description,
                EndDate = entity.EndDate,
                HostedBy = entity.HostedBy,
                Name = entity.Name,
                NumberOfTickets = entity.NumberOfTickets,
                PricePerTicket = entity.PricePerTicket,
                StartDate = entity.StartDate,
                Status = entity.Status,
                DateCreated = entity.DateCreated,
                TotalSold = entity.TotalSold,
                DateWon = entity.DateWon,
                SortOrder = entity.SortOrder,
                Archived = entity.PaidOut,
                PaidOut = entity.Archived,
                Location = entity.Location,
                AreaInCity = entity.AreaInCity

            };

            await _raffleRepository.Update(data);

            
            var ticketsOfRaflle = await ticketRepository.GetAllTickets(raffleId: entity.Id);

            foreach (var tick in ticketsOfRaflle.Source)
            {
                try
                {


                    await ticketRepository.UpdateTicketStatus(tick.Id);
                }
                catch (Exception c)
                {

                }
            }

        }


        public async Task<List<RaffleListDto>> GetRaffleList(long raffleId)
        {
            var list = new List<RaffleListDto>();
            var raffle = await _raffleRepository.Get(raffleId);

            if(raffle != null)
            {
                //get Tickets sold
                var purchased = await _purchaseTicketAppService.GetAllTickets(raffleId: raffleId);
                var count = raffle.NumberOfTickets + 1;

                for (int i = 1; i < count; i++)
                {
                    //var checkr = purchased.Source.FirstOrDefault(x => x.TicketNumber == i.ToString());
                    var check = purchased.Source.FirstOrDefault(x => x.TicketNumber == i.ToString());
                    list.Add(new RaffleListDto
                    {
                        RaffleNumber = i,
                        Status = check != null?"checked":null
                    });
                }
   
            }

           
            return list;
            
        }

       
        public async Task<bool> AddToArchieve(long id)
        {
            return await _raffleRepository.AddToArchieve(id);
        }

        public async Task<bool> RemoveFromArchieve(long id)
        {
            return await _raffleRepository.RemoveFromArchieve(id);
        }

        public async Task<PagedList<RaffleDto>> GetRafflesByArchieved(bool archieved = true, int? status = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<RaffleDto> raffles = new List<RaffleDto>();
            var paggedRaffles = await _raffleRepository.GetRafflesByArchieved(archieved, status, startIndex, count, searchString);
            var paggedSource = paggedRaffles.Source.ToList();


            raffles.AddRange(paggedSource.Select(x => new RaffleDto()
            {
                DeliveryType = x.DeliveryType,
                Description = x.Description,
                EndDate = x.EndDate,
                HostedBy = x.HostedBy,
                Id = x.Id,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets,
                PricePerTicket = x.PricePerTicket,
                StartDate = x.StartDate,
                Status = x.Status,
                Username = x.Username,
                DateCreated = x.DateCreated,
                TotalSold = x.TotalSold,
                SortOrder = x.SortOrder,
                Archived = x.Archived,
                PaidOut = x.PaidOut,
                Location = x.Location,
                AreaInCity = x.AreaInCity

            }));

            return new PagedList<RaffleDto>(source: raffles, pageIndex: paggedRaffles.PageIndex,
                                            pageSize: paggedRaffles.PageSize, filteredCount: paggedRaffles.FilteredCount, totalCount:
                                            paggedRaffles.TotalCount);
        }
    }
}
