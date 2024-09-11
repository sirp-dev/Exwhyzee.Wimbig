using Exwhyzee.Wimbig.Core.MapRaffleToCategorys.Dto;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Exwhyzee.Wimbig.Data.Repository.MapRaffleToCategorys
{
    public interface IMapRaffleToCategoryRepository : IRepository<MapRaffleToCategory>
    {
        Task<PagedList<RaffleCagtegoryDto>> GetRafflesByCategory(long id);

        Task<MapRaffleToCategory> GetByRaffleId(long id);
    }
}
