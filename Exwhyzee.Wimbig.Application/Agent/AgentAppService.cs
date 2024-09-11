using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Agent.Dto;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.DailyStatistics.Dto;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Agent;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Categories;
using Exwhyzee.Wimbig.Core.DailyStatistics;
using Exwhyzee.Wimbig.Data.Repository.Agent;
using Exwhyzee.Wimbig.Data.Repository.Categorys;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Exwhyzee.Wimbig.Data.Repository.DailyStatistics;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Agent
{
    public class AgentAppService : IAgentAppService
    {
        private readonly IAgentRepository _agentRepository;

        public AgentAppService(IAgentRepository agentRepository
           )
        {
            _agentRepository = agentRepository;

        }

        public async Task<long> Add(AgentDto entity)
        {
            try
            {
                var agent = new Agents()
                {
                    Fullname = entity.Fullname,
                    EmailAddress = entity.EmailAddress,
                    State = entity.State,
                    LGA = entity.LGA,
                    AreYouNewToWimbig = entity.AreYouNewToWimbig,
                    ContactAddress = entity.ContactAddress,
                    DateCreated = DateTime.UtcNow.AddHours(1),
                    CurrentOccupation = entity.CurrentOccupation,
                    PhoneNumber = entity.PhoneNumber,
                    ShopLocation = entity.ShopLocation,
                    Gender = entity.Gender,
                    Status = AgentStatusEnum.Pending
                };

                return await _agentRepository.Add(agent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<AgentDto>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {

            List<AgentDto> agent = new List<AgentDto>();

            var query = await _agentRepository.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);

            agent.AddRange(query.Source.Select(entity => new AgentDto()
            {
                Fullname = entity.Fullname,
                EmailAddress = entity.EmailAddress,
                State = entity.State,
                LGA = entity.LGA,
                AreYouNewToWimbig = entity.AreYouNewToWimbig,
                ContactAddress = entity.ContactAddress,
                DateCreated = entity.DateCreated,
                CurrentOccupation = entity.CurrentOccupation,
                PhoneNumber = entity.PhoneNumber,
                ShopLocation = entity.ShopLocation,
                Gender = entity.Gender,
                Status = entity.Status

            }));

            return new PagedList<AgentDto>(source: agent, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        public async Task<AgentDto> Get(long id)
        {
            try
            {
                var entity = await _agentRepository.Get(id);

                var data = new AgentDto
                {
                    Fullname = entity.Fullname,
                    EmailAddress = entity.EmailAddress,
                    State = entity.State,
                    LGA = entity.LGA,
                    AreYouNewToWimbig = entity.AreYouNewToWimbig,
                    ContactAddress = entity.ContactAddress,
                    DateCreated = entity.DateCreated,
                    CurrentOccupation = entity.CurrentOccupation,
                    PhoneNumber = entity.PhoneNumber,
                    ShopLocation = entity.ShopLocation,
                    Gender = entity.Gender,
                    Status = entity.Status
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task Update(AgentDto entity)
        {
            try
            {


                var data = new Agents
                {
                    Id = entity.Id,
                    Fullname = entity.Fullname,
                    EmailAddress = entity.EmailAddress,
                    State = entity.State,
                    LGA = entity.LGA,
                    AreYouNewToWimbig = entity.AreYouNewToWimbig,
                    ContactAddress = entity.ContactAddress,
                    DateCreated = entity.DateCreated,
                    CurrentOccupation = entity.CurrentOccupation,
                    PhoneNumber = entity.PhoneNumber,
                    ShopLocation = entity.ShopLocation,
                    Gender = entity.Gender,
                    Status = entity.Status

                };

                await _agentRepository.Update(data);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
