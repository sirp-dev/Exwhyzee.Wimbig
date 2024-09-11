using Exwhyzee.Wimbig.Core.BarnerImage;
using Exwhyzee.Wimbig.Core.SideBarner;
using Exwhyzee.Wimbig.Data.Repository.BarnerImages;
using Exwhyzee.Wimbig.Data.Repository.SideBarImages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Barner
{
    public class BarnerAppService : IBarnerAppService
    {
        private IBarnerRepository _barnerRepository;
        private ISideBarImageRepository _sideBarImageRepository;
        public BarnerAppService(IBarnerRepository barnerRepository, ISideBarImageRepository sideBarImageRepository)
        {
            _barnerRepository = barnerRepository;
            _sideBarImageRepository = sideBarImageRepository;
        }

        public async Task Delete(long Id)
        {
            try
            {
               await _barnerRepository.Delete(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteSideBarner(long Id)
        {
            try
            {
                await _sideBarImageRepository.Delete(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        public async Task<PagedList<BarnerFile>> GetBarnerFile(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<BarnerFile> raffles = new List<BarnerFile>();
            var paggedBarner = await _barnerRepository.GetAllBarner(dateStart, dateEnd, startIndex, count, searchString);
            var paggedSource = paggedBarner.Source;

            
            return new PagedList<BarnerFile>(source: paggedSource, pageIndex: paggedBarner.PageIndex,
                                            pageSize: paggedBarner.PageSize, filteredCount: paggedBarner.FilteredCount, totalCount:
                                            paggedBarner.TotalCount);
        }

        public async Task<PagedList<SideBarnerFile>> GetBarnerFileSideBarner(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<SideBarnerFile> images = new List<SideBarnerFile>();
            var paggedBarner = await _sideBarImageRepository.GetAllSideBarImage(dateStart, dateEnd, startIndex, count, searchString);
            var paggedSource = paggedBarner.Source;


            return new PagedList<SideBarnerFile>(source: paggedSource, pageIndex: paggedBarner.PageIndex,
                                            pageSize: paggedBarner.PageSize, filteredCount: paggedBarner.FilteredCount, totalCount:
                                            paggedBarner.TotalCount);
        }

        public Task<BarnerFile> GetById(long Id)
        {
            throw new NotImplementedException();
        }

     

        public async Task<long> Insert(BarnerFile image)
        {
            try
            {
                image.Extension = image.Extension.Substring(2);
                return await _barnerRepository.Insert(image);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> InsertSideBarner(SideBarnerFile image)
        {
            try
            {
                image.Extension = image.Extension.Substring(2);
                return await _sideBarImageRepository.Insert(image);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
