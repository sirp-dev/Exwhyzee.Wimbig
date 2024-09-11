using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Application.Wimbank.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Core.WimBank;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.Wallets;
using Exwhyzee.Wimbig.Data.Repository.Wimbank;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Wimbank
{
    public class WimbankAppService : IWimbankAppService
    {
        private readonly IWimbankRepository _wimbankRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletAppService _walletAppService;
        private readonly ILogger _logger;
        private readonly IMessageStoreRepository _messageStoreRepository;


        public WimbankAppService(ILogger<WimbankAppService> logger,IWimbankRepository wimbankRepository, IMessageStoreRepository messageStoreRepository,
                                 IWalletAppService walletAppService,UserManager<ApplicationUser> userManager,IWalletRepository walletRepository)
        {
            _wimbankRepository = wimbankRepository;
            _walletRepository = walletRepository;
            _walletAppService = walletAppService;
            _messageStoreRepository = messageStoreRepository;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<WimbankDto> CreateWimbank(WimbankDto wimbank)
        {
            var data = new WimBank
            {
                Amount = wimbank.Amount,
                DateOfTransaction = wimbank.DateOfTransaction,
                TransactionStatus = wimbank.TransactionStatus,
                UserId = wimbank.UserId,
                Balance = wimbank.Balance,
                Note = wimbank.Note,
                ReceiverId = wimbank.UserId
            };
            try
            {
                    var result = await _wimbankRepository.InsertWimbank(data);            
                return new WimbankDto
                {
                    Amount = result.Amount,
                    DateOfTransaction = result.DateOfTransaction,
                    Id = result.Id,
                    TransactionStatus = result.TransactionStatus,
                    UserId = result.UserId,
                    Balance = result.Balance,
                    Note = result.Note,
                    ReceiverId = wimbank.UserId
                
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Insert Error");
                return new WimbankDto();
            }
        }

        public async Task<PagedList<WimbankDto>> GetAllWimbank(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<WimbankDto> wimbank = new List<WimbankDto>();

            var query = await _wimbankRepository.GetAllWimbank(status, dateStart, dateEnd, startIndex, count, searchString);

            wimbank.AddRange(query.Source.Select(x => new WimbankDto()
            {
                Amount = x.Amount,
                DateOfTransaction = x.DateOfTransaction,
                Id = x.Id,
                TransactionStatus = x.TransactionStatus,
                UserId = x.UserId,
                Username = x.Username,
                Balance = x.Balance,
                PhoneNumber = x.PhoneNumber,
                Note = x.Note
            }));

            return new PagedList<WimbankDto>(source: wimbank, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }

        public async Task<WimbankDto> GetWimbank(long id)
        {
            var wimbank = await _wimbankRepository.GetWimbank(id);

            var data = new WimbankDto
            {
                Amount = wimbank.Amount,
                DateOfTransaction = wimbank.DateOfTransaction,
                Id = wimbank.Id,
                TransactionStatus = wimbank.TransactionStatus,
                UserId = wimbank.UserId,
                Username = wimbank.Username,
                Balance = wimbank.Balance,
                Note = wimbank.Note
            };

            return data;
        }

        public async Task<WimbankDto> UpdateWimbank(WimbankDto wimbank)
        {            
            var data = new WimBank
            {
                Id = wimbank.Id,
                Username = wimbank.Username,
                Amount = wimbank.Amount,
                DateOfTransaction = wimbank.DateOfTransaction,
                TransactionStatus = wimbank.TransactionStatus,
                Balance = wimbank.Balance,
                UserId = wimbank.UserId,
                Note = wimbank.Note

            };

            var result = await _wimbankRepository.UpdateWimbank(data);
       
            return new WimbankDto
            {
                Amount = data.Amount,
                DateOfTransaction = data.DateOfTransaction,
                Id = data.Id,
                TransactionStatus = data.TransactionStatus,
                Balance = data.Balance,
                UserId = data.UserId,
                Username = data.Username,
                Note = data.Note
            };
        }

        public async Task<WimbankDto> WimbankLastRecord()
        {
            var wimbank = await _wimbankRepository.GetWimbankLastRecord();

            var data = new WimbankDto
            {
                Amount = wimbank.Amount,
                DateOfTransaction = wimbank.DateOfTransaction,
                Id = wimbank.Id,
                TransactionStatus = wimbank.TransactionStatus,
                UserId = wimbank.UserId,
                Username = wimbank.Username,
                Balance = wimbank.Balance,
                Note = wimbank.Note
            };

            return data;
        }


        ///
        public async Task<long> CreateWimbankTransfer(WimTransferDto transferMoney)
        {
            var wimtransfer = new WimbankTransfer
            {
                UserId = transferMoney.UserId,
                ReceiverId = transferMoney.ReceiverId,
                Amount = transferMoney.Amount,
                Note = transferMoney.Note,
                ReceiverPhone = transferMoney.ReceiverPhone,
                DateOfTransaction = DateTime.UtcNow.AddHours(1),
                TransactionStatus = Enums.TransactionTypeEnum.Credit

            };

            WimBank wimbankbal = await _wimbankRepository.GetWimbankLastRecord();

            wimbankbal.Balance = wimbankbal.Balance -= transferMoney.Amount;

            var wimbank = new WimBank
            {
                UserId = wimbankbal.UserId,
                Balance = wimbankbal.Balance,
                DateOfTransaction = DateTime.UtcNow.AddHours(1),
                Amount = transferMoney.Amount,
                TransactionStatus = Enums.TransactionTypeEnum.Debit,
                Note = transferMoney.Note,
                ReceiverId = transferMoney.ReceiverId

            };

            Wallet wallet = await _walletRepository.GetWallet(transferMoney.ReceiverId);
            wallet.Balance = wallet.Balance += transferMoney.Amount;

            var transaction = new Transaction
            {
                Amount = transferMoney.Amount,
                DateOfTransaction = DateTime.UtcNow.AddHours(1),
                Status = Enums.EntityStatus.Success,
                TransactionType = Enums.TransactionTypeEnum.Credit,
                UserId = transferMoney.ReceiverId,
                WalletId = wallet.Id,
                Sender = "Wim Bank",
                TransactionReference = transferMoney.Note,
                Description = "Wimbank"

            };

            var result = await _wimbankRepository.InsertWimbankTransfer(wimtransfer, wimbank, wallet, transaction);

            //receiver
            var receiverUser = await _userManager.FindByIdAsync(transferMoney.ReceiverId);
            var walletcurent = await _walletRepository.GetWallet(transferMoney.ReceiverId);

            string receiverEmailMessageBody = "N" + transferMoney.Amount + " on " + transaction.DateOfTransaction.ToLongDateString() + ". Your Wimbig Wallet Balance is " + walletcurent.Balance + ". Play more @ https://wimbig.com Thanks.";
            string receiverEmailMessage = string.Format("{0};??{1};??{2};??{3}", "Wimpay Credit", "Transaction Notification", "Dear " + receiverUser.FullName, receiverEmailMessageBody);
            string receiverSMSMessage = "Wimbank Credit N" + transferMoney.Amount + " on " + transaction.DateOfTransaction.ToShortDateString() + ". Your Wimbig Wallet Balance is " + walletcurent.Balance;

            _logger.LogInformation("Pusing Reciver Email To Store");
            await SendMessage(receiverEmailMessage, receiverUser.Email, MessageChannel.Email, MessageType.Activation);
            _logger.LogInformation("Pusing Reciver SMS To Store");
            await SendMessage(receiverSMSMessage, receiverUser.PhoneNumber, MessageChannel.SMS, MessageType.Activation);

            return result.Id;
        }

        public async Task<PagedList<WimbankTransferDto>> GetAllWimbankTransfer(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<WimbankTransferDto> wimbank = new List<WimbankTransferDto>();

            var query = await _wimbankRepository.GetAllWimbankTransfer(status, dateStart, dateEnd, startIndex, count, searchString);

            wimbank.AddRange(query.Source.Select(x => new WimbankTransferDto()
            {
                Amount = x.Amount,
                DateOfTransaction = x.DateOfTransaction,
                Id = x.Id,
                TransactionStatus = x.TransactionStatus,
                UserId = x.UserId,
                Username = x.Username,
                ReceiverId = x.ReceiverId,
                Note = x.Note
            }));

            return new PagedList<WimbankTransferDto>(source: wimbank, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }

        public async Task<WimbankTransferDto> GetWimbankTransfer(long id)
        {
            var wimbank = await _wimbankRepository.GetWimbankTransfer(id);

            var data = new WimbankTransferDto
            {
                Amount = wimbank.Amount,
                DateOfTransaction = wimbank.DateOfTransaction,
                Id = wimbank.Id,
                TransactionStatus = wimbank.TransactionStatus,
                UserId = wimbank.UserId,
                Username = wimbank.Username,
                ReceiverId = wimbank.ReceiverId,
                Note = wimbank.Note,
                ReceiverPhone = wimbank.ReceiverPhone
            };

            return data;

        }

        public async Task<WimbankTransferDto> UpdateWimbankTransfer(WimbankTransferDto wimbank)
        {

            var data = new WimbankTransfer
            {
                Id = wimbank.Id,
                Username = wimbank.Username,
                Amount = wimbank.Amount,
                DateOfTransaction = wimbank.DateOfTransaction,
                TransactionStatus = wimbank.TransactionStatus,
                ReceiverId = wimbank.ReceiverId,
                UserId = wimbank.UserId,
                Note = wimbank.Note,
                ReceiverPhone = wimbank.ReceiverPhone

            };

            var result = await _wimbankRepository.UpdateWimbankTransfer(data);


            return new WimbankTransferDto
            {
                Amount = data.Amount,
                DateOfTransaction = data.DateOfTransaction,
                Id = data.Id,
                TransactionStatus = data.TransactionStatus,
                ReceiverId = data.ReceiverId,
                UserId = data.UserId,
                Username = data.Username,
                Note = data.Note,
                ReceiverPhone = data.ReceiverPhone
            };

        }

        ///
        public async Task<long> WimBankUpdateOfWallet(TransactionDto transaction, WalletDto wallet)
        {
            var transactionData = new Transaction
            {
                Id = transaction.Id,
                Username = transaction.Username,
                Amount = transaction.Amount,
                DateOfTransaction = transaction.DateOfTransaction,
                Status = transaction.Status,
                TransactionType = transaction.TransactionType,
                UserId = transaction.UserId,
                WalletId = transaction.WalletId,
                TransactionReference = transaction.TransactionReference

            };


            var walletData = new Wallet
            {
                Balance = wallet.Balance,
                DateUpdated = wallet.DateUpdated,
                UserId = wallet.UserId,
                Id = wallet.Id
            };


            var result = await _wimbankRepository.WimbankUpdateUser(walletData, transactionData);
            return result.Id;

        }

        public async Task<long> CreateWimPay(WimpayDto wimpay)
        {
            try
            {
                var transactionDataSender = new Transaction
                {
                    WalletId = wimpay.Senderwalletid,
                    UserId = wimpay.Sender,
                    Amount = wimpay.Amount,
                    TransactionType = Enums.TransactionTypeEnum.TransferDebit,
                    DateOfTransaction = DateTime.UtcNow.AddHours(1),
                    Status = Enums.EntityStatus.Success,                   
                    TransactionReference = wimpay.Sender,
                    Description = "Wimpay Transaction"
                };
                var transactionDataReceiver = new Transaction
                {
                    WalletId = wimpay.Receiverwalletid,
                    UserId = wimpay.ReceiverId,
                    Amount = wimpay.Amount,
                    TransactionType = Enums.TransactionTypeEnum.TransferCredit,
                    DateOfTransaction = DateTime.UtcNow.AddHours(1),
                    Status = Enums.EntityStatus.Success,
                    TransactionReference = wimpay.Sender,
                    Description = "Wimpay Transaction"
                };

                var senderwalletid = await _walletAppService.GetWallet(wimpay.Sender);
                var receiverwalletid = await _walletAppService.GetWallet(wimpay.ReceiverId);

                var walletDataSender = new Wallet
                {
                    Balance = senderwalletid.Balance - wimpay.Amount,
                    DateUpdated = DateTime.UtcNow.AddHours(1),
                    UserId = wimpay.Sender,
                    Id = senderwalletid.Id
                };
                var walletDataReceiver = new Wallet
                {
                    Balance = receiverwalletid.Balance + wimpay.Amount,
                    DateUpdated = DateTime.UtcNow.AddHours(1),
                    UserId = wimpay.ReceiverId,
                    Id = receiverwalletid.Id
                };


                var result = await _wimbankRepository.WimpayUpdate(transactionDataSender, transactionDataReceiver, walletDataSender, walletDataReceiver);

                var senderUser = await _userManager.FindByIdAsync(transactionDataSender.UserId);

                //receiver
                var receiverUser = await _userManager.FindByIdAsync(transactionDataReceiver.UserId);

                string receiverEmailMessageBody = "Amount N" + transactionDataReceiver.Amount + " with ref no: " + transactionDataReceiver.Id + " on " + transactionDataReceiver.DateOfTransaction.ToLongDateString() + " from " + senderUser.UserName + ". Your Wimbig Wallet Balance is " + walletDataReceiver.Balance + ". Play more @ https://wimbig.com Thanks.";
                var receiverEmailMessage = string.Format("{0};??{1};??{2};??{3}", "Wimpay Credit", "Transaction Notification", "Dear " + receiverUser.FullName, receiverEmailMessageBody);
                string receiverSMSMessage = "Wimpay Credit. Amount N" + transactionDataReceiver.Amount + " with ref no: " + transactionDataReceiver.Id + " on " + transactionDataReceiver.DateOfTransaction.ToShortDateString() + " from " + senderUser.UserName + ". Your Wimbig Wallet Balance is " + walletDataReceiver.Balance;

                _logger.LogInformation("Pusing Reciver Email To Store");
                await SendMessage(receiverEmailMessage, receiverUser.Email, MessageChannel.Email, MessageType.Activation);
                _logger.LogInformation("Pusing Reciver SMS To Store");
                await SendMessage(receiverSMSMessage, receiverUser.PhoneNumber, MessageChannel.SMS, MessageType.Activation);

                //sender
                // At this point, receiver is teated as the current user

                string senderEmailMessageBody = "Amount N" + transactionDataSender.Amount + " with ref no: " + transactionDataSender.Id + " on " + transactionDataSender.DateOfTransaction.ToLongDateString() + " to " + receiverUser.UserName + ". Your Wimbig Wallet Balance is " + walletDataSender.Balance + ". Play more @ https://wimbig.com Thanks.";
                string senderEmailMessage = string.Format("{0};??{1};??{2};??{3}", "Wimpay Debit", "Transaction Notification", "Dear " + senderUser.FullName, senderEmailMessageBody);

                string senderSMSMessage = "Wimpay Debit. Amount N" + transactionDataSender.Amount + " with ref no: " + transactionDataSender.Id + " on " + transactionDataSender.DateOfTransaction.ToShortDateString() + " to " + receiverUser.UserName + ". Your Wimbig Wallet Balance is " + walletDataSender.Balance;

                _logger.LogInformation("Pusing Sender Email To Store");
                await SendMessage(senderEmailMessage, senderUser.Email, MessageChannel.Email, MessageType.Activation);
                _logger.LogInformation("Pusing Sender SMS To Store");
                await SendMessage(senderSMSMessage, senderUser.PhoneNumber, MessageChannel.SMS, MessageType.Activation);

                return result.Id;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message Compose Error");
            }
            return 0;
        }

        private async Task SendMessage(string message, string address,
            MessageChannel messageChannel, MessageType messageType)
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
                _logger.LogError("MessageChanel Not found");
                return;
            }

            await _messageStoreRepository.Add(messageStore);
        }

    }
}
