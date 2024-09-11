using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.App.Services
{
    public class UserManager:IUserManager
    {
        Dictionary<string, string> _userCredentials;
        public UserManager()
        {
            //TODO: Remove and add real login
            _userCredentials = new Dictionary<string, string>
            {
                { "us@sad.com", "aaaaaaaa" },
                { "user2@sad.com", "Userabc123" },
                { "user3@sad.com", "!A@3534" }
            };
        }

        public async Task<bool> Login(string username, string password)
        {
            if (_userCredentials.ContainsKey(username))
            {
                return _userCredentials[username] == password;
            }

            return false;
        }
    }
}
