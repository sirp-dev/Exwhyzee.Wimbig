using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<PagedList<T>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);
         
        Task<T> Get(long id);

        Task<long> Add(T entity);

        Task Delete(long id);

        Task Update(T entity);       
    }
}
