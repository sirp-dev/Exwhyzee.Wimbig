using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Exwhyzee.Wimbig.Web.Areas.Raffles.Pages.Raffles
{
    public class ValidateModel : PageModel
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly IRaffleAppService _raffleAppService;
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IMessageStoreRepository _messageStoreRepository;


        private readonly UserManager<ApplicationUser> _userManager;

        public ValidateModel(IPurchaseTicketAppService purchaseTicketAppService,
            ITransactionAppService transactionAppService,
            IMapImageToRaffleAppService mapImageToRaffleAppService,
            IRaffleAppService raffleAppService, IMessageStoreRepository messageStoreRepository,
            UserManager<ApplicationUser> userManager,
            IWalletAppService walletAppService)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
            _transactionAppService = transactionAppService;
            _raffleAppService = raffleAppService;
            this.mapImageToRaffleAppService = mapImageToRaffleAppService;
            _userManager = userManager;
            _walletAppService = walletAppService;
            _messageStoreRepository = messageStoreRepository;

        }

        [BindProperty]
        public List<RaffleListDto> RaffleListDto { get; set; }

        [BindProperty]
        public RaffleDto Raffle { get; set; }

        [BindProperty]
        public string NumberOfTickets { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public decimal TotalAmount { get; set; }

        [BindProperty]
        public bool HidePhoneNumber { get; set; }

        [BindProperty]
        public List<ImageOfARaffle> Images { get; set; }
        public WalletDto Wallet { get; set; }


        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync(long id = 0)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            //check raffle
            var raffle = await _raffleAppService.GetById(id);
            Raffle = raffle ?? throw new ArgumentNullException(nameof(id));
            var images = await mapImageToRaffleAppService.GetAllImagesOfARaffle(id);

            Images = images.ToList();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // return Redirect("/Identity/Account/Login");
                //return RedirectToPage("Login", "Raffles", new { area = "Identity" });
                //return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            try
            {
                Wallet = await _walletAppService.GetWallet(user.Id);
                RaffleListDto = await _raffleAppService.GetRaffleList(id);
                HidePhoneNumber = await _userManager.IsInRoleAsync(user, "User");
            }
            catch (Exception e)
            {

            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user.");
                }

                var wallet = await _walletAppService.GetWallet(user.Id);

                //check balance
                if (wallet.Balance < TotalAmount)
                {
                    StatusMessage = "Error! You have insufficient Balance to carry out the transaction.";

                    return Redirect("/Identity/Account/Manage/");
                }



                //Create Transaction

                var transaction = new TransactionDto
                {
                    WalletId = wallet.Id,
                    TransactionType = Enums.TransactionTypeEnum.Debit,
                    Amount = -(TotalAmount),
                    DateOfTransaction = DateTime.UtcNow.AddHours(1),
                    Status = Enums.EntityStatus.Success,
                    TransactionReference = $"{NumberOfTickets} TICKET(S)",
                    UserId = user.Id,
                    Username = user.UserName
                };

                //Update Wallet
                wallet.Balance = wallet.Balance + transaction.Amount;

                List<InsertTicketDto> insertTicketDtos = new List<InsertTicketDto>();
                var tickets = NumberOfTickets.Split(',').ToList();

                foreach (var ticket in tickets)
                {
                    insertTicketDtos.Add(new InsertTicketDto
                    {
                        Price = TotalAmount / tickets.Count(),
                        PurchaseDate = DateTime.UtcNow.AddHours(1),
                        RaffleId = Raffle.Id,
                        TicketNumber = ticket,
                        TransactionId = 0,
                        YourPhoneNumber = PhoneNumber ?? user.PhoneNumber,
                        UserId = user.Id,
                        TicketStatus = Enums.TicketStatusEnum.Active,
                        Email = user.Email,
                        PlayerName = user.UserName
                        
                    });
                }
                StatusMessage = $"Your Ticket was successful.";
                var result = await _purchaseTicketAppService.ProcessTicket(transaction, wallet, insertTicketDtos);
                var raffleinfo = await _raffleAppService.GetById(Raffle.Id);
                string ticketlist = "No. " + string.Join(", No. ", tickets);
                string emailMessageBody = "Your Ref ID " + result + " played ticket" + ticketlist + " with N" + TotalAmount + " for Raffle: " + raffleinfo.Name + "  with ID " + raffleinfo.Id + " on " + transaction.DateOfTransaction.ToString("ddd MMM, yyyy hh:mm tt") + ". Play more @ https://wimbig.com Thanks.";
                string emailMessage = string.Format("{0};??{1};??{2};??{3}", "Raffle Notification", "Play Notification", "Welcome " + user.FullName, emailMessageBody);

                string smsmessage = "Your Ref ID " + result + " played ticket" + ticketlist + " with N" + TotalAmount + " for Raffle: " + raffleinfo.Name + " with ID: " + raffleinfo.Id + " on " + transaction.DateOfTransaction.ToString("ddd MMM, yyyy hh:mm tt");

                await SendMessage(emailMessage, user.Email, MessageChannel.Email, MessageType.Activation);

                await SendMessage(smsmessage, user.PhoneNumber, MessageChannel.SMS, MessageType.Activation);


                return Redirect("/Identity/Account/Manage/RaffleHistory");
            }
            catch (Exception e)
            {
                StatusMessage = "Error: Some of the selected tickes are not available";

                return Redirect("/Raffles/Raffles/Index");
            }

            // return RedirectToPage("Index");
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