
using Exwhyzee.Wimbig.Application.Agent.Dto;
using Exwhyzee.Wimbig.Application.DailyStatistics.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Agent
{
    public interface IAgentAppService
    {
        Task<PagedList<AgentDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

     
        Task<AgentDto> Get(long id);

        Task<long> Add(AgentDto entity);

        Task Update(AgentDto entity);
    }
}
