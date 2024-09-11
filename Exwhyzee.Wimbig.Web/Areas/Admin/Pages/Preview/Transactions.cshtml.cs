using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Preview
{

    [Authorize]

    public class TransactionsModel : PageModel
    { 

        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITransactionAppService _transactionAppService;


        public TransactionsModel(IWalletAppService walletAppService, UserManager<ApplicationUser> userManager,
            ITransactionAppService transactionAppService
            )
        {
            _walletAppService = walletAppService;
            _transactionAppService = transactionAppService;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public PagedList<TransactionDto> Transactions { get; set; }

        public WalletDto Wallet { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            TempData["username"] = user.UserName + " (" + user.CurrentCity + ")";

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if(user.CurrentCity == null)
            {
                TempData["update"] = "Update your current city";
                return RedirectToPage("./UpdateProfile");
            }

            Wallet = await _walletAppService.GetWallet(user.Id);
            Transactions = await _transactionAppService.GetAllTransactions(user.Id);
            return Page();
        }
    }
}