using Exwhyzee.Wimbig.Application.Paystack;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Areas.Callback.Pages.Transactioncallback
{
    public class CallbackModel : PageModel
    {
        private readonly ITransactionAppService _transactionAppService;
        private readonly IPaystackTransactionAppService _paystackTransactionAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;




        public CallbackModel(ITransactionAppService transactionAppService,
            IPaystackTransactionAppService paystackTransactionAppService,
            UserManager<ApplicationUser> userManager, IWalletAppService walletAppService,
            IMessageStoreRepository messageStoreRepository, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            _transactionAppService = transactionAppService;
            _paystackTransactionAppService = paystackTransactionAppService;
            _userManager = userManager;
            _config = configuration;
            _walletAppService = walletAppService;
            Check++;
            _messageStoreRepository = messageStoreRepository;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public decimal Amount { get; set; }

        public int Check { get; set; }

        public decimal WalletTotal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            //var secretKey = _config["SecretKey"];
            var secretKey = "sk_live_ce8df52dee2bab80417d8b5ac4528a8628cabb9e";
            var tranxRef = HttpContext.Request.Query["reference"].ToString();
            if (tranxRef != null)
            {
                var response = await _paystackTransactionAppService.VerifyTransaction(tranxRef, secretKey);

                var id = long.Parse(response.data.metadata.CustomFields.FirstOrDefault(x => x.DisplayName == "Transaction Id").Value);
                var transaction = await _transactionAppService.GetTransaction(id);

               // var user = await _userManager.GetUserAsync(User);

                var wallet = await _walletAppService.GetWallet(transaction.UserId);
               
                if (response.status)
                {


                    Amount = transaction.Amount;

                    if (transaction == null)
                    {
                        StatusMessage = $"Transaction with Reference {tranxRef} was successful. But Wallet was not updated. Please contact Help Desk.";
                        return Page();
                    }
                    else if (!string.IsNullOrEmpty(transaction.TransactionReference))
                    {
                        StatusMessage = $"Transaction with Reference {tranxRef} was successful.";
                        return Page();
                    }
                    else
                    {

                        WalletTotal = wallet.Balance;
                        transaction.WalletId = wallet.Id;
                        transaction.Status = Enums.EntityStatus.Success;
                        transaction.TransactionReference = tranxRef;
                        var update = await _transactionAppService.UpdateTransaction(transaction);

                        wallet.Balance += transaction.Amount;
                        await _walletAppService.UpdateWallet(wallet);
                        var walletcurrent = await _walletAppService.GetWallet(transaction.UserId);
                        var user = await _userManager.FindByIdAsync(transaction.UserId);
                       
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        StatusMessage = $"Transaction with Reference {tranxRef} was successful.";

                        string emailMessageBody = StatusMessage + " Your Wimbig Wallet Balance is N" + walletcurrent.Balance + ". Play more @ https://wimbig.com Thanks.";
                        var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Transaction Notification", "Transaction Notification", "Thanks " + user.FullName, emailMessageBody);

                        string smsMessage = StatusMessage + " Your Wimbig Wallet Balance is N" + walletcurrent.Balance;

                        await SendMessage(emailMessage, user.Email, MessageChannel.Email, MessageType.Activation);

                        await SendMessage(smsMessage, user.PhoneNumber, MessageChannel.SMS, MessageType.Activation);



                        return Page();
                    }


                }
                else
                {
                    transaction.WalletId = wallet.Id;
                    transaction.Status = Enums.EntityStatus.Failed;
                    transaction.TransactionReference = tranxRef;
                    var update = await _transactionAppService.UpdateTransaction(transaction);
                    StatusMessage = $"Transaction with Reference {tranxRef} failed.";
                    return Page();

                }

            }

            return Page();
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