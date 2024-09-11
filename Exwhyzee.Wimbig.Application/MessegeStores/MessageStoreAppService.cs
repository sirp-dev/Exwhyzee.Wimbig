using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.MessageStores.Dto;
using Exwhyzee.Wimbig.Core.Categories;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.Categorys;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.NotificationService.Abstract;
using Exwhyzee.Wimbig.NotificationService.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.MessageStores
{
    public class MessageStoreAppService : IMessageStoreAppService
    {
        private readonly IMessageStoreRepository _messageStoreRepository;
       

        public MessageStoreAppService(IMessageStoreRepository messageStoreRepository
           )
        {
            _messageStoreRepository = messageStoreRepository;
            
        }


        public async Task<PagedList<MessageStoreDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                List<MessageStoreDto> data = new List<MessageStoreDto>();

                var query = await _messageStoreRepository.GetAsyncAllMessege(status, dateStart, dateEnd, startIndex, count, searchString);

                data.AddRange(query.Source.Select(entity => new MessageStoreDto()
                {
                    Id = entity.Id,
                    DateCreated = entity.DateCreated,
                    Message = entity.Message,
                    MessageStatus = entity.MessageStatus,
                    Response = entity.Response,
                    EmailAddress = entity.EmailAddress,
                    PhoneNumber = entity.PhoneNumber,
                    DateSent = entity.DateSent

                }));



                return new PagedList<MessageStoreDto>(source: data, pageIndex: startIndex, pageSize: count,
                    filteredCount: query.FilteredCount,
                    totalCount: query.TotalCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageStoreDto> Get(long id)
        {
            try
            {
                var item = await _messageStoreRepository.Get(id);

                var data = new MessageStoreDto
                {
                    Id = item.Id,
                    PhoneNumber = item.PhoneNumber,
                    Message = item.Message,
                    EmailAddress = item.EmailAddress,
                    MessageStatus = item.MessageStatus,
                    MessageChannel = item.MessageChannel,
                    MessageType = item.MessageType,
                    Retries = item.Retries,
                    DateCreated = item.DateCreated,
                    DateSent = item.DateSent,
                    AddressType = item.AddressType,
                    ImageUrl = item.ImageUrl,
                    Response = item.Response



                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task Update(MessageStoreDto entity)
        {
            try
            {


                var data = new MessageStore
                {
                    MessageStatus = entity.MessageStatus,
                    DateSent = entity.DateSent,
                    Id = entity.Id,
                    Retries = entity.Retries,
                    Response = entity.Response,
                    EmailAddress = entity.EmailAddress,
                    PhoneNumber = entity.PhoneNumber,
                    MessageChannel = entity.MessageChannel,
                    MessageType = entity.MessageType,

                    DateCreated = entity.DateCreated,
                    Message = entity.Message,
                    ImageUrl = entity.ImageUrl,
                    AddressType = entity.AddressType

                };

                await _messageStoreRepository.Update(data);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}

