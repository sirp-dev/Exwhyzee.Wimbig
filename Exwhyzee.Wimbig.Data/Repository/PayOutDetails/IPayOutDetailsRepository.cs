
using Exwhyzee.Wimbig.Core.PayOutDetails;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.PayOutDetails
{
    public interface IPayOutDetailsRepository : IDisposable
    {
        // define extra specific members here.
    
        Task<PagedList<PayOutDetail>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<PayOutDetail> Get(long id);

        Task<long> Add(PayOutDetail entity);

     //   Task Delete(long id);

        Task Update(PayOutDetail entity);
    }
}
