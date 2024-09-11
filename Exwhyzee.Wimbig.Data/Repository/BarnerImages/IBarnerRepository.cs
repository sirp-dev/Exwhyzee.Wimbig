using Exwhyzee.Wimbig.Core.BarnerImage;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.BarnerImages
{
    public interface IBarnerRepository: IDisposable
    {
        Task<long> Insert(BarnerFile map);
        Task<BarnerFile> GetById(long Id);

        Task<PagedList<BarnerFile>> GetAllBarner(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);
        Task Delete(long Id);
        Task Upate(BarnerFile mapping);

    }
}
