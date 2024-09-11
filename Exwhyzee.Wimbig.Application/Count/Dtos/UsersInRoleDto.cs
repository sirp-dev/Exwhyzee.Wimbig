using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Count.Dtos
{
   public class UsersInRoleDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public string CurrentCity { get; set; }
        public int Percentage { get; set; }
        public string AreaInCurrentCity { get; set; }

        public string RoleString { get; set; }

    }
}
