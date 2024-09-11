using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.UserManagement.Pages.Users
{

    [Authorize(Roles = "SuperAdmin,mSuperAdmin")]
    public class UserRoleUpdateModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUser Person { get; set; }

        public UserRoleUpdateModel(UserManager<ApplicationUser> userManger, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManger;
            Person = new ApplicationUser();
            _roleManager = roleManager;
        }


        [BindProperty]

        public List<ApplicationRole> Roles { get; set; }

        public ApplicationUser People { get; set; }
        public string LoggedInUser { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }



        public class InputModel
        {

            public string RoleId { get; set; }
            public string UserId { get; set; }

        }
        public async Task OnGet(string id)
        {
            List<ApplicationRole> roles = new List<ApplicationRole>();

            roles = _roleManager.Roles.Where(x => x.Name != "mSuperAdmin").ToList();

            Roles = roles;
            LoggedInUser = id;
            var user = await _userManager.FindByIdAsync(id);
            People = user;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //clear roles
            var user = await _userManager.FindByIdAsync(Input.UserId);
            var role = await _userManager.GetRolesAsync(user);
            if (role != null)
            {
                foreach (var r in role.ToList())
                {
                    try
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, r);
                        if (result.Succeeded)
                        {

                        }
                    }
                    catch (Exception c) { }


                }
                user.RoleString = "";
                await _userManager.UpdateAsync(user);
            }
            try
            {

                var rolename = await _roleManager.FindByNameAsync(Input.RoleId);

                try
                {
                    var result = await _userManager.AddToRoleAsync(user, Input.RoleId);
                    if (result.Succeeded)
                    {
                        user.RoleString = rolename.Name;
                        await _userManager.UpdateAsync(user);
                        return Redirect("/UserManagement/Users/Index");
                    }
                    else
                    {

                    }
                }
                catch (Exception c)
                {

                }


            }
            catch (Exception d)
            {

            }


            // TempData["removeuser"] = "Remove user from this role ("+string.Join("; ", rolecount)+") first";

            var useri = await _userManager.FindByIdAsync(Input.UserId);
            People = useri;
            List<ApplicationRole> roles = new List<ApplicationRole>();

            roles = _roleManager.Roles.Where(x => x.Name != "SuperAdmin").ToList();

            Roles = roles;

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }

}