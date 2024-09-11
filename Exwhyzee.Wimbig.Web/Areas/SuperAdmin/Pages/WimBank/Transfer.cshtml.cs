using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Wimbank;
using Exwhyzee.Wimbig.Application.Wimbank.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Areas.SuperAdmin.Pages.WimBank
{
    [Authorize(Roles = "mSuperAdmin")]
    public class TransferModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWimbankAppService _wimbankAppService;
        private readonly IWalletAppService _walletAppService;

        private readonly IHostingEnvironment _hostingEnv;

        public TransferModel(IHostingEnvironment env, IWimbankAppService wimbankAppService, UserManager<ApplicationUser> userManger, IWalletAppService walletAppService)
        {
            _hostingEnv = env;
            _wimbankAppService = wimbankAppService;
            _userManager = userManger;
            _walletAppService = walletAppService;

        }


        public WimbankDto wimbank { get; set; }

        public WimTransferDto wimTransferDto { get; set; }

        public List<ApplicationUser> Users { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public TransferMoney transferMoney { get; set; }

        public class TransferMoney
        {
            public string UserId { get; set; }
            public string ReceiverId { get; set; }

            public decimal Amount { get; set; }

            public DateTime DateOfTransaction { get; set; }

            public TransactionTypeEnum Status { get; set; }

            public string Note { get; set; }
            public string PhoneNumber { get; set; }
        }

        public string LoggedInUser { get; set; }

        public string ReturnUrl { get; set; }
        public decimal Balance { get; set; }


        public async Task OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            var lastBank = await _wimbankAppService.WimbankLastRecord();
            Balance = lastBank.Balance;
            LoggedInUser = _userManager.GetUserId(HttpContext.User);
            var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").OrderByDescending(x=>x.UserName).ToList();
            Users = users;


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var wimbank = await _wimbankAppService.WimbankLastRecord();
                    if (wimbank == null)
                    {
                        var lastBank = await _wimbankAppService.WimbankLastRecord();
                        Balance = lastBank.Balance;
                        LoggedInUser = _userManager.GetUserId(HttpContext.User);
                        var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
                        Users = users;
                        StatusMessage = "Error! Wallet not found.";
                        return Page();
                    }
                    else
                    {
                        if (wimbank.Balance < transferMoney.Amount)
                        {
                            var lastBank = await _wimbankAppService.WimbankLastRecord();
                            Balance = lastBank.Balance;
                            LoggedInUser = _userManager.GetUserId(HttpContext.User);
                            var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
                            Users = users;
                            StatusMessage = "Error! Insufficient balance. Credit your account or send amount below your balance.";
                            return Page();
                        }
                    }

                    var receiverVerify = await _userManager.FindByIdAsync(transferMoney.ReceiverId);
                    if (receiverVerify.PhoneNumber != transferMoney.PhoneNumber)
                    {
                        var lastBank = await _wimbankAppService.WimbankLastRecord();
                        Balance = lastBank.Balance;
                        LoggedInUser = _userManager.GetUserId(HttpContext.User);
                        var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
                        Users = users;
                        StatusMessage = "Error! Unable to verify User. Confirm phone number";
                        return Page();
                    }

                    var wimtransfer = new WimTransferDto
                    {
                        UserId = transferMoney.UserId,
                        ReceiverId = transferMoney.ReceiverId,
                        Amount = transferMoney.Amount,
                        Note = transferMoney.Note,
                        ReceiverPhone = transferMoney.PhoneNumber
                    };


                    await _wimbankAppService.CreateWimbankTransfer(wimtransfer);


                    return RedirectToPage("Index");
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}