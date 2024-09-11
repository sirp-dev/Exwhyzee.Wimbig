using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.StakeHolders.Pages.Agents
{
    [Authorize]
    public class TicketConfirmModel : PageModel
    {
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly IRaffleAppService _raffleAppService;
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IMessageStoreRepository _messageStoreRepository;


        private readonly UserManager<ApplicationUser> _userManager;

        public TicketConfirmModel(IPurchaseTicketAppService purchaseTicketAppService,
            ITransactionAppService transactionAppService,
            IMapImageToRaffleAppService mapImageToRaffleAppService,
            IRaffleAppService raffleAppService,
            IMessageStoreRepository messageStoreRepository,
            UserManager<ApplicationUser> userManager,
            IWalletAppService walletAppService)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
            _transactionAppService = transactionAppService;
            _raffleAppService = raffleAppService;
            this.mapImageToRaffleAppService = mapImageToRaffleAppService;
            _userManager = userManager;
            _messageStoreRepository = messageStoreRepository;
            _walletAppService = walletAppService;

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

        public string Email { get; set; }

        [BindProperty]

        public string PlayerName { get; set; }

        [BindProperty]
        public List<ImageOfARaffle> Images { get; set; }
        public WalletDto Wallet { get; set; }


        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync(long id = 0)
        {
            if (id < 0)
                throw new ArgumentNullException(nameof(id));

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

                //var check = await _purchaseTicketAppService.GetAllTickets(raffleId: Raffle.Id);
                //var count = check.Source.Where(x => x.PhoneNumber == PhoneNumber).Count();
                //if (count >= 5)
                //{
                //    TempData["checkaffle"] = "You can not play morethan 5 ticket in a Raffle. you have " + (3 - count) + " chance remaining";

                //    return RedirectToPage("./TicketConfirm", new { id = Raffle.Id });

                //}


                //Create Transaction

                var transaction = new TransactionDto
                {
                    WalletId = wallet.Id,
                    TransactionType = Enums.TransactionTypeEnum.Debit,
                    Amount = -(TotalAmount),
                    DateOfTransaction = DateTime.Now,
                    Status = Enums.EntityStatus.Success,
                    TransactionReference = $"{NumberOfTickets} TICKET(S)",
                    UserId = user.Id,
                    Username = user.UserName,
                    Description = "Ticket Transaction"
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
                            YourPhoneNumber = PhoneNumber,
                            UserId = user.Id,
                            TicketStatus = Enums.TicketStatusEnum.Active,
                            Email = Email,
                            PlayerName = PlayerName,
                            CurrentLocation = user.CurrentCity

                        });
                    
                }

                StatusMessage = $"Your Ticket was successful.";
                long result = await _purchaseTicketAppService.ProcessTicket(transaction, wallet, insertTicketDtos);
                var raffleinfo = await _raffleAppService.GetById(Raffle.Id);

                string emailMessageBody = " Your Ref ID " + result + " played " + NumberOfTickets + " with N" + TotalAmount + " for " + raffleinfo.Name + " Raffle with ID " + raffleinfo.Id + " Phone: " + PhoneNumber + " on " + DateTime.UtcNow.AddHours(1).ToString("ddd MMM, yyyy hh:mm tt") + ".Play more @ https://wimbig.com Thanks.";
                var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Wimbig Raffles", "Ticket Information", "Hello! " + PlayerName, emailMessageBody);

                string smsmessage = "Your Ref ID " + result + " played " + NumberOfTickets + " with N" + TotalAmount + " for " + raffleinfo.Name + " Raffle with ID " + raffleinfo.Id + " Phone: " + PhoneNumber + " on " + DateTime.UtcNow.AddHours(1).ToString("ddd MMM, yyyy hh:mm tt") + ". Play more @ https://wimbig.com Thanks.";
                if (!string.IsNullOrEmpty(Email))
                {
                    await SendMessage(emailMessage, Email, MessageChannel.Email, MessageType.Activation);
                }
                await SendMessage(smsmessage, PhoneNumber, MessageChannel.SMS, MessageType.Activation);


                return RedirectToPage("./Print", new { id = result });
            }
            catch (Exception e)
            {
                return Redirect("/StakeHolders/Agents/Index");
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

        private string GenerateNumber(long raffleId)
        {
            var number = DateTime.Now.ToString("yyMMddHHmmssff") + raffleId;

            return number;

        }
    }
}