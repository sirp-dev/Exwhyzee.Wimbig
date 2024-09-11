using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Id { get; set; }
        }

        public string LoggedInUser { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
if (id == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            LoggedInUser = id;

            return Page();
            
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Page(); ;
            }

            try
            {


                //var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
                var result = await _userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    var AddPassword = await _userManager.AddPasswordAsync(user, Input.Password);
                    if (AddPassword.Succeeded)
                    {
                        return RedirectToPage("./ResetPasswordConfirmation");
                    }

                    foreach (var error in AddPassword.Errors)
                    {
                       ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception d)
            {
               
            }
           
                return Page();
        }
    }
}
