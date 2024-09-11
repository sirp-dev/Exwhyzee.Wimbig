using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.MapRaffleToCategorys.Dtos;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys.Dto;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys;
using Exwhyzee.Wimbig.Data.Repository.MapRaffleToCategorys;

namespace Exwhyzee.Wimbig.Application.MapRaffleToCategorys
{
    public class MapRaffleToCategoryAppService: IMapRaffleToCategoryAppService
    {
        private readonly IMapRaffleToCategoryRepository _raffleCategoryRepo;

        #region Ctro
        public MapRaffleToCategoryAppService(IMapRaffleToCategoryRepository repo)
        {
            _raffleCategoryRepo = repo;
        }
        #endregion

        public async Task<long> Add(MapRaffleToCategoryDto entity)
        {
            try
            {
                var insertedId = await _raffleCategoryRepo.Add(ModelReturnHelper(entity));
                return insertedId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(long id)
        {
            try
            {
                await _raffleCategoryRepo.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MapRaffleToCategoryDto> Get(long id)
        {
            try
            {
                return DtoReturnHelper(await _raffleCategoryRepo.Get(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<MapRaffleToCategoryDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                var result = await _raffleCategoryRepo.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);
                var sourceDtos = new List<MapRaffleToCategoryDto>();
                foreach (var item in result.Source)
                {
                    sourceDtos.Add(DtoReturnHelper(item));
                }
                return new PagedList<MapRaffleToCategoryDto>(source: sourceDtos, pageIndex: 1, pageSize: int.MaxValue, filteredCount: sourceDtos.Count, totalCount: sourceDtos.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<RaffleCagtegoryDto>> GetRafflesByCategory(long id)
        {
            try
            {
                return await _raffleCategoryRepo.GetRafflesByCategory(id); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(MapRaffleToCategoryDto entity)
        {
            try
            {
                await _raffleCategoryRepo.Update(ModelReturnHelper(entity));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Data Transfer Helper
        private static MapRaffleToCategoryDto DtoReturnHelper(MapRaffleToCategory model)
        {
            if (model != null)
            {
                var mapREffleToCategory = new MapRaffleToCategoryDto()
                {
                    Id= model.Id,
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    RaffleId = model.RaffleId,
                    RaffleName = model.RaffleName,
                    DateCreated = model.DateCreated,
                };
                return mapREffleToCategory ;
            }
            return null;
        }

        private static List<MapRaffleToCategoryDto> DtoListReturnHelper(List<MapRaffleToCategory> models)
        {
            if(models != null)
            {
                var outputDtos = new List<MapRaffleToCategoryDto>();
                outputDtos.AddRange(models.OrderBy(order => order.CategoryName).Select(model => new MapRaffleToCategoryDto()
                {
                    Id = model.Id,
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    RaffleId = model.RaffleId,
                    RaffleName = model.RaffleName,
                    DateCreated = model.DateCreated,
                }));

                return outputDtos;
            }
            return null;
        }

        private static MapRaffleToCategory ModelReturnHelper(MapRaffleToCategoryDto model)
        {
            if (model != null)
            {
                var mappings = new MapRaffleToCategory()
                {
                    Id = model.Id,
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    RaffleId = model.RaffleId,
                    RaffleName = model.RaffleName,
                    DateCreated = model.DateCreated,
                };
                return mappings;
            }
            return null;
        }

        public async Task<MapRaffleToCategoryDto> GetByRaffleId(long id)
        {
            try
            {
                return DtoReturnHelper(await _raffleCategoryRepo.GetByRaffleId(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
