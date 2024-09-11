using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Core.MessageStores;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Exwhyzee.Wimbig.RaffleDrawBgService
{
    public class RaffleDrawService : IMicroService
    {
        private IMicroServiceController controller;
        private readonly IRaffleAppService raffleAppService;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;
       

        private Timer timer = new Timer(10000);

        public RaffleDrawService(IMicroServiceController controller,
            IRaffleAppService raffleAppService,
            IPurchaseTicketAppService purchaseTicketAppService)
        {
            this.controller = controller;
            this.raffleAppService = raffleAppService;
            this.purchaseTicketAppService = purchaseTicketAppService;

        }


        public void Start()
        {
            Console.WriteLine("Raffle Draw Service Started.");
           
           
            timer.Elapsed += ProccessDraw;
            timer.Start();
        }

        private async void ProccessDraw(object sender, ElapsedEventArgs e)
        {
            //Get Raffles that are complete but still Active
            Console.WriteLine("Get Raffles that are complete but still Active");
            var raffles = await GetRafflesByStatus((int)EntityStatus.Active, 10);

            Console.WriteLine("Updating Raffles");
            if (raffles.Count > 0)
                await UpdateRafflesStatus(raffles, (int)EntityStatus.Processing);


            //Get Raffles that are complete but still Active
            Console.WriteLine("Get "+raffles.Count()+" Raffles that are Processing");
            var rafflesToProcess = await GetRafflesByStatus((int)EntityStatus.Processing, 10);

            //Process the Raffles
            await RaffleDraw(rafflesToProcess);

            await SendToLotteryCommision();
        }

        public void Stop()
        {
            
        }



        private async Task<List<RaffleDto>> GetRafflesByStatus(int status, int count)
        {
            var raffles = await raffleAppService.GetRaffleByStatus(status, 10);

            return raffles;
        }

        private async Task UpdateRafflesStatus(List<RaffleDto> raffles, int status)
        {
            foreach(var item in raffles)
            {
                item.Status = (EntityStatus)status;
                item.DateWon = DateTime.UtcNow.AddHours(1);
                await raffleAppService.Update(item);
            }

        }

        private async Task SendToLotteryCommision()
        {
            //Send to National Lottery Regulator Commission Games Statistics

           var raffles = await GetRafflesByStatus((int)EntityStatus.Drawn, 10);
            var updated = new List<RaffleDto>();
            foreach (var item in raffles)
            {
                DrawServiceReference.DrawDetailsServiceClient client = new DrawServiceReference.DrawDetailsServiceClient();

                var tickets = await purchaseTicketAppService.GetAllTickets(raffleId: item.Id);

                var ticketId = tickets.Source.FirstOrDefault(x => x.IsWinner == true);

                if (ticketId != null)
                {
                    var result = await client.DrawAsync(new DrawServiceReference.DrawRequest
                    {
                        drawingDateTime = DateTime.Now.ToString("yyyyMMddHHmm"),
                        gameType = 292,
                        permitHolderId = 42
                    });

                    if (!string.IsNullOrEmpty(result.DrawResponse.transactionNumber))
                    {
                        updated.Add(item);
                    }
                    
                }


            }

            await UpdateRafflesStatus(updated, (int)EntityStatus.Closed);

        }

        private async Task RaffleDraw(List<RaffleDto> raffleDtos)
        {
            if(raffleDtos.Count > 0)
            {
                var updated = new List<RaffleDto>();
                Random random = new Random();
                foreach (var item in raffleDtos)
                {

                   var tickets = await purchaseTicketAppService.GetAllTickets(raffleId: item.Id);
                    var check = tickets.Source.Any(x => x.IsWinner == true);
                    if (tickets != null && !check)
                    {
                        var winningTicket = tickets.Source.OrderBy(x => random.NextDouble()).FirstOrDefault();

                        var ticketId = await purchaseTicketAppService.UpdateTicket(winningTicket.Id);
                        Console.WriteLine("updated ticket winner.");

                        try
                        {
                          //  var winnerticket = await purchaseTicketAppService.GetById(winningTicket.Id);
                            string emailMessageBody = " You are the winner for our wimbig Raffle on " + winningTicket.RaffleName + " Raffle, ID " + winningTicket.RaffleId + " with Ticket Number " + winningTicket.TicketNumber + " on " + winningTicket.DateWon + " Phone: " + winningTicket.PhoneNumber + " and email " + winningTicket.Email + ". Play more @ https://wimbig.com Thanks.";
                            var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Wimbig Winner", "Congratulations", "Hello! " + winningTicket.PlayerName, emailMessageBody);

                            string smsmessage = "Wimbig Winner! You " + winningTicket.PlayerName + " won " + winningTicket.RaffleName + " of Id " + winningTicket.RaffleId + " with ticket number " + winningTicket.TicketNumber + " on " + winningTicket.DateWon + ". Play more @ https://wimbig.com Thanks.";

                            await SendMessage(emailMessage, winningTicket.Email, MessageChannel.Email, MessageType.Activation);

                            await SendMessage(smsmessage, winningTicket.PhoneNumber, MessageChannel.SMS, MessageType.Activation);


                        } catch(Exception c) { }
                        if (ticketId > 0)
                        {
                            updated.Add(item);

                           
                        }
                    }
                                  
                
                    
                }

                await UpdateRafflesStatus(updated, (int)EntityStatus.Drawn);
                Console.WriteLine("Updtated status to drawn.");

            }

            Console.WriteLine("No Raffle Draw to process.");
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
