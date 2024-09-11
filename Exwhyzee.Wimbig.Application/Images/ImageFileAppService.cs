using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Data.Repository.ImageFiles;

namespace Exwhyzee.Wimbig.Application.Images
{
    public class ImageFileAppService : IImageFileAppService
    {
        private IImageFileRepository _imageFileRepository;
        public ImageFileAppService(IImageFileRepository imageFileRepository)
        {
            _imageFileRepository = imageFileRepository;
        }

        public async Task Delete(long Id)
        {
            try
            {
               await _imageFileRepository.Delete(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ImageFile> GetById(long Id)
        {
            try
            {
                return await _imageFileRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> Insert(ImageFile image)
        {
            try
            {
                image.Extension = image.Extension.Substring(2);
                return await _imageFileRepository.Insert(image);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Upate(ImageFile image)
        {
            try
            {
                await _imageFileRepository.Upate(image);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<ImageFile>> GetAllImages(string extension = null, DateTime? dateStart = null, DateTime? dateStop = null, int startIndex = 0, int count = int.MaxValue)
        {
            List<ImageFile> imageFile = new List<ImageFile>();
            var paggedimages = await _imageFileRepository.GetAsyncAll(extension, dateStart, dateStop, startIndex, count);

            var paggedSource = paggedimages;


            //imageFile.AddRange(paggedSource.Select(x => new Raffle()
            //{
            //    DeliveryType = x.DeliveryType,
            //    Description = x.Description,
            //    EndDate = x.EndDate,
            //    HostedBy = x.HostedBy,
            //    Id = x.Id,
            //    Name = x.Name,
            //    NumberOfTickets = x.NumberOfTickets,
            //    PricePerTicket = x.PricePerTicket,
            //    StartDate = x.StartDate,
            //    Status = x.Status,
            //    Username = x.Username,
            //    DateCreated = x.DateCreated,
            //    TotalSold = x.TotalSold,
            //    SortOrder = x.SortOrder,
            //    Archived = x.Archived,
            //    PaidOut = x.PaidOut,
            //    Location = x.Location

            //}));

            return new PagedList<ImageFile>(source: paggedimages.Source, pageIndex: paggedimages.PageIndex,
                                            pageSize: paggedimages.PageSize, filteredCount: paggedimages.FilteredCount, totalCount:
                                            paggedimages.TotalCount);


        }


    }
}
