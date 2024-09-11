using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Raffles;
using Exwhyzee.Wimbig.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Count
{
    public interface ICountRepository
    {
       
        Task<int> TotalUsers();
        Task<int> TotalAgents(string rolename);
        Task<int> TotalDgas(string rolename);
        Task<int> TotalTicket();
        Task<int> TotalTicket(string userid = null);
        Task<decimal> TotalAmount(int status = 0);

        Task<PagedList<UsersInRole>> UsersInRole(string roleid = null, int startIndex = 0, int count = int.MaxValue);
        Task<PagedList<UsersInRole>> UsersInRoleForDgaAgentSup(string roleid = null, int startIndex = 0, int count = int.MaxValue);


    }
}
