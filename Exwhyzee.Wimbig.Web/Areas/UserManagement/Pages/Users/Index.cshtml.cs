using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.Count.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Data.Repository.Count;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exwhyzee.Wimbig.Web.Areas.UserManagement.Pages.Users
{
    [Authorize(Roles = "Admin,SuperAdmin,mSuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICountAppService _countAppService;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public IndexModel(
            UserManager<ApplicationUser> userManger, ICountAppService countAppService, RoleManager<ApplicationRole> roleManager

            )
        {
            _countAppService = countAppService;
            _userManager = userManger;
            _roleManager = roleManager;

        }
        public string Message { get; set; } = "Initial Request";

        public PagedList<UsersInRoleDto> UsersInRole { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

         
            UsersInRole = await _countAppService.UsersInRole();
            if (UsersInRole == null)
            { 
                return NotFound($"Unable to load users.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateRoles()
        {
            UsersInRole = await _countAppService.UsersInRole();
            if (UsersInRole == null)
            {
                 Message = "Unable to load users.";
            }
            var userstoupdate = UsersInRole.Source.ToList();
            foreach (var user in userstoupdate) {
                //update rolestring
                string rolename = "";
                var userRole = await _userManager.FindByIdAsync(user.Id);
                var roleRole = await _userManager.GetRolesAsync(userRole);
                if (roleRole == null)
                {
                    var Role = await _roleManager.FindByNameAsync("User");
                    if (Role != null)
                    {
                        await _userManager.AddToRoleAsync(userRole, Role.Name);

                    }
                }
                else if(roleRole.Count() < 1)
                {
                    var Role = await _roleManager.FindByNameAsync("User");
                    if (Role != null)
                    {
                        await _userManager.AddToRoleAsync(userRole, Role.Name);

                    }
                }
               
                    rolename = string.Join("; ", roleRole);
              
                userRole.RoleString = rolename;
                try
                {
                await _userManager.UpdateAsync(userRole);

                }catch(Exception c)
                {

                }
               
            } return Page();
        }
    }
}