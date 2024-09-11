using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Paystack;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]

    public class PaymentModel : PageModel
    {
        private readonly IWalletAppService _walletAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IPaystackTransactionAppService _paystackTransactionAppService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentModel(ITransactionAppService transactionAppService, 
            IPaystackTransactionAppService paystackTransactionAppService,
            UserManager<ApplicationUser> userManager, IWalletAppService walletAppService)
        {
            _transactionAppService = transactionAppService;
            _paystackTransactionAppService = paystackTransactionAppService;
            _userManager = userManager;
            _walletAppService = walletAppService;
        }

        [BindProperty]
        [Required, Range(100,100000)]
        public decimal Amount { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {


                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var wallet = await _walletAppService.GetWallet(user.Id);
                var transaction = await _transactionAppService.CreateTransaction(new InsertTransactionDto
                {
                    Amount = Amount,
                    DateOfTransaction = DateTime.UtcNow.AddHours(1),
                    Status = EntityStatus.Pending,
                    TransactionType = TransactionTypeEnum.Credit,
                    UserId = user.Id,
                    WalletId = wallet.Id,
                    Description = "Online Transaction"
                });

                var secretKey = "sk_live_ce8df52dee2bab80417d8b5ac4528a8628cabb9e";

                int amountInKobo = (int)Amount * 100;

                var response = await _paystackTransactionAppService.InitializeTransaction(secretKey, user.Email, amountInKobo, transaction.Id, user.FirstName,
                    user.LastName);

                if (response.status == true)
                {
                    return Redirect(response.data.authorization_url);
                }

                return Page();
            }catch(Exception c)
            {
                StatusMessage = $"Erro: "+ c;
                return Page();
            }

        }

    }
}