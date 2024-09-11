using Exwhyzee.Wimbig.Core.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Services.Users
{
    public interface IUserAppService
    {
        Task<ApplicationUser> Authenticate(string username, string password);
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task<string> Create(ApplicationUser user, string password);
        Task<bool> Update(ApplicationUser user, string password);
        Task<bool> Delete(ApplicationUser user);

        Task<bool> CreateRole(ApplicationRole role);
        Task<bool> CreateUserRole(ApplicationUser user, string roleName);

    }
}
