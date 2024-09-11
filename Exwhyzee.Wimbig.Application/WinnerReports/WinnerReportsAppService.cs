
using Exwhyzee.Wimbig.Application.WinnerReports.Dto;
using Exwhyzee.Wimbig.Core.WinnerReports;
using Exwhyzee.Wimbig.Data.Repository.WinnerReports;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.WinnerReports
{
    public class WinnerReportsAppService : IWinnerReportsAppService
    {
        private readonly IWinnerReportsRepository _winnerReportsRepository;
        

        public WinnerReportsAppService(IWinnerReportsRepository winnerReportsRepository)
        {
            _winnerReportsRepository = winnerReportsRepository;
        }

        public async Task<long> Add(WinnerReportDto entity)
        {
            try
            {
                var report = new WinnerReport()
                {
                    WinnerName = entity.WinnerName,
                    WinnerPhoneNumber = entity.WinnerPhoneNumber,
                    WinnerEmail = entity.WinnerEmail,
                    WinnerLocation = entity.WinnerLocation,
                    AmountPlayed = entity.AmountPlayed,
                    RaffleName = entity.RaffleName,
                    RaffleId = entity.RaffleId,
                    TicketNumber = entity.TicketNumber,
                    ItemCost = entity.ItemCost,
                    DateCreated = entity.DateCreated,
                    DateWon = entity.DateWon,
                    DateDelivered = entity.DateDelivered,
                    DeliveredBy = entity.DeliveredBy,
                    DeliveredPhone = entity.DeliveredPhone,
                    DeliveryAddress = entity.DeliveryAddress,
                    TotalAmountPlayed = entity.TotalAmountPlayed,
                    UserId = entity.UserId,
                    Status = entity.Status

                };

                return await _winnerReportsRepository.Add(report);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
           
        public async Task<PagedList<WinnerReportDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            
            List<WinnerReportDto> statistic = new List<WinnerReportDto>();

            var query = await _winnerReportsRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);

            statistic.AddRange(query.Source.Select(entity => new WinnerReportDto()
            {
                Id =entity.Id,
                WinnerName = entity.WinnerName,
                WinnerPhoneNumber = entity.WinnerPhoneNumber,
                WinnerEmail = entity.WinnerEmail,
                WinnerLocation = entity.WinnerLocation,
                AmountPlayed = entity.AmountPlayed,
                RaffleName = entity.RaffleName,
                RaffleId = entity.RaffleId,
                TicketNumber = entity.TicketNumber,
                ItemCost = entity.ItemCost,
                DateCreated = entity.DateCreated,
                DateWon = entity.DateWon,
                DateDelivered = entity.DateDelivered,
                DeliveredBy = entity.DeliveredBy,
                DeliveredPhone = entity.DeliveredPhone,
                DeliveryAddress = entity.DeliveryAddress,
                TotalAmountPlayed = entity.TotalAmountPlayed,
                UserId = entity.UserId,
                Status = entity.Status



            }));

            return new PagedList<WinnerReportDto>(source: statistic, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<WinnerReportDto> Get(long id)
        {
            try
            {
                var entity = await _winnerReportsRepository.Get(id);

                var data = new WinnerReportDto
                {
                    Id = entity.Id,
                    WinnerName = entity.WinnerName,
                    WinnerPhoneNumber = entity.WinnerPhoneNumber,
                    WinnerEmail = entity.WinnerEmail,
                    WinnerLocation = entity.WinnerLocation,
                    AmountPlayed = entity.AmountPlayed,
                    RaffleName = entity.RaffleName,
                    RaffleId = entity.RaffleId,
                    TicketNumber = entity.TicketNumber,
                    ItemCost = entity.ItemCost,
                    DateCreated = entity.DateCreated,
                    DateWon = entity.DateWon,
                    DateDelivered = entity.DateDelivered,
                    DeliveredBy = entity.DeliveredBy,
                    DeliveredPhone = entity.DeliveredPhone,
                    DeliveryAddress = entity.DeliveryAddress,
                    TotalAmountPlayed = entity.TotalAmountPlayed,
                    UserId = entity.UserId,
                    Status = entity.Status


                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        public async Task Update(WinnerReportDto entity)
        {
            try
            {


                var data = new WinnerReport
                {
                    Id = entity.Id,
                    WinnerName = entity.WinnerName,
                    WinnerPhoneNumber = entity.WinnerPhoneNumber,
                    WinnerEmail = entity.WinnerEmail,
                    WinnerLocation = entity.WinnerLocation,
                    AmountPlayed = entity.AmountPlayed,
                    RaffleName = entity.RaffleName,
                    RaffleId = entity.RaffleId,
                    TicketNumber = entity.TicketNumber,
                    ItemCost = entity.ItemCost,
                    DateCreated = entity.DateCreated,
                    DateWon = entity.DateWon,
                    DateDelivered = entity.DateDelivered,
                    DeliveredBy = entity.DeliveredBy,
                    DeliveredPhone = entity.DeliveredPhone,
                    DeliveryAddress = entity.DeliveryAddress,
                    TotalAmountPlayed = entity.TotalAmountPlayed,
                    UserId = entity.UserId,
                    Status = entity.Status

                };

               await _winnerReportsRepository.Update(data);


                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
