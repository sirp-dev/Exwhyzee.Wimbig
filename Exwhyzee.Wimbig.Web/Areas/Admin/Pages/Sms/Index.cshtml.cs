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

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Sms
{
    [Authorize(Roles = "Admin,SuperAdmin,mSuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICountAppService _countAppService;
        
        public IndexModel(
            UserManager<ApplicationUser> userManger, ICountAppService countAppService
            )
        {
            _countAppService = countAppService;
            _userManager = userManger;
        }
       
        public int Users { get; set; }
        public int DGAs { get; set; }
        public int Agents { get; set; }
        public int Supervisors { get; set; }
        public int Admin { get; set; }
        public int Stakeholders { get; set; }

        public PagedList<UsersInRoleDto> UsersInRole { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //user list
           var Userlist = await _countAppService.UsersInRole();
            Users = Userlist.Source.Count();

            //dga list
            var usersD = _userManager.GetUsersInRoleAsync("DGAs").Result;
            DGAs = usersD.Count();

            //agents
            var usersA = _userManager.GetUsersInRoleAsync("Agent").Result;
            Agents = usersA.Count();

            //supervisor
            var usersS = _userManager.GetUsersInRoleAsync("Supervisors").Result;
            Supervisors = usersS.Count();

            //admin
            var usersAdmin = _userManager.GetUsersInRoleAsync("Admin").Result;
            Admin = usersAdmin.Count();
            //admin
            var stake = _userManager.GetUsersInRoleAsync("SuperAdmin").Result;
            Stakeholders = stake.Count();


            //fetch users
            UsersInRole = await _countAppService.UsersInRole();

            return Page();
        }
    }
}