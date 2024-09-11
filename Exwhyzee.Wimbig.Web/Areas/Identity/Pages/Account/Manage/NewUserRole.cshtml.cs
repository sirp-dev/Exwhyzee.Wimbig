using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    public class NewUserRoleModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
       

        public NewUserRoleModel(
            RoleManager<ApplicationRole> roleManager
           
           )
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
         
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole
                {
                   Name = Input.Name
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                   
                    return RedirectToPage("Roles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}