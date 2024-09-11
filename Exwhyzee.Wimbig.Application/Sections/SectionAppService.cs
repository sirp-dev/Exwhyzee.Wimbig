using Exwhyzee.Wimbig.Application.Sections.Dto;
using Exwhyzee.Wimbig.Core.Sections;
using Exwhyzee.Wimbig.Data.Repository.Sections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Sections
{
    public class SectionAppService : ISectionAppService
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionAppService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }
        public async Task<long> Add(CreateSectionDto model)
        {
            try
            {
                var entity = new Section()
                {
                    Name = model.Name,
                    Description = model.Description,
                    SectionId = model.SectionId,
                    DateCreated = model.DateCreated,
                    EntityStatus = model.EntityStatus
                };

              return  await _sectionRepository.Add(entity);
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
                await _sectionRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SectionDto> Get(long id)
        {
            try
            {
                var result = await _sectionRepository.Get(id);
                return DtoReturnHelper(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<SectionDto>> GetAsync(int? status = 0, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                var result = await _sectionRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);
                var sourceDtos = new List<SectionDto>();
                var sections = result.Source;

                foreach (var item in sections)
                {
                    sourceDtos.Add(DtoReturnHelper(item));
                }

                return new PagedList<SectionDto>(source: sourceDtos, pageIndex: 1, pageSize: int.MaxValue, filteredCount: count, totalCount: count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(SectionDto model)
        {
            try
            {
                var entity = new Section()
                {
                    SectionId = model.SectionId,
                    Name = model.Name,
                    Description = model.Description,
                    DateCreated = model.DateCreated,
                    EntityStatus = model.EntityStatus,
                };
                await _sectionRepository.Update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Data Transfer Helper
        private static SectionDto DtoReturnHelper(Section model)
        {
            if (model != null)
            {
                var sectionDto = new SectionDto()
                {
                    SectionId = model.SectionId,
                    Name = model.Name,
                    Description = model.Description,
                    DateCreated = model.DateCreated,
                    EntityStatus = model.EntityStatus,
                };
                return sectionDto;
            }
            return null;
        }
        #endregion
    }
}
