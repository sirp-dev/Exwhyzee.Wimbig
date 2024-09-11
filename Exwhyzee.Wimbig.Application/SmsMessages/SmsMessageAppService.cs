
using Exwhyzee.Wimbig.Application.SmsMessages.Dto;
using Exwhyzee.Wimbig.Core.SmsMessages;
using Exwhyzee.Wimbig.Data.Repository.SmsMessages;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.SmsMessages
{
    public class SmsMessageAppService : ISmsMessageAppService
    {
        private readonly ISmsMessageRepository _smsMessageRepository;


        public SmsMessageAppService(ISmsMessageRepository smsMessageRepository
           )
        {
            _smsMessageRepository = smsMessageRepository;

        }

        public async Task<long> Add(SmsMessageDto entity)
        {
            try
            {
                var data = new SmsMessage()
                {
                    SenderId = entity.SenderId,
                    DateCreated = entity.DateCreated,
                    Recipient = entity.Recipient,
                    Message = entity.Message,
                    Status = entity.Status,
                    Response = entity.Response

                };

                return await _smsMessageRepository.Add(data);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public async Task<PagedList<SmsMessageDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {

            List<SmsMessageDto> data = new List<SmsMessageDto>();

            var query = await _smsMessageRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);

            data.AddRange(query.Source.Select(entity => new SmsMessageDto()
            {
                Id = entity.Id,
                SenderId = entity.SenderId,
                DateCreated = entity.DateCreated,
                Recipient = entity.Recipient,
                Message = entity.Message,
                Status = entity.Status,
                Response = entity.Response

            }));

            return new PagedList<SmsMessageDto>(source: data, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<SmsMessageDto> Get(long id)
        {
            try
            {
                var entity = await _smsMessageRepository.Get(id);

                var data = new SmsMessageDto
                {
                    Id = entity.Id,
                    SenderId = entity.SenderId,
                    DateCreated = entity.DateCreated,
                    Recipient = entity.Recipient,
                    Message = entity.Message,
                    Status = entity.Status,
                    Response = entity.Response
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task Update(SmsMessageDto entity)
        {
            try
            {


                var data = new SmsMessage
                {
                    Id = entity.Id,
                    SenderId = entity.SenderId,
                    DateCreated = entity.DateCreated,
                    Recipient = entity.Recipient,
                    Message = entity.Message,
                    Status = entity.Status,
                    Response = entity.Response

                };

                await _smsMessageRepository.Update(data);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
