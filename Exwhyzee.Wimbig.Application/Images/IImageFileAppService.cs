using Exwhyzee.Wimbig.Core.Images;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Images
{
    public interface IImageFileAppService
    {
        Task<long> Insert(ImageFile map);
        Task<ImageFile> GetById(long Id);

        Task<PagedList<ImageFile>> GetAllImages(string extension = null, DateTime? dateStart = null, DateTime? dateStop = null, int startIndex = 0, int count = int.MaxValue);

        // Task<IEnumerable<ImageFile>> GetImagesOfARaffle(long reffleId);
        Task Delete(long Id);
        Task Upate(ImageFile mapping);
    }
}
