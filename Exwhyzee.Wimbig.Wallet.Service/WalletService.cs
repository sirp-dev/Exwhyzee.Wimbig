using Exwhyzee.Wimbig.Application.Transactions;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Exwhyzee.Wimbig.Wallet.Service
{
    public class WalletService : MicroService, IMicroService
    {
        private readonly IWalletAppService _walletAppService;
        private readonly ITransactionAppService _transactionAppService;
        private IMicroServiceController _controller;

      
        public WalletService(IWalletAppService walletAppService, 
            IMicroServiceController controller, ITransactionAppService transactionAppService)
        {
            _walletAppService = walletAppService;
            _transactionAppService = transactionAppService;
            _controller = controller;
        }

        public void Start()
        {
            StartBase();
            Timers.Start("Poller", 1000, () =>
            {
                WalletBalanceReconciliation().Wait();
            });
            Console.WriteLine("I started");
            
        }

      
        private async Task WalletBalanceReconciliation()
        {
            // get ready transactions
            var transactions = await _transactionAppService.GetAllTransactions();

            var readyTransactions = transactions.Where(x => x.Status == Enums.EntityStatus.Pending && !string.IsNullOrEmpty(x.TransactionReference));


            // update wallet
            foreach(var item in readyTransactions)
            {
                Console.WriteLine($"Transaction by {item.Username} with current Amount {item.Amount}");
                var wallet = await _walletAppService.GetWallet(item.UserId);

                Console.WriteLine($"Wallet of {item.Username} with current Balance {wallet.Balance}");
                wallet.Balance = wallet.Balance + item.Amount;

                await _walletAppService.UpdateWallet(wallet);

                item.Status = Enums.EntityStatus.Active;

                await _transactionAppService.UpdateTransaction(item);

                Console.WriteLine($"Update for {item.Username}'s wallet with {item.Amount} completed");
            }
        }

        public void Stop()
        {
            StopBase();
        }

      
    }
}
