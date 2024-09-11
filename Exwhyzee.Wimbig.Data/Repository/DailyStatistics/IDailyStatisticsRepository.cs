using Exwhyzee.Wimbig.Core.DailyStatistics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.DailyStatistics
{
    public interface IDailyStatisticsRepository : IDisposable
    {
        // define extra specific members here.
    
        Task<PagedList<DailyStatistic>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<DailyStatistic> Get(long id);

        Task<long> Add(DailyStatistic entity);

     //   Task Delete(long id);

        Task Update(DailyStatistic entity);
    }
}
