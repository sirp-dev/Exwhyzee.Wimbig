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
    [Authorize(Roles = "Admin,SuperAdmin,mSuperAdmin")]
    public class AddUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUser NewPerson { get; set; }

        public AddUserModel(UserManager<ApplicationUser> userManger)
        {
            _userManager = userManger;
            NewPerson = new ApplicationUser();

        }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}