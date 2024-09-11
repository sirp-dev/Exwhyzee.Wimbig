using Exwhyzee.Wimbig.Core.Cities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Cities
{
    public interface ICityRepository : IDisposable
    {
        // define extra specific members here.
    
        Task<PagedList<City>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<City> Get(long id);

        Task<long> Add(City entity);

        Task Delete(long id);

        Task Update(City entity);

        //

        Task<PagedList<AreaInCity>> GetAreaInCityAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);
        Task<PagedList<AreaInCity>> GetAreaInCityByCityIdAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long cityId = 0);

        Task<AreaInCity> GetAreaInCity(long id);

        Task<long> AddAreaInCity(AreaInCity entity);

        Task DeleteAreaInCity(long id);

        Task UpdateAreaInCity(AreaInCity entity);
    }
}
