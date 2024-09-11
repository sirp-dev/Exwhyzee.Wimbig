
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Application.YoutubeLink.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.YoutubeLink
{
    public interface IYoutubeLinkAppService
    {
        Task<PagedList<YoutubeLinkDto>> GetAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

     
        Task<YoutubeLinkDto> Get(long id);

        Task Delete(long Id);

        Task<long> Add(YoutubeLinkDto entity);

        Task Update(YoutubeLinkDto entity);
    }
}
