using Exwhyzee.Wimbig.Core.Agent;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Agent
{
    public interface IAgentRepository : IDisposable
    {
        // define extra specific members here.

        Task<PagedList<Agents>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<Agents> Get(long id);

        Task<long> Add(Agents entity);

        //   Task Delete(long id);

        Task Update(Agents entity);
    }
}
