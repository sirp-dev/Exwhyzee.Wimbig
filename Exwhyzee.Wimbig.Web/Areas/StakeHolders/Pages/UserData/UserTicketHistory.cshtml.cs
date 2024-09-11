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

    public class UserTicketHistoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;

        public UserTicketHistoryModel(UserManager<ApplicationUser> userManager,
            IPurchaseTicketAppService purchaseTicketAppService)
        {
            _userManager = userManager;
            _purchaseTicketAppService = purchaseTicketAppService;
        }

        
       
        public List<TicketDto> Tickes { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, DateTime startdate, DateTime enddate)
        {
            
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            TempData["date"] = "From " + startdate.ToString("dddd dd MMM, yyyy") + " to " + enddate.ToString("dddd dd MMM, yyyy");
            TempData["user"] = user.FirstName + " " + user.LastName + " " + user.OtherNames;
                var tickets = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: user.UserName);
            Tickes = tickets.Source.Where(a => a.Date.Date >= startdate && a.Date.Date <= enddate).ToList();


            return Page();
        }
    }
}