
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.PayOutReports;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Data.Repository.PayOutReports;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.PayOutReports
{
    public class PayOutReportsAppService : IPayOutReportsAppService
    {
        private readonly IPayOutReportRepository _payOutReportRepository;
        private readonly IRaffleAppService _raffleAppService;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        

        public PayOutReportsAppService(IPayOutReportRepository payOutReportRepository,
            IRaffleAppService raffleAppService,
            IPurchaseTicketAppService purchaseTicketAppService)
        {
            _payOutReportRepository = payOutReportRepository;
            _raffleAppService = raffleAppService;
            _purchaseTicketAppService = purchaseTicketAppService;
           
        }

        public async Task<long> Add(PayOutReportDto entity)
        {
           try
            {
                var report = new PayOutReport()
                {
                    Date = entity.Date,
                    Amount = entity.Amount,
                    PercentageAmount = entity.PercentageAmount,
                    Percentage = entity.Percentage,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    Note = entity.Note,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    UserId = entity.UserId

                };

                return await _payOutReportRepository.Add(report);
    }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }



        public async Task<PagedList<PayOutReportDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            
            List<PayOutReportDto> statistic = new List<PayOutReportDto>();

            var query = await _payOutReportRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);

            statistic.AddRange(query.Source.Select(entity => new PayOutReportDto()
            {
                Id = entity.Id,
                Date = entity.Date,
                Amount = entity.Amount,
                PercentageAmount = entity.PercentageAmount,
                Percentage = entity.Percentage,
                Reference = entity.Reference,
                Status = entity.Status,
                Note = entity.Note,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                userName = entity.UserName,
                UserId = entity.UserId,
                RoleName = entity.RoleName

            }));

            return new PagedList<PayOutReportDto>(source: statistic, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<PayOutReportDto> Get(long id)
        {
            try
            {
                var entity = await _payOutReportRepository.Get(id);

                var data = new PayOutReportDto
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Amount = entity.Amount,
                    PercentageAmount = entity.PercentageAmount,
                    Percentage = entity.Percentage,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    Note = entity.Note,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    userName = entity.UserName,
                    RoleName = entity.RoleName,
                    UserId = entity.UserId
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        public async Task Update(PayOutReportDto entity)
        {
            try
            {


                var data = new PayOutReport
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Amount = entity.Amount,
                    Percentage = entity.Percentage,
                    PercentageAmount = entity.PercentageAmount,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    Note = entity.Note,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    UserId = entity.UserId

                };

               await _payOutReportRepository.Update(data);


                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PayOutReportDto> GetByUserIdAndDateRange(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var entity = await _payOutReportRepository.GetByUserIdAndDateRange(userId, startDate, endDate);

                var data = new PayOutReportDto
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Amount = entity.Amount,
                    PercentageAmount = entity.PercentageAmount,
                    Percentage = entity.Percentage,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    Note = entity.Note,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    userName = entity.UserName,
                    UserId = entity.UserId,
                    RoleName = entity.RoleName
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<PayOutReportDto> GetByDateRange(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var entity = await _payOutReportRepository.GetByDateRange(startDate, endDate);

                var data = new PayOutReportDto
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Amount = entity.Amount,
                    PercentageAmount = entity.PercentageAmount,
                    Percentage = entity.Percentage,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    Note = entity.Note,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    userName = entity.UserName,
                    UserId = entity.UserId,
                    RoleName = entity.RoleName
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PayOutReportDto> PayOutReportGetLastRecordByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var entity = await _payOutReportRepository.PayOutReportGetLastRecordByUserId(userId, startDate, endDate);

                var data = new PayOutReportDto
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Amount = entity.Amount,
                    PercentageAmount = entity.PercentageAmount,
                    Percentage = entity.Percentage,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    Note = entity.Note,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    userName = entity.UserName,
                    UserId = entity.UserId,
                    RoleName = entity.RoleName
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PayOutReportDto> PayOutReportGetLastRecord(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var entity = await _payOutReportRepository.PayOutReportGetLastRecord(startDate, endDate);

                var data = new PayOutReportDto
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Amount = entity.Amount,
                    PercentageAmount = entity.PercentageAmount,
                    Percentage = entity.Percentage,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    Note = entity.Note,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    userName = entity.UserName,
                    UserId = entity.UserId,
                    RoleName = entity.RoleName
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePayout(PayOutReportDto Payout, WalletDto wallet, TransactionDto transaction)
        {
            try
            {

                var data = new PayOutReport
                {
                    Id = Payout.Id,
                    Date = Payout.Date,
                    Amount = Payout.Amount,
                    Percentage = Payout.Percentage,
                    PercentageAmount = Payout.PercentageAmount,
                    Reference = Payout.Reference,
                    Status = Payout.Status,
                    Note = Payout.Note,
                    StartDate = Payout.StartDate,
                    EndDate = Payout.EndDate,
                    UserId = Payout.UserId

                };

                var walletData = new Wallet
                {
                    Balance = wallet.Balance,
                    DateUpdated = wallet.DateUpdated,
                    UserId = wallet.UserId,
                    Id = wallet.Id
                };

                var transactionData = new Transaction
                {
                    Id = transaction.Id,
                    Username = transaction.Username,
                    Amount = transaction.Amount,
                    DateOfTransaction = transaction.DateOfTransaction,
                    Status = transaction.Status,
                    TransactionType = transaction.TransactionType,
                    UserId = transaction.UserId,
                    WalletId = transaction.WalletId,
                    TransactionReference = transaction.TransactionReference,
                    Description = transaction.Description

                };
                await _payOutReportRepository.UpdatePayout(data, walletData, transactionData);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
