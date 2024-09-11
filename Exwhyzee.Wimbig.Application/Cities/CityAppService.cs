using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Exwhyzee.Wimbig.Core.Cities;
using Exwhyzee.Wimbig.Data.Repository.Cities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Cities
{
    public class CityAppService : ICityAppService
    {
        private readonly ICityRepository _cityRepository;

        public CityAppService(ICityRepository cityRepository
           )
        {
            _cityRepository = cityRepository;

        }

        public async Task<long> Add(CityDto entity)
        {
            try
            {
                var city = new City()
                {
                    Name = entity.Name
                };

                return await _cityRepository.Add(city);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<CityDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {

            List<CityDto> agent = new List<CityDto>();

            var query = await _cityRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);

            agent.AddRange(query.Source.Select(entity => new CityDto()
            {
                Id = entity.Id,
                Name = entity.Name


            }));

            return new PagedList<CityDto>(source: agent, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<CityDto> Get(long id)
        {
            try
            {
                var entity = await _cityRepository.Get(id);

                var data = new CityDto
                {
                    Id = entity.Id,
                    Name = entity.Name
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task Update(CityDto entity)
        {
            try
            {


                var data = new City
                {
                    Id = entity.Id,
                    Name = entity.Name


                };

                await _cityRepository.Update(data);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task Delete(long id)
        {
            await _cityRepository.Delete(id);
        }


        ///

        public async Task<long> AddAreaInCity(AreaInCityDto entity)
        {
            try
            {
                var city = new AreaInCity()
                {
                    Name = entity.Name,
                    CityId = entity.CityId
                };

                return await _cityRepository.AddAreaInCity(city);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<AreaInCityDto>> GetAreaInCityAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {

            List<AreaInCityDto> agent = new List<AreaInCityDto>();

            var query = await _cityRepository.GetAreaInCityAsync(status, dateStart, dateEnd, startIndex, count, searchString);

            agent.AddRange(query.Source.Select(entity => new AreaInCityDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                CityId = entity.CityId,
                CityName = entity.CityName


            }));

            return new PagedList<AreaInCityDto>(source: agent, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<AreaInCityDto> GetAreaInCity(long id)
        {
            try
            {
                var entity = await _cityRepository.GetAreaInCity(id);

                var data = new AreaInCityDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CityId = entity.CityId
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task UpdateAreaInCity(AreaInCityDto entity)
        {
            try
            {


                var data = new AreaInCity
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CityId = entity.CityId


                };

                await _cityRepository.UpdateAreaInCity(data);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task DeleteAreaInCity(long id)
        {
            await _cityRepository.DeleteAreaInCity(id);
        }

        public async Task<PagedList<AreaInCityDto>> GetAreaInCityByCityIdAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long cityId = 0)
        {

            List<AreaInCityDto> city = new List<AreaInCityDto>();

            var query = await _cityRepository.GetAreaInCityByCityIdAsync(dateStart, dateEnd, startIndex, count, searchString, cityId);

            city.AddRange(query.Source.Select(entity => new AreaInCityDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                CityId = entity.CityId,
                CityName = entity.CityName


            }));

            return new PagedList<AreaInCityDto>(source: city, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }
    }
}
