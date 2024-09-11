using Exwhyzee.Wimbig.Core.BarnerImage;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.SideBarner;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Barner
{
    public interface IBarnerAppService
    {
        Task<long> Insert(BarnerFile map);
       
        Task<PagedList<BarnerFile>> GetBarnerFile(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task Delete(long Id);

        ///

        Task<long> InsertSideBarner(SideBarnerFile map);
        
        Task<PagedList<SideBarnerFile>> GetBarnerFileSideBarner(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task DeleteSideBarner(long Id);

    }
}
