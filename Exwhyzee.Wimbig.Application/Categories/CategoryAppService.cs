using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Core.Categories;
using Exwhyzee.Wimbig.Data.Repository.Categorys;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Categories
{
    public class CategoryAppService: ICategoryAppService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAppService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<long> Add(CreateCategoryDto model)
        {
            try
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Description = model.Description,
                    SectionId = model.SectionId,
                    DateCreated = model.DateCreated,
                    EntityStatus = model.EntityStatus
                };

                return await _categoryRepository.Add(category);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(long id)
        {
            try
            {
                await _categoryRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryDto> Get(long id)
        {
            try
            {
                var result = await _categoryRepository.Get(id);
                return DtoReturnHelper(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<CategorySectionDetailsDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                var result = await _categoryRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);
                var sourceDtos = new List<CategorySectionDetailsDto>();
                var sections = result.Source;

                foreach (var item in sections)
                {
                    sourceDtos.Add(item);
                }
                return new PagedList<CategorySectionDetailsDto>(source: sourceDtos, pageIndex: 1, pageSize: int.MaxValue, filteredCount: sourceDtos.Count, totalCount: sourceDtos.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesBySection(long sectionId)
        {
            try
            {
                var result = await _categoryRepository.GetCategoriesBySection(sectionId);
                var categoriesDto = new List<CategoryDto>();

                categoriesDto.AddRange(result.OrderBy(x=>x.Name).Select(model=> new CategoryDto() {
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    Description = model.Description,
                    DateCreated = model.DateCreated,
                    EntityStatus = model.EntityStatus,
                    SectionId = model.SectionId
                }));

                return categoriesDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(CategoryDto entity)
        {
            try
            {
                await _categoryRepository.Update(ModelReturnHelper(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Data Transfer Helper
        private static CategoryDto DtoReturnHelper(Category model)
        {
            if (model != null)
            {
                var categoryDto = new CategoryDto()
                {                  
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    Description = model.Description,
                    DateCreated = model.DateCreated,
                    EntityStatus = model.EntityStatus,
                    SectionId = model.SectionId
                };
                return categoryDto;
            }
            return null;
        }

        private static Category ModelReturnHelper(CategoryDto model)
        {
            if (model != null)
            {
                var category = new Category()
                {
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    Description = model.Description,
                    DateCreated = model.DateCreated,
                    EntityStatus = model.EntityStatus,
                    SectionId = model.SectionId
                };
                return category;
            }
            return null;
        }
        #endregion
    }
}
