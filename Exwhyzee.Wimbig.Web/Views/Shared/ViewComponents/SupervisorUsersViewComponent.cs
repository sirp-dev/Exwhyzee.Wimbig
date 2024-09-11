using Exwhyzee.Wimbig.Application.Tickets;
using System.Collections.Generic;
using System.Linq;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class SupervisorUsersViewComponent : ViewComponent
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupervisorUsersViewComponent(
            UserManager<ApplicationUser> userManager,
            IPurchaseTicketAppService purchaseTicketAppService)
        {
           
            _userManager = userManager;
            _purchaseTicketAppService = purchaseTicketAppService;

        }

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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetTicketAsync();
            ViewData["totalticket"] = item;
            return View();
        }

        
        private async Task<string> GetTicketAsync()
        {

            var useri = await _userManager.GetUserAsync(HttpContext.User);

            var users =  _userManager.GetUsersInRoleAsync("User").Result;

            var myuser = users.Where(x => x.ReferenceId == useri.Id);

            string ticketcount = myuser.Count().ToString();


            return ticketcount;
        }

    }
}
