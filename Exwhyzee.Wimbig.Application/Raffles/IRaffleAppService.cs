using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Raffles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Raffles
{
    public interface IRaffleAppService
    {

        Task<PagedList<RaffleDto>> GetAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PagedList<RaffleDto>> GetRafflesByHostedBy(string hostedBy, int? status = null, int startIndex = 0, int count = int.MaxValue);

        Task<PagedList<Raffle>> GetAllRaffles(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PagedList<RaffleDto>> GetRaffleByLocationAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PagedList<RaffleDto>> GetRafflesByArchieved(bool archieved = true, int? status = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<RaffleDto> GetById(long id);

        Task<long> Add(RaffleDto entity);

        Task Delete(long id);

        Task Update(RaffleDto entity);

        Task<List<RaffleDto>> GetRaffleByStatus(int status, int count);

        Task<List<RaffleListDto>> GetRaffleList(long raffleId);

        Task<bool> AddToArchieve(long id);

        Task<bool> RemoveFromArchieve(long id);

    }
}
