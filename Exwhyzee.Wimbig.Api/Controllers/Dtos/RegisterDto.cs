using Exwhyzee.Core.Mvc.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone"), RegularExpression(@"^[0]\d{10}$", ErrorMessage = "Error! Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Other Names")]
        public string OtherNames { get; set; }

        public string CodeVerify { get; set; }


        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [EnsureMinimumAge(18, ErrorMessage = "Sorry, You are not Eligible to Play Small and Winbig... Date of Birth must be 18years +")]
        public DateTime DateOfBirth { get; set; }

        public long UniqueId { get; set; }

        [Required]
        [Display(Name = "Current City")]
        public string CurrentCity { get; set; }
    }
}
