using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Exwhyzee.Wimbig.Core.RaffleImages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.ImageFiles.RaffleImage
{
    public interface IMapImageToRaffleRepo : IDisposable
    {
        Task<long> InsertMap(MapImageToRaffle model);

        Task Update(MapImageToRaffle model);

        Task Delete(long id);

        Task<MapImageToRaffle> GetById(long id);

        Task<MapImageToRaffle> GetByRaffleId(long id);

        //gets all images mapped to a raffle when the count is null or not set. 
        //Note to get the default image of the raffle pass the count parameter as 1
        Task<IEnumerable<ImageOfARaffle>> GetAllImagesOfARaffle(long raffleId, int? count= int.MaxValue);
    }
}
