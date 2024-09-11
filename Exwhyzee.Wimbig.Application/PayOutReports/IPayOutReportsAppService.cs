
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.PayOutReports
{
    public interface IPayOutReportsAppService
    {
        Task<PagedList<PayOutReportDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

     
        Task<PayOutReportDto> Get(long id);

        Task<PayOutReportDto> GetByUserIdAndDateRange(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<PayOutReportDto> GetByDateRange(DateTime? startDate = null, DateTime? endDate = null);

        Task<PayOutReportDto> PayOutReportGetLastRecordByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<PayOutReportDto> PayOutReportGetLastRecord(DateTime? startDate = null, DateTime? endDate = null);

        Task<long> Add(PayOutReportDto entity);

        Task Update(PayOutReportDto entity);

        Task UpdatePayout(PayOutReportDto Payout, WalletDto Wallet, TransactionDto transaction);
    }
}
