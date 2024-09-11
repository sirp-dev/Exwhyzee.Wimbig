using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Authorization.Users
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
            DateRegistered = DateTime.UtcNow;
        }

        [PersonalData]
        [Required]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        public string LastName { get; set; }

        [PersonalData]
        public string OtherNames { get; set; }

        [PersonalData]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [PersonalData]
        public DateTime DateRegistered { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
