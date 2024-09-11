
using Exwhyzee.Wimbig.Core.SmsMessages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.SmsMessages
{
    public interface ISmsMessageRepository : IDisposable
    {
        // define extra specific members here.
    
        Task<PagedList<SmsMessage>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<SmsMessage> Get(long id);

     
        Task<long> Add(SmsMessage entity);

        Task Delete(long id);

        Task Update(SmsMessage entity);
    }
}
