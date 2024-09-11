using Exwhyzee.Wimbig.Core.WinnerReports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.WinnerReports
{
    public interface IWinnerReportsRepository : IDisposable
    {
        // define extra specific members here.
    
        Task<PagedList<WinnerReport>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<WinnerReport> Get(long id);

        Task<long> Add(WinnerReport entity);

     //   Task Delete(long id);

        Task Update(WinnerReport entity);
    }
}
