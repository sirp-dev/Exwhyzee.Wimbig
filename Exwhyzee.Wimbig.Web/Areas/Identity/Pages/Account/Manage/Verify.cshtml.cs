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

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]
    public class VerifyModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public VerifyModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager

           )
        {
           
            _userManager = userManager;
            _signInManager = signInManager;
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

            return Page();


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(Input.Id);
                    if(user.CodeVerify == Input.CodeVerify)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return Redirect("/");
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