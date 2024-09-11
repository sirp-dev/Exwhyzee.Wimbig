
using Exwhyzee.Wimbig.Application.WinnerReports.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.WinnerReports
{
    public interface IWinnerReportsAppService
    {
        Task<PagedList<WinnerReportDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

     
        Task<WinnerReportDto> Get(long id);

        Task<long> Add(WinnerReportDto entity);

        Task Update(WinnerReportDto entity);
    }
}
