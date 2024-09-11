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
    public class SearchResultModel : PageModel
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletAppService _walletAppService;


        public SearchResultModel(IPurchaseTicketAppService purchaseTicketAppService, IWalletAppService walletAppService, UserManager<ApplicationUser> userManager)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
            _userManager = userManager;
            _walletAppService = walletAppService;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public DateTime? startdateData { get; set; }
        [TempData]
        public DateTime? enddateData { get; set; }
        [TempData]
        public string usernameData { get; set; }

        [TempData]
        public string Sumdata { get; set; }


        public PagedList<TicketDto> Tickets { get; set; }
        public List<TicketDto> Listing { get; set; }


        public async Task<IActionResult> OnGetAsync(DateTime? startdate, DateTime? enddate, string username)
        {
            startdateData = startdate;
            enddateData = enddate;
            usernameData = username;
            Tickets = await _purchaseTicketAppService.GetAllTickets(dateStart: startdate, dateEnd: enddate, searchString: username);

            Sumdata = Tickets.Source.Sum(x => x.Price).ToString();
            return Page();
        }

    }
}