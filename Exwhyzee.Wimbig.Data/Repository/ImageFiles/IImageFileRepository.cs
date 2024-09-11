using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.ImageFiles
{
    public interface IImageFileRepository: IDisposable
    {
        Task<long> Insert(ImageFile map);
        Task<ImageFile> GetById(long Id);

        Task<IEnumerable<ImageFile>> GetImagesOfARaffle(long reffleId);
        Task Delete(long Id);
        Task Upate(ImageFile mapping);

        Task<PagedList<ImageFile>> GetAsyncAll(string extension = null, DateTime? dateStart = null, DateTime? dateStop = null, int startIndex = 0, int count = int.MaxValue);


    }
}
