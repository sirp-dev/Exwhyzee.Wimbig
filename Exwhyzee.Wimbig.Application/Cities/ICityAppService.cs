using Exwhyzee.Wimbig.Application.Cities.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Cities
{
    public interface ICityAppService
    {
        Task<PagedList<CityDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

     
        Task<CityDto> Get(long id);

        Task<long> Add(CityDto entity);

        Task Update(CityDto entity);
        Task Delete(long id);


        Task<PagedList<AreaInCityDto>> GetAreaInCityAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PagedList<AreaInCityDto>> GetAreaInCityByCityIdAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long cityId = 0);

        Task<AreaInCityDto> GetAreaInCity(long id);

        Task<long> AddAreaInCity(AreaInCityDto entity);

        Task UpdateAreaInCity(AreaInCityDto entity);

        Task DeleteAreaInCity(long id);
    }
}
