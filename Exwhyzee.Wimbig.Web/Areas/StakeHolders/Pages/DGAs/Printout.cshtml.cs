using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Print;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.DGAs
{
    [Authorize]
    public class PrintoutModel : PageModel
    {

        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PrintoutModel(IPurchaseTicketAppService purchaseTicketAppService,
            ITransactionAppService transactionAppService, UserManager<ApplicationUser> userManager
           )
        {
            _purchaseTicketAppService = purchaseTicketAppService;
            _transactionAppService = transactionAppService;
            _userManager = userManager;

        }
        [BindProperty]
        public PrintDto Print { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id < 0)
                throw new ArgumentNullException(nameof(id));


            //check raffle
            var transaction = await _transactionAppService.GetTransaction(id);
            var agentName = await _userManager.FindByIdAsync(transaction.UserId);
            var tickets = await _purchaseTicketAppService.GetAllTicketsByTransactionId(transactionId: transaction.Id);
            string ticketlist = "No. "+ string.Join(", No. ", tickets.Source.Select(x => x.TicketNumber));
            var phoneinfo = tickets.Source.Select(x => x.PhoneNumber).Distinct().FirstOrDefault();
            var raffleNameinfo = tickets.Source.Select(x => x.RaffleName).Distinct().FirstOrDefault();
            var raffleIdinfo = tickets.Source.Select(x => x.RaffleId).Distinct().FirstOrDefault();

            Print = new PrintDto
            {
                TransactionDate = transaction.DateOfTransaction,
                Tickets = ticketlist,
                RaffleName = raffleNameinfo,
                RaffleNumber = raffleIdinfo,
                Phone = phoneinfo,
                TransactionId = transaction.Id,
                Amount = transaction.Amount,
                AgentName = agentName.UserName
            };
            return Page();


        }
    }
}