using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.UserData
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin,Agent,DGAs,Supervisors")]
    public class TicketAgentDgaSummaryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;

        public PagedList<TicketDto> Tickesl { get; set; }
        public PagedList<TicketDto> newticket { get; set; }
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
       public List<TicketDto> Tickes = new List<TicketDto>();
        public TicketAgentDgaSummaryModel(
            UserManager<ApplicationUser> userManger, IPurchaseTicketAppService purchaseTicketAppService
            )
        {
            _userManager = userManger;
            _purchaseTicketAppService = purchaseTicketAppService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string id = _userManager.GetUserId(HttpContext.User);

            var usersA = _userManager.GetUsersInRoleAsync("Agent").Result;

            var myuserA = usersA.Where(x => x.ReferenceId == id);

            //

            var usersD = _userManager.GetUsersInRoleAsync("DGAs").Result;

            var myuserD = usersD.Where(x => x.ReferenceId == id);

            var userlist = myuserA.Concat(myuserD);
           // var users = _userManager.GetUsersInRoleAsync("User").Result;

           // var myuser = users.Where(x => x.ReferenceId == id);

            var list = (from user in userlist
                        select new UserVM
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Name = user.FullName,
                            Email = user.Email
                        }).ToList();
            
            foreach (var i in list)
            {

                newticket = await _purchaseTicketAppService.GetAllTicketsByReferenceId(searchString: i.UserName);
                //Tickes.ToList().ad(newticket.Source.ToList());
                Tickes.AddRange(newticket.Source.Select(x => new TicketDto()
                {
                    Email = x.Email,
                    FullName = x.FullName,
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    Price = x.Price,
                    PurchaseDate = x.PurchaseDate,
                    RaffleId = x.RaffleId,
                    RaffleName = x.RaffleName,
                    TicketNumber = x.TicketNumber,
                    TransactionId = x.TransactionId,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    IsWinner = x.IsWinner,
                    IsSentToStat = x.IsSentToStat,
                    Date = x.Date,
                    Status = x.Status,
                    PlayerName = x.PlayerName,
                    PaidOut = x.PaidOut
                }));

            };

         //   Tickesj = Tickes.topIPagedList
            // 
            //

            if (list == null)
            {
                return NotFound($"Unable to load users.");
            }

            return Page();
        }


    }
}