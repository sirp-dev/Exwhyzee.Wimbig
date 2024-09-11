using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.Count.Dtos;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Data.Repository.Count;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Sms
{
    [Authorize]
    public class PassiveUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly ICountAppService _countAppService;

        public IList<ApplicationUser> Users { get; private set; }
        public IList<UserVM> userVM { get; private set; }
        public class UserVM
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string city { get; set; }
            public decimal Bal { get; set; }
            public double LastPlayed { get; set; }
            public DateTime Date { get; set; }
        }
        public PassiveUsersModel(
            UserManager<ApplicationUser> userManger, IPurchaseTicketAppService purchaseTicketAppService,
             ICountAppService countAppService
            )
        {
            _userManager = userManger;
            _countAppService = countAppService;
            _purchaseTicketAppService = purchaseTicketAppService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var allusers = await _countAppService.UsersInRole();


            var list = (from user in allusers.Source
                        select new UserVM
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Name = user.FullName,
                            PhoneNumber = user.PhoneNumber,
                            Bal = user.Balance,
                            city = user.CurrentCity
                        }).ToList();


            list.Select(async user =>
            {
                var tickets = await _purchaseTicketAppService.GetAllTickets(searchString: user.UserName);
                var last = tickets.Source.OrderByDescending(x => x.Date).FirstOrDefault();
                DateTime now = DateTime.UtcNow;
                DateTime lasttime =last.Date;


                user.LastPlayed = Math.Round((now - lasttime).TotalDays);
                user.Date = last.Date.Date;
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