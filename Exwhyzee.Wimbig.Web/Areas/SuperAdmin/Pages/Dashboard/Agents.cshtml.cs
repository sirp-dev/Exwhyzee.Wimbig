using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Exwhyzee.Wimbig.Web.Areas.SuperAdmin.Pages.Dashboard
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class AgentsModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();


        private readonly UserManager<ApplicationUser> _userManager;
        public IList<ApplicationUser> Users { get; private set; }

        public AgentsModel(
            UserManager<ApplicationUser> userManger, RoleManager<ApplicationRole> roleManager
            )
        {
            _userManager = userManger;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _userManager.GetUsersInRoleAsync("Agent");

            if (Users == null)
            {
                return NotFound($"Unable to load users.");
            }

            return Page();
        }
    }
}