using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Categories
{
    public interface ICategoryAppService
    {
        Task<PagedList<CategorySectionDetailsDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<IEnumerable<CategoryDto>> GetCategoriesBySection(long sectionId);

        Task<CategoryDto> Get(long id);

        Task<long> Add(CreateCategoryDto entity);

        Task Delete(long id);

        Task Update(CategoryDto entity);
    }
}
