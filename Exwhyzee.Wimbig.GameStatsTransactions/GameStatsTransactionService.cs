using Exwhyzee.Wimbig.Application.Tickets;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Exwhyzee.Wimbig.GameStatsTransactions
{
    public class GameStatsTransactionService : IMicroService
    {
        private IMicroServiceController controller;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;

        private Timer timer = new Timer(1000);

        public GameStatsTransactionService(IMicroServiceController controller,
            IPurchaseTicketAppService purchaseTicketAppService)
        {
            this.controller = controller;
            this.purchaseTicketAppService = purchaseTicketAppService;
        }



        public void Start()
        {
            timer.Start();
            timer.Elapsed += ProccessGameStats;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async void ProccessGameStats(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            var tickets = await purchaseTicketAppService.GetAllTickets(count: 10, isSentToStat: false) ?? null;

            if(tickets.FilteredCount > 0 )
            {
                //Add to service
                GameStatReference.SaveTransactionServiceClient client = new GameStatReference.SaveTransactionServiceClient();
                foreach(var item in tickets.Source)
                {
                    try
                    {
                        //Send to National Lottery Regulator Commission Games Statistics
                        var result = await client.TransactionAsync(new GameStatReference.TransactionRequest
                        {
                            drawingDateTime = DateTime.UtcNow.AddHours(1).AddDays(30).ToString("yyyyMMddHHmmss"),
                            gameType = 292,
                            permitHolderId = 42,
                            providerIdentifier = "MTN",
                            sourceIdentifier = item.PhoneNumber,
                            source = 1,
                            ticketReference = $"{item.RaffleNumber}-{item.TicketNumber}",
                            transactionAmount = item.Price,
                            transactionTimestamp = $"{item.PurchaseDate.ToString("yyyyMMddHHmmss")}",
                            transactionType = 1,
                            winingDrawDateTime = null

                        });

                        if (!string.IsNullOrEmpty(result.TransactionResponse.transactionNumber))
                        {
                            //TODO:Add transactionNumber to ticket....
                            //TODO:Update the Ticket table with TransactionNumber and make IsSetToStat TRUE
                            var update = await purchaseTicketAppService.UpdateTicketGameStat(item.Id);
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }

                //
            }

            timer.Enabled = false;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }


    }
}
