using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exwhyzee.Enums;

namespace Exwhyzee.Wimbig.Web.Areas.Raffles.Pages.Raffles
{
    public class InvokeModel : PageModel
    {
        private readonly IWalletAppService walletAppService;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;
        private readonly IRaffleAppService raffleAppService;
        private readonly UserManager<ApplicationUser> userManager;


        public InvokeModel(UserManager<ApplicationUser> userManager, IPurchaseTicketAppService purchaseTicketAppService, IWalletAppService walletAppService, IMapImageToRaffleAppService mapImageToRaffleAppService, IRaffleAppService raffleAppService)
        {
            this.purchaseTicketAppService = purchaseTicketAppService;
            this.raffleAppService = raffleAppService;
            this.walletAppService = walletAppService;
            this.userManager = userManager;
        }

        [BindProperty]
        public RaffleDto Raffle { get; set; }

        [BindProperty]
        public List<ImageOfARaffle> Images { get; set; }
        public WalletDto Wallet { get; set; }

        //[Authorize]
        public async Task<IActionResult> OnGetAsync(long id)
        {
            try
            {


                Random random = new Random();
                string winner = "";
                var tickets = await purchaseTicketAppService.GetAllTickets(raffleId: id);
                var check = tickets.Source.Any(x => x.IsWinner == true);
                if (tickets != null && !check)
                {
                    var winningTicket = tickets.Source.OrderBy(x => random.NextDouble()).FirstOrDefault();

                    var ticketId = await purchaseTicketAppService.UpdateTicket(winningTicket.Id);

                    try
                    {
                        //  var winnerticket = await purchaseTicketAppService.GetById(winningTicket.Id);
                        string emailMessageBody = " You are the winner for our wimbig Raffle on " + winningTicket.RaffleName + " Raffle, ID " + winningTicket.RaffleId + " with Ticket Number " + winningTicket.TicketNumber + " on " + winningTicket.DateWon + " Phone: " + winningTicket.PhoneNumber + " and email " + winningTicket.Email + ". Play more @ https://wimbig.com Thanks.";
                        var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Wimbig Winner", "Congratulations", "Hello! " + winningTicket.PlayerName, emailMessageBody);

                        string smsmessage = "Wimbig Winner! You " + winningTicket.PlayerName + " won " + winningTicket.RaffleName + " of Id " + winningTicket.RaffleId + " with ticket number " + winningTicket.TicketNumber + " on " + winningTicket.DateWon + ". Play more @ https://wimbig.com Thanks.";
                        winner = emailMessage + " ........................ " + smsmessage;
                        await SendMessage(emailMessage, winningTicket.Email, MessageChannel.Email, MessageType.Activation);

                        await SendMessage(smsmessage, winningTicket.PhoneNumber, MessageChannel.SMS, MessageType.Activation);


                    }
                    catch (Exception c) { }

                }


                // await UpdateRafflesStatus(updated, (int)EntityStatus.Drawn);
                var item = await raffleAppService.GetById(id);

                item.Status = EntityStatus.Drawn;
                item.DateWon = DateTime.UtcNow.AddHours(1);
                await raffleAppService.Update(item);


                TempData["winner"] = winner;
                return Page();
            }catch(Exception e)
            {
                TempData["error"] = e;
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

                await purchaseTicketAppService.Add(messageStore);
            }
            catch (Exception e)
            {

            }
        }

    }
}