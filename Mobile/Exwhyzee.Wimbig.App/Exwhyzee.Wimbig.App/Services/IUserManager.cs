using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.App.Services
{
    public interface IUserManager
    {
        Task<bool> Login(string username, string password);
    }
}
