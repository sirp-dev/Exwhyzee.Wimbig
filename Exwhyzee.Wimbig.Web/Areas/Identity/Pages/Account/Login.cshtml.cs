using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Exwhyzee.Wimbig.Core.Authorization.Users;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            try
            {
                if (ModelState.IsValid)
                {

                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        var user = await _userManager.FindByNameAsync(Input.UserName);
                        _logger.LogInformation("User logged in.");
                        if (returnUrl != null)
                        {
                            return LocalRedirect(returnUrl);
                        }
                        else if (await _signInManager.UserManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToPage("Index", "Users", "UserManagement");

                        }
                        else if (await _signInManager.UserManager.IsInRoleAsync(user, "SuperAdmin"))
                        {
                            return RedirectToPage("Index", "Dashboard", "SuperAdmin");
                        }
                        else if (await _signInManager.UserManager.IsInRoleAsync(user, "Agent"))
                        {
                            return RedirectToPage("Index", "Agents", "StakeHolders");
                        }
                        else if (await _signInManager.UserManager.IsInRoleAsync(user, "SuperAgent"))
                        {

                            return RedirectToPage("Index", "SuperAgents", "StakeHolders");
                        }
                        else if (await _signInManager.UserManager.IsInRoleAsync(user, "Supervisor"))
                        {
                            return RedirectToPage("Index", "Supervisor", "StakeHolders");

                        }
                        else if (await _signInManager.UserManager.IsInRoleAsync(user, "DGA"))
                        {
                            return RedirectToPage("Index", "DGAs", "StakeHolders");

                        }
                        else if (await _signInManager.UserManager.IsInRoleAsync(user, "User"))
                        {
                            return RedirectToPage("Index", "Raffles", "Raffles");

                        }
                        else
                        {
                            return RedirectToPage("/");
                        }


                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                       
                        return Page();
                    }
                }

                // If we got this far, something failed, redisplay form
            }catch(Exception e)
            {

            }
            return Page();
        }
    }
}
