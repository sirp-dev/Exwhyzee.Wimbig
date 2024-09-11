using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Authorization.Users
{
    public class ApplicationRole: IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
