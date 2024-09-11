using Exwhyzee.Wimbig.Application.Sections.Dto;
using Exwhyzee.Wimbig.Core.Sections;
using Exwhyzee.Wimbig.Data.Repository.Sections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Sections
{
    public interface ISectionAppService 
    {
        Task<PagedList<SectionDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<SectionDto> Get(long id);

        Task<long> Add(CreateSectionDto entity);

        Task Delete(long id);

        Task Update(SectionDto entity);
    }
}
