using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ForgotPasswordConfirmation(UserManager<ApplicationUser> userManager

           )
        {

            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }



        public class InputModel
        {
            [Required]

            public string CodeVerify { get; set; }
            public string Id { get; set; }

        }
        [TempData]
        public string StatusMessage { get; set; }
        public string LoggedInUser { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {

            LoggedInUser = id;
            var user = await _userManager.FindByIdAsync(id);
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
            ViewData["usernumber"] = user.PhoneNumber.Substring(user.PhoneNumber.Length - 4).ToLower();

            }
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                string a = "";
                string b = "";
                try
                {
  a = user.Email.Substring(user.Email.Length - 10);
                }catch(Exception d) { }
                try
                {
                     b = user.Email.Substring(0, 4);
                }
                catch (Exception d) { }
              
                
                ViewData["usermail"] = b.ToLower() + "***" + a.ToLower();
            }
            return Page();


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(Input.Id);
                    if (user.CodeVerify == Input.CodeVerify)
                    {
                        
                        return RedirectToPage("./ResetPassword", new { id = user.Id });
                    }
                    StatusMessage = "Error! Wrong Code";
                    return Page();
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


    }
}
