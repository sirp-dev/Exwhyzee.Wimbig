using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin")]
    public class EditRoleModel : PageModel
    {
      

        private readonly RoleManager<ApplicationRole> _roleManager;


        public EditRoleModel(
            RoleManager<ApplicationRole> roleManager

           )
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationRole Role { get; set; }

        //[TempData]
        //public string StatusMessage { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Unable to load raffle with the ID '{id}'.");
            }

            Role = role;
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Unable to load Raffle with the ID '{id}'.");
            }

            role.Name = Role.Name;
            await _roleManager.UpdateAsync(role);

            //StatusMessage = "The Selected Raffle has been updated";
            return RedirectToPage("./Roles");
        }
    }
}