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
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICountAppService _countAppService;
        
        public UsersModel(
            UserManager<ApplicationUser> userManger, ICountAppService countAppService
            )
        {
            _countAppService = countAppService;
            _userManager = userManger;
        }
       
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
    }
}