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

    public class RolesModel : PageModel
    {

        private readonly RoleManager<ApplicationRole> _roleManager;
        public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();


        public RolesModel(
            RoleManager<ApplicationRole> roleManager

           )
        {
            _roleManager = roleManager;
        }

      
        public async Task<IActionResult> OnGetAsync()
        {
            Roles =  _roleManager.Roles.Where(x=>x.Name != "mSuperAdmin").ToList();
           
            return Page();
        }
    }
}