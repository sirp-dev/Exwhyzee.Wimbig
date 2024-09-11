
using Exwhyzee.Wimbig.Core.PayOutDetails;
using Exwhyzee.Wimbig.Core.YoutubeLink;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.YoutubeLink
{
    public interface IYoutubeLinkRepository : IDisposable
    {
        // define extra specific members here.
    
        Task<PagedList<YoutubeLinks>> GetAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<YoutubeLinks> Get(long id);

        Task<long> Add(YoutubeLinks entity);

        Task Delete(long id);

        Task Update(YoutubeLinks entity);
    }
}
