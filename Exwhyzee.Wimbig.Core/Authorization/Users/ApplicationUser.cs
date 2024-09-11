using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Authorization.Users
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            DateRegistered = DateTime.UtcNow.AddHours(1);
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

        [PersonalData]
        public string CodeVerify { get; set; }

        [PersonalData]
        public string ReferenceId { get; set; }

        [PersonalData]
        public string UserDescription { get; set; }

        [PersonalData]
        public DateTime PayoutStartDate { get; set; }

        [PersonalData]
        public DateTime PayoutEndDate { get; set; }

        [PersonalData]
        public string UniqueId { get; set; }

        [NotMapped]
        public string FullName { get { return LastName + ", " + FirstName; } }

        [NotMapped]
        public string NameDescription { get { return LastName + ", " + FirstName + " (" + UserDescription + ")"; } }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public string CurrentCity { get; set; }

        public string ContactAddress { get; set; }

        public string AreaInCurrentCity { get; set; }

        public string RoleString { get; set; }
    }
}
