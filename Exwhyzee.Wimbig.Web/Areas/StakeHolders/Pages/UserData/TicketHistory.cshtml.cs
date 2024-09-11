using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.UserData
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin,Agent,DGAs,Supervisors")]

    public class TicketHistoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;

        public TicketHistoryModel(UserManager<ApplicationUser> userManager,
            IPurchaseTicketAppService purchaseTicketAppService)
        {
            _userManager = userManager;
            _purchaseTicketAppService = purchaseTicketAppService;
        }

        
       
        public PagedList<TicketDto> Tickes { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
           ViewData["UserName"] = "Username: "+user.UserName +" "+ user.FullName;

            ///check if user is in any of the role
            var Roleusers = _userManager.GetUsersInRoleAsync("User").Result;
var RoleDga = _userManager.GetUsersInRoleAsync("DGAs").Result;
            var Roleagent = _userManager.GetUsersInRoleAsync("Agent").Result;

           ///

            var roleusers = Roleusers.FirstOrDefault(x => x.Id == user.Id);
            if(roleusers != null)
            {
            Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceIdUser(searchString: user.UserName);

            }
            var roleagent = Roleagent.FirstOrDefault(x => x.Id == user.Id);
            if (roleagent != null)
            {
                Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);

            }
            var roledga = RoleDga.FirstOrDefault(x => x.Id == user.Id);
            if (roledga != null)
            {
                Tickes = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);

            }
            return Page();
        }
    }
}