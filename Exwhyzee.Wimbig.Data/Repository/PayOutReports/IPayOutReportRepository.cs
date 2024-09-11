
using Exwhyzee.Wimbig.Core.PayOutReports;
using Exwhyzee.Wimbig.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.PayOutReports
{
    public interface IPayOutReportRepository : IDisposable
    {
        // define extra specific members here.
    
        Task<PagedList<PayOutReport>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PayOutReport> Get(long id);

        Task<PayOutReport> GetByUserIdAndDateRange(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<PayOutReport> GetByDateRange(DateTime? startDate = null, DateTime? endDate = null);

        Task<PayOutReport> PayOutReportGetLastRecordByUserId(string userId, DateTime? startDate = null, DateTime? endDate = null);
        Task<PayOutReport> PayOutReportGetLastRecord(DateTime? startDate = null, DateTime? endDate = null);

        Task<long> Add(PayOutReport entity);

     //   Task Delete(long id);

        Task Update(PayOutReport entity);
        

        Task UpdatePayout(PayOutReport data, Wallet walletData, Transaction transactionData);
    }
}
