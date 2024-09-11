using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Count.Dtos;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Count
{
    public interface ICountAppService
    {
        Task<decimal> Amount(int status = 0);
        Task<PagedList<UsersInRoleDto>> UsersInRole(string roleid = null, int startIndex = 0, int count = int.MaxValue);
        Task<PagedList<UsersInRoleDto>> UsersInRoleForDgaAgentSup(string roleid = null, int startIndex = 0, int count = int.MaxValue);

    }
}
