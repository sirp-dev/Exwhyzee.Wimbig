using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Wimbank;
using Exwhyzee.Wimbig.Application.Wimbank.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]

    public class WimPayModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWimbankAppService _wimbankAppService;
        private readonly IWalletAppService _walletAppService;
       

        private readonly IHostingEnvironment _hostingEnv;

        public WimPayModel(IHostingEnvironment env, IWimbankAppService wimbankAppService, 
            UserManager<ApplicationUser> userManger, IWalletAppService walletAppService)
        {
            _hostingEnv = env;
            _wimbankAppService = wimbankAppService;
            _userManager = userManger;
            _walletAppService = walletAppService;
           
        }



        public List<ApplicationUser> Users { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public TransferMoney transferMoney { get; set; }

        public class TransferMoney
        {
            public string SenderId { get; set; }
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
LoggedInUser = _userManager.GetUserId(HttpContext.User);
            var userbalance = await _walletAppService.GetWallet(LoggedInUser);
            Balance = userbalance.Balance;
            
            var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
            Users = users;


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var senderwallet = await _walletAppService.GetWallet(transferMoney.SenderId);
                    if (senderwallet == null)
                    {
                        var userbalance = await _walletAppService.GetWallet(transferMoney.SenderId);
                        Balance = userbalance.Balance;
                        LoggedInUser = _userManager.GetUserId(HttpContext.User);
                        var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
                        Users = users;
                        StatusMessage = "Error! Your Wallet not found.";
                        return Page();
                    }
                    else
                    {
                        if (senderwallet.Balance < transferMoney.Amount)
                        {
                            var userbalance = await _walletAppService.GetWallet(transferMoney.SenderId);
                            Balance = userbalance.Balance;
                            LoggedInUser = _userManager.GetUserId(HttpContext.User);
                            var users = _userManager.Users.ToList();
                            Users = users;
                            StatusMessage = "Error! Insufficient balance. Credit your account or send amount below your balance.";
                            return Page();
                        }
                        if (transferMoney.Amount > 5000)
                        {
                            var userbalance = await _walletAppService.GetWallet(transferMoney.SenderId);
                            Balance = userbalance.Balance;
                            LoggedInUser = _userManager.GetUserId(HttpContext.User);
                            var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
                            Users = users;
                            StatusMessage = "Error! cannot wimpay above #5000.";
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
                    var senderwalletid = await _walletAppService.GetWallet(transferMoney.SenderId);
                    var receiverwalletid = await _walletAppService.GetWallet(transferMoney.ReceiverId);

                    var wimpay = new WimpayDto
                    {
                        Sender = transferMoney.SenderId,
                        ReceiverId = transferMoney.ReceiverId,
                        Amount = transferMoney.Amount,
                        Note = transferMoney.Note,
                        ReceiverPhone = transferMoney.PhoneNumber,
                        Senderwalletid = senderwalletid.Id,
                        Receiverwalletid = receiverwalletid.Id
                    };


                   var result = await _wimbankAppService.CreateWimPay(wimpay);


                   

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