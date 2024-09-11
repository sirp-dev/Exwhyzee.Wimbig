using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.PayOutReports.Dto;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.UserData
{
    public class PayoutProcessModel : PageModel
    {
        private readonly IWalletAppService _walletAppService;
        private readonly IPayOutReportsAppService _payOutReportsAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStoreRepository _messageStoreRepository;



        public PayoutProcessModel(IMessageStoreRepository messageStoreRepository, IWalletAppService walletAppService, UserManager<ApplicationUser> userManger, IPayOutReportsAppService payOutReportsAppService)
        {
            _messageStoreRepository = messageStoreRepository;
             _walletAppService = walletAppService;
            _userManager = userManger;
            _payOutReportsAppService = payOutReportsAppService;
        }



        public decimal Balance { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public long id { get; set; }
            public string UserId { get; set; }
            public string Source { get; set; }
        }


            public PayOutReportDto Payout { get; set; }
        public WalletDto wallet { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGet(long id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
            }
            try
            {
                var wallet = await _walletAppService.GetWallet(user.Id);
                Balance = wallet.Balance;

                Payout = await _payOutReportsAppService.Get(id);
                
            }
            catch (Exception e)
            {

            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Input.UserId);
                if (user == null)
                {
                    return NotFound($"Unable to load user.");
                }

                wallet = await _walletAppService.GetWallet(user.Id);
                Payout = await _payOutReportsAppService.Get(Input.id);

                wallet.Balance = wallet.Balance + Payout.PercentageAmount;
                Payout.Status = Enums.PayoutEnum.PayedToWallet;

                var transaction = new TransactionDto
                {
                    WalletId = wallet.Id,
                    TransactionType = Enums.TransactionTypeEnum.Credit,
                    Amount = Payout.PercentageAmount,
                    DateOfTransaction = DateTime.UtcNow.AddHours(1),
                    Status = Enums.EntityStatus.Success,
                    TransactionReference = "Payout Cash",
                    UserId = user.Id,
                    Username = user.UserName,
                    Description = "Payout Transaction"
                };
                await _payOutReportsAppService.UpdatePayout(Payout, wallet, transaction);



                string senderEmailMessageBody = "Payout Cash of N" + Payout.PercentageAmount + " was Credited to your Wimbig Wallet on " + DateTime.UtcNow.AddHours(1).ToLongDateString() + ". Your Wimbig Wallet Balance is " + wallet.Balance + ". Play more @ https://wimbig.com Thanks.";
                string emailMessage = string.Format("{0};??{1};??{2};??{3}", "Payout", "Transaction Notification", "Dear " + user.FullName, senderEmailMessageBody);

                string smsmessage = "Payout of Amount N" + Payout.PercentageAmount + " was Credited to your Wimbig Wallet on " + DateTime.UtcNow.AddHours(1).ToLongDateString()+". Your Wimbig Wallet Balance is " + wallet.Balance;

                await SendMessage(emailMessage, user.Email, MessageChannel.Email, MessageType.Activation);

                await SendMessage(smsmessage, user.PhoneNumber, MessageChannel.SMS, MessageType.Activation);

                TempData["msg"] = "Payout Successfull.";
                return RedirectToPage("./PayoutHistory", new { status = "success" });
            }
            catch (Exception e)
            {
                TempData["msg"] = "Error: try again";

                return RedirectToPage("./PayoutHistory", new { status = "success" });
            }

            
        }


        private async Task SendMessage(string message, string address,
          MessageChannel messageChannel, MessageType messageType)
        {
            try
            {


                var messageStore = new MessageStore()
                {
                    MessageChannel = messageChannel,
                    MessageType = messageType,
                    Message = message,
                    AddressType = AddressType.Single
                };

                if (messageChannel == MessageChannel.Email)
                {
                    messageStore.EmailAddress = address;
                }
                else if (messageChannel == MessageChannel.SMS)
                {
                    messageStore.PhoneNumber = address;
                }
                else
                {

                }

                await _messageStoreRepository.Add(messageStore);
            }
            catch (Exception e)
            {

            }
        }
    }
}