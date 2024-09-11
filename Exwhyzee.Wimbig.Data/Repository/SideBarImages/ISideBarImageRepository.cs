using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.SideBarner;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.SideBarImages
{
    public interface ISideBarImageRepository: IDisposable
    {
        Task<long> Insert(SideBarnerFile map);
        Task<SideBarnerFile> GetById(long Id);
        Task<PagedList<SideBarnerFile>> GetAllSideBarImage(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);
        
        Task Delete(long Id);
        //Task Upate(SideBarnerFile mapping);

    }
}
