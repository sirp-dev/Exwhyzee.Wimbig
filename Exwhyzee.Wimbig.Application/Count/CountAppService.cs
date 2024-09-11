using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Count.Dtos;
using Exwhyzee.Wimbig.Core.Categories;
using Exwhyzee.Wimbig.Data.Repository.Categorys;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Exwhyzee.Wimbig.Data.Repository.Count;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Count
{
    public class CountAppService: ICountAppService
    {
        private readonly ICountRepository _countRepository;

        public CountAppService(ICountRepository countRepository)
        {
            _countRepository = countRepository;
        }

        public async Task<decimal> Amount(int status = 0)
        {
            return await _countRepository.TotalAmount(status: 2);
        }

        public async Task<PagedList<UsersInRoleDto>> UsersInRole(string roleid = null, int startIndex = 0, int count = int.MaxValue)
        {
            try
            {
                List<UsersInRoleDto> statistic = new List<UsersInRoleDto>();

                var query = await _countRepository.UsersInRole(roleid, startIndex, count);

                statistic.AddRange(query.Source.Select(entity => new UsersInRoleDto()
                {
                    Id = entity.Id,
                    UserName = entity.UserName,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    DateRegistered = entity.DateRegistered,
                    FullName = entity.FullName,
                    Balance = entity.Balance,
                    CurrentCity = entity.CurrentCity,
                    
                    Percentage = entity.Percentage,
                    AreaInCurrentCity = entity.AreaInCurrentCity,
                    RoleString = entity.RoleString


                }));

                return new PagedList<UsersInRoleDto>(source: statistic, pageIndex: startIndex, pageSize: count,
                    filteredCount: query.FilteredCount,
                    totalCount: query.TotalCount);
            }catch(Exception c)
            {
                return null;
            }
        }

        public async Task<PagedList<UsersInRoleDto>> UsersInRoleForDgaAgentSup(string roleid = null, int startIndex = 0, int count = int.MaxValue)
        {
            List<UsersInRoleDto> statistic = new List<UsersInRoleDto>();

            var query = await _countRepository.UsersInRoleForDgaAgentSup(roleid, startIndex, count);

            statistic.AddRange(query.Source.Select(entity => new UsersInRoleDto()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                RoleName = entity.RoleName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                DateRegistered = entity.DateRegistered,
                FullName = entity.FullName,
                Balance = entity.Balance,
                CurrentCity = entity.CurrentCity,
                Percentage = entity.Percentage


            }));

            return new PagedList<UsersInRoleDto>(source: statistic, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }
    }
}
