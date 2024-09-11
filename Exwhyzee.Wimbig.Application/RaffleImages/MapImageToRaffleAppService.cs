using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Data.Repository.ImageFiles.RaffleImage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.RaffleImages
{
    public class MapImageToRaffleAppService : IMapImageToRaffleAppService
    {
        private IMapImageToRaffleRepo _repo;

        public MapImageToRaffleAppService(IMapImageToRaffleRepo repo)
        {
            _repo = repo;
        }

        public async Task Delete(long id)
        {
            try
            {
               await _repo.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ImageOfARaffle>> GetAllImagesOfARaffle(long raffleId, int? count = int.MaxValue)
        {
            try
            {
                return await _repo.GetAllImagesOfARaffle(raffleId, count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MapImageToRaffle> GetById(long id)
        {
            try
            {
                return await _repo.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
                    
            }
        }

        public async Task<MapImageToRaffle> GetByRaffleId(long id)
        {
            try
            {
                return await _repo.GetByRaffleId(id);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<long> InsertMap(MapImageToRaffle model)
        {
            try
            {
                return await _repo.InsertMap(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task Update(MapImageToRaffle model)
        {
            try
            {
                await _repo.Update(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
