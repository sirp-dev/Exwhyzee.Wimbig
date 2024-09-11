using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Tickets
{
    public class IndexModel : PageModel
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletAppService _walletAppService;


        public IndexModel(IPurchaseTicketAppService purchaseTicketAppService, IWalletAppService walletAppService, UserManager<ApplicationUser> userManager)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
            _userManager = userManager;
            _walletAppService = walletAppService;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public PagedList<TicketDto> Tickets { get; set; }
        public List<TicketDto> Listing { get; set; }

        DateTime? dateStart = null;
        DateTime? dateEnd = null;
        int startIndex = 0;
        int count = int.MaxValue;

        
        
        public async Task<IActionResult> OnGetAsync(int day = 0, DateTime? date = null)
        {

            var query = await _purchaseTicketAppService.GetAllTickets();
            List<TicketDto> tickets = new List<TicketDto>();
            List<TicketDto> items = new List<TicketDto>();
            if (day == 0 && date != null)
            {
                DateTime oDate = Convert.ToDateTime(date);
                if (date != null)
                {
                    //string iDate = "05/05/2005";
                   
                    TempData["range"] = "(Date)" + date.Value.Date.ToString("ddd dd MMM, yyyy");

                }
                if (date != null)
                {
                    items = query.Source.Where(x => x.Date.Date == date.Value.Date).ToList();
                }
            }
            else if (day > 0 && date == null)
            {


                if (day > 1)
                {
                    TempData["range"] = "From Today " + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy") + " to " + DateTime.UtcNow.AddDays(-day).Date.ToString("ddd dd MMM, yyyy") + "( " + day + "days Ago)";

                }
                else
                {
                    TempData["range"] = "(Today)" + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy");

                }
                if (day > 1)
                {
                    items = query.Source.Where(x => x.Date.Date >= DateTime.UtcNow.AddDays(-day).Date && x.Date.Date <= DateTime.UtcNow.Date).ToList();
                }
                else
                {
                    items = query.Source.Where(x => x.Date.Date == DateTime.UtcNow.Date).ToList();
                }
            }
            else if (day == 0 && date == null)
            {
                
                TempData["range"] = "(Today)" + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy");

                items = query.Source.Where(x => x.Date.Date == DateTime.UtcNow.Date).ToList();

            }
            else
            {
                TempData["error"] = "Choose either Date or Days. Thank you";

                
                    TempData["range"] = "(Today)" + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy");
                
                    items = query.Source.Where(x => x.Date.Date == DateTime.UtcNow.Date).ToList();
                
            }
            tickets.AddRange(items.Select(x => new TicketDto()
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
                PaidOut = x.PaidOut,
                CurrentLocation = x.CurrentLocation
            }));

            Tickets = new PagedList<TicketDto>(source: tickets.ToList(), pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);

            return Page();
    }
       
    }
}