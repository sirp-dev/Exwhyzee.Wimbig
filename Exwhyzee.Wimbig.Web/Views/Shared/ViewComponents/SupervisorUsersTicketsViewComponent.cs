using Exwhyzee.Wimbig.Application.Tickets;
using System.Collections.Generic;
using System.Linq;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class SupervisorUsersTicketsViewComponent : ViewComponent
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupervisorUsersTicketsViewComponent(
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
        public PagedList<TicketDto> newticket { get; set; }
        public List<TicketDto> Tickes = new List<TicketDto>();


        private async Task<string> GetTicketAsync()
        {

            var useri = await _userManager.GetUserAsync(HttpContext.User);

            var users = _userManager.GetUsersInRoleAsync("User").Result;

            var myuser = users.Where(x => x.ReferenceId == useri.Id);

            var list = (from user in myuser
                        select new UserVM
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Name = user.FullName,
                            Email = user.Email
                        }).ToList();

            foreach (var i in list)
            {

                newticket = await _purchaseTicketAppService.GetAllTicketsByReferenceIdUser(searchString: i.UserName);
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

            string ticketcount = Tickes.Count().ToString();


            return ticketcount;
        }

    }
}
