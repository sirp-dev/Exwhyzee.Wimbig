
using Exwhyzee.Wimbig.Application.SmsMessages.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.SmsMessages
{
    public interface ISmsMessageAppService
    {
        Task<PagedList<SmsMessageDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

     
        Task<SmsMessageDto> Get(long id);

    
        Task<long> Add(SmsMessageDto entity);

        Task Update(SmsMessageDto entity);
    }
}
