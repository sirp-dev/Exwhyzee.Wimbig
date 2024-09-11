
using Exwhyzee.Wimbig.Application.DailyStatistics.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.DailyStatistics
{
    public interface IDailyStatisticsAppService
    {
        Task<PagedList<DailyStatisticsDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

     
        Task<DailyStatisticsDto> Get(long id);

        Task<long> AddOrUpdate();

        Task Update(DailyStatisticsDto entity);
    }
}
