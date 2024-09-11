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

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Transactions
{
    [Authorize(Roles = "mSuperAdmin")]

    public class EditTransactionModel : PageModel
    {
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITransactionAppService _transactionAppService;


        public EditTransactionModel(IWalletAppService walletAppService, UserManager<ApplicationUser> userManager,
            ITransactionAppService transactionAppService
            )
        {
            _walletAppService = walletAppService;
            _transactionAppService = transactionAppService;
            _userManager = userManager;
        }

        [BindProperty]
        public TransactionDto transactions { get; set; }

        [BindProperty]
        public WalletDto wallet { get; set; }

        [TempData]
        public string Userinfo { get; set; }

        [TempData]
        public string Username { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var trans = await _transactionAppService.GetTransaction(id);
            var wall = await _walletAppService.GetWallet(trans.UserId);

            var user = await _userManager.FindByIdAsync(trans.UserId);
            Userinfo = user.FirstName + " " + user.LastName + " " + user.OtherNames;
            Username = user.UserName;
            if (trans == null)
            {
                return NotFound($"Unable to load raffle with the ID '{id}'.");
            }

            wallet = new WalletDto
            {
                Balance = wall.Balance
            };
            transactions = new TransactionDto
            {
                Amount = trans.Amount,
                DateOfTransaction = trans.DateOfTransaction,
                Status = trans.Status,
                TransactionType = trans.TransactionType,
                UserId = trans.UserId,
                WalletId = trans.WalletId,
                TransactionReference = trans.TransactionReference
            };



            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var Transaction = await _transactionAppService.GetTransaction(id);
            if (Transaction == null)
            {
                return NotFound($"Unable to load Raffle with the ID '{id}'.");
            }

            Transaction.Status = transactions.Status;

            Transaction.WalletId = wallet.Id;
            Transaction.TransactionReference = transactions.TransactionReference;
await _transactionAppService.UpdateTransaction(Transaction);

            ////
            ///
            var wall = await _walletAppService.GetWallet(Transaction.UserId);
            wall.Balance = wall.Balance + Transaction.Amount;

            await _walletAppService.UpdateWallet(wall);

            //StatusMessage = "The Selected Raffle has been updated";
            return RedirectToPage("./Index");
        }
    }
}