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

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "mSuperAdmin")]
    public class InvokeModel : PageModel
    {
        private readonly IWalletAppService walletAppService;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;
        private readonly IRaffleAppService raffleAppService;
        private readonly IMapImageToRaffleAppService mapImageToRaffleService;

        private readonly UserManager<ApplicationUser> userManager;


        public InvokeModel(UserManager<ApplicationUser> userManager, IMapImageToRaffleAppService mapImageToRaffleService, IPurchaseTicketAppService purchaseTicketAppService, IWalletAppService walletAppService, IMapImageToRaffleAppService mapImageToRaffleAppService, IRaffleAppService raffleAppService)
        {
            this.purchaseTicketAppService = purchaseTicketAppService;
            this.raffleAppService = raffleAppService;
            this.walletAppService = walletAppService;
            this.userManager = userManager;
            this.mapImageToRaffleService = mapImageToRaffleService;
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
                        string url = HttpContext.Request.Host.ToString();
                        string imge = "";
                        var raffledetails = await raffleAppService.GetById(id);
                        //
                        try
                        {
                            var images = await mapImageToRaffleService.GetAllImagesOfARaffle(id, 1);

                            imge = images.FirstOrDefault().Url;
                        }
                        catch (Exception e)
                        {

                        }
                        string imageurl = url + imge;


                        //  var winnerticket = await purchaseTicketAppService.GetById(winningTicket.Id);
                        string emailMessageBody = " You are the winner for our wimbig Raffle on " + winningTicket.RaffleName + " Raffle, ID " + winningTicket.RaffleId + " with Ticket Number " + winningTicket.TicketNumber + " on " + winningTicket.DateWon + " Phone: " + winningTicket.PhoneNumber + " and email " + winningTicket.Email + ". Play more @ https://wimbig.com Thanks.";
                        var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Wimbig Winner", "Congratulations", "Hello! " + winningTicket.PlayerName, emailMessageBody);

                        string smsmessage = "Wimbig Winner! You " + winningTicket.PlayerName + " won " + winningTicket.RaffleName + " of Id " + winningTicket.RaffleId + " with ticket number " + winningTicket.TicketNumber + " on " + winningTicket.DateWon + ". Play more @ https://wimbig.com Thanks.";
                        winner = emailMessage + " ........................ " + smsmessage;
                        await SendMessage(emailMessage, winningTicket.Email, MessageChannel.Email, MessageType.Activation, imageurl);

                        await SendMessage(smsmessage, winningTicket.PhoneNumber, MessageChannel.SMS, MessageType.Activation, null);

                        List<string> mylist = new List<string>(new string[] { "onwukaemeka41@gmail.com", "judengama@gmail.com" });
                        var users = mylist;
                        foreach (var newemail in users)
                        {
                            string emailMessageBodyAdmin = " The winner for our wimbig Raffle on " + winningTicket.RaffleName + " Raffle, ID " + winningTicket.RaffleId + " with Ticket Number " + winningTicket.TicketNumber + " on " + winningTicket.DateWon + " Phone: " + winningTicket.PhoneNumber + " and email " + winningTicket.Email;
                            var emailMessageAdmin = string.Format("{0};??{1};??{2};??{3}", "Wimbig Winner", "Winner Info", "Hello! " + winningTicket.PlayerName, emailMessageBodyAdmin);

                            await SendMessage(emailMessage, newemail, MessageChannel.Email, MessageType.Activation, imageurl);

                        }

                    }
                    catch (Exception c) { }

                }


                // await UpdateRafflesStatus(updated, (int)EntityStatus.Drawn);
                var item = await raffleAppService.GetById(id);

                item.Status = EntityStatus.Drawn;
                item.Archived = true;
                item.SortOrder = 18;
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
     MessageChannel messageChannel, MessageType messageType, string imageurl)
        {
            try
            {


                var messageStore = new MessageStore()
                {
                    MessageChannel = messageChannel,
                    MessageType = messageType,
                    Message = message,
                    AddressType = AddressType.Single,
                    ImageUrl = imageurl
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