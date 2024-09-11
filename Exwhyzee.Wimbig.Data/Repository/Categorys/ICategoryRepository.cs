using Exwhyzee.Wimbig.Core.Categories;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Categorys
{
    public interface ICategoryRepository : IDisposable
    {
        // define extra specific members here.
        Task<List<Category>> GetCategoriesBySection(long id);

        Task<PagedList<CategorySectionDetailsDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<Category> Get(long id);

        Task<long> Add(Category entity);

        Task Delete(long id);

        Task Update(Category entity);
    }
}
