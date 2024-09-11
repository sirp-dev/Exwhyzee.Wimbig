using Exwhyzee.Wimbig.Core.Raffles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Raffles
{
    public interface IRaffleRepository: IRepository<Raffle>
    {
        Task<PagedList<Raffle>> GetRafflesByHostedBy(string hostedBy,int? status = null, int startIndex = 0, int count = int.MaxValue);

        Task<PagedList<Raffle>> GetRafflesByArchieved(bool archieved = true,int? status = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<List<Raffle>> GetRafflesByStatus(int status, int count);

        Task<PagedList<Raffle>> GetAsyncAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PagedList<Raffle>> GetRaffleByLocationAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);


        Task<bool> AddToArchieve(long id);

        Task<bool> RemoveFromArchieve(long id);
    }
}
