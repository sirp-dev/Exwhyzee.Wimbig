using Exwhyzee.Wimbig.Application.MapRaffleToCategorys.Dtos;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys.Dto;
using System;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.MapRaffleToCategorys
{
    public interface  IMapRaffleToCategoryAppService
    {
        Task<long> Add(MapRaffleToCategoryDto model);

        Task Delete(long id);

        Task<MapRaffleToCategoryDto> Get(long id);

        Task<PagedList<MapRaffleToCategoryDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PagedList<RaffleCagtegoryDto>> GetRafflesByCategory(long id);

        Task Update(MapRaffleToCategoryDto entity);

        Task<MapRaffleToCategoryDto> GetByRaffleId(long id);
    }
}
