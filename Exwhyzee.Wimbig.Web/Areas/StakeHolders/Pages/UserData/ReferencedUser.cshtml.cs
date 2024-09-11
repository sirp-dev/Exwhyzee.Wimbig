using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.UserData
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin,Agent,DGAs,Supervisors")]
    public class ReferencedUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;

        public IList<ApplicationUser> Users { get; private set; }
        public IList<UserVM> userVM { get; private set; }
        public class UserVM
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Roles { get; set; }
            public int Tickets { get; set; }
        }
        public ReferencedUserModel(
            UserManager<ApplicationUser> userManger, IPurchaseTicketAppService purchaseTicketAppService
            )
        {
            _userManager = userManger;
            _purchaseTicketAppService = purchaseTicketAppService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string id = _userManager.GetUserId(HttpContext.User);

            var users = _userManager.GetUsersInRoleAsync("User").Result;

            var myuser = users.Where(x => x.ReferenceId == id);

            var list = (from user in myuser
                        select new UserVM
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Name = user.FullName,
                            Email = user.Email
                        }).ToList();

           
            list.Select(async user =>
            {
                var tickets = await _purchaseTicketAppService.GetAllTicketsByReferenceIdUser(searchString: user.UserName);
                user.Tickets = tickets.Source.Count();
            }
            ).ToList();

            userVM = list;
            
            if (list == null)
            { 
                return NotFound($"Unable to load users.");
            }
           
            return Page();
        }

    }
}