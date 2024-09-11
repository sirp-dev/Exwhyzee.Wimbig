

using Exwhyzee.Wimbig.Application.YoutubeLink.Dto;
using Exwhyzee.Wimbig.Core.YoutubeLink;
using Exwhyzee.Wimbig.Data.Repository.YoutubeLink;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.YoutubeLink
{
    public class YoutubeLinkAppService : IYoutubeLinkAppService
    {
        private readonly IYoutubeLinkRepository _youtubeLinkRepository;
       

        public YoutubeLinkAppService(IYoutubeLinkRepository youtubeLinkRepository
            )
        {
            _youtubeLinkRepository = youtubeLinkRepository;
           
        }

        public async Task<long> Add(YoutubeLinkDto entity)
        {
           try
            {
                var report = new YoutubeLinks()
                {
                    Url = entity.Url,
                    Title = entity.Title,
                    DateCreated = entity.DateCreated
                   

                };

                return await _youtubeLinkRepository.Add(report);
    }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }



        public async Task<PagedList<YoutubeLinkDto>> GetAsync(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            
            List<YoutubeLinkDto> statistic = new List<YoutubeLinkDto>();

            var query = await _youtubeLinkRepository.GetAsync(dateStart, dateEnd, startIndex, count, searchString);

            statistic.AddRange(query.Source.Select(entity => new YoutubeLinkDto()
            {
                Id = entity.Id,
                Title = entity.Title,
                Url = entity.Url,
                DateCreated = entity.DateCreated

            }));

            return new PagedList<YoutubeLinkDto>(source: statistic, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<YoutubeLinkDto> Get(long id)
        {
            try
            {
                var entity = await _youtubeLinkRepository.Get(id);

                var data = new YoutubeLinkDto
                {
                    Id = entity.Id,
                    Url = entity.Url,
                    Title = entity.Title,
                    DateCreated = entity.DateCreated
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        public async Task Update(YoutubeLinkDto entity)
        {
            try
            {


                var data = new YoutubeLinks
                {
                    Id = entity.Id,
                    Url = entity.Url,
                    Title = entity.Title,
                    DateCreated = entity.DateCreated

                };

               await _youtubeLinkRepository.Update(data);


                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(long Id)
        {
            try
            {
                await _youtubeLinkRepository.Delete(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
