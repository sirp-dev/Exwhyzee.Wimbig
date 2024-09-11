using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Data.Repository.Transactions;
using Exwhyzee.Wimbig.Data.Repository.Wallets;

namespace Exwhyzee.Wimbig.Application.Transactions
{
    public class TransactionAppService : ITransactionAppService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        public TransactionAppService(ITransactionRepository transactionRepository, IWalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
        }

        public async Task<TransactionDto> CreateTransaction(InsertTransactionDto transaction)
        {
            var data = new Transaction
            {
                Amount = transaction.Amount,
                DateOfTransaction = transaction.DateOfTransaction,
                Status = transaction.Status,
                TransactionType = transaction.TransactionType,
                UserId = transaction.UserId,
                WalletId = transaction.WalletId,
                TransactionReference = transaction.TransactionReference,
                Description = transaction.Description

            };

            var result = await _transactionRepository.InsertTransaction(data);


            return new TransactionDto
            {
                Amount = result.Amount,
                DateOfTransaction = result.DateOfTransaction,
                Id = result.Id,
                Status = result.Status,
                TransactionType = result.TransactionType,
                UserId = result.UserId,
                Username = result.Username,
                WalletId = result.WalletId,
                TransactionReference = result.TransactionReference,
                Description = result.Description
            };
        }

        public async Task<PagedList<TransactionDto>> GetAllTransactions(string userId = null, long? walletId = null, int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<TransactionDto> transactions = new List<TransactionDto>();

            var query = await _transactionRepository.GetAllTransactions(userId, walletId, status, dateStart, dateEnd, startIndex, count, searchString);

            transactions.AddRange(query.Source.Select(x => new TransactionDto()
            {
                Amount = x.Amount,
                DateOfTransaction = x.DateOfTransaction,
                Id = x.Id,
                Status = x.Status,
                TransactionType = x.TransactionType,
                UserId = x.UserId,
                Username = x.Username,
                WalletId = x.WalletId,
                TransactionReference = x.TransactionReference,
                Description = x.Description
            }));

            #region cooment
            ///update transactions
            ///
            // var jj = transactions.Count();

            //var wallet = await _walletRepository.GetWallet();
            //var check = transactions.Where(c => c.Amount.ToString().Contains("-"));
            //foreach (var aa in check)
            //{
            //    var i = await _transactionRepository.GetTransaction(aa.Id);
            //    TransactionDto tran = new TransactionDto();
            //    tran.Id = i.Id;
            //    tran.Username = i.Username;
            //    tran.Amount = i.Amount;
            //    tran.DateOfTransaction = i.DateOfTransaction;
            //    tran.Status = i.Status;
            //    tran.TransactionType = i.TransactionType;
            //    tran.UserId = i.UserId;
            //    var wall = await _walletRepository.GetWallet(i.UserId);
            //    tran.WalletId = wall.Id;
            //    tran.TransactionReference = i.TransactionReference;
            //    tran.Description = "Ticket Transaction";


            //    var data = new Transaction
            //    {
            //        Id = tran.Id,
            //        WalletId = tran.WalletId,
            //        UserId = tran.UserId,
            //        Amount = tran.Amount,
            //        TransactionType = tran.TransactionType,
            //        DateOfTransaction = tran.DateOfTransaction,
            //        TransactionReference = tran.TransactionReference,
            //        Sender = tran.Sender,
            //        Status = tran.Status,
            //        Description = tran.Description,

            //    };
            //    var result = await _transactionRepository.UpdateTransaction(data);
            //}

            //var check2 = transactions.Where(c => c.Sender.Contains("Wim Bank"));
            //foreach (var aa in check2)
            //{
            //    var i = await _transactionRepository.GetTransaction(aa.Id);
            //    TransactionDto tran2 = new TransactionDto();
            //    tran2.Id = i.Id;
            //    tran2.Username = i.Username;
            //    tran2.Amount = i.Amount;
            //    tran2.DateOfTransaction = i.DateOfTransaction;
            //    tran2.Status = i.Status;
            //    tran2.TransactionType = i.TransactionType;
            //    tran2.UserId = i.UserId;
            //    var wall = await _walletRepository.GetWallet(i.UserId);
            //    tran2.WalletId = wall.Id;
            //    tran2.TransactionReference = i.TransactionReference;
            //    tran2.Description = "Wimbank Transaction";


            //    var data2 = new Transaction
            //    {
            //        Id = tran2.Id,
            //        WalletId = tran2.WalletId,
            //        UserId = tran2.UserId,
            //        Amount = tran2.Amount,
            //        TransactionType = tran2.TransactionType,
            //        DateOfTransaction = tran2.DateOfTransaction,
            //        TransactionReference = tran2.TransactionReference,
            //        Sender = tran2.Sender,
            //        Status = tran2.Status,
            //        Description = tran2.Description,

            //    };
            //    var result2 = await _transactionRepository.UpdateTransaction(data2);
            //}

            //var check3 = transactions.Where(c => c.Sender.Contains("Wimpay"));
            //foreach (var aa in check3)
            //{
            //    var i = await _transactionRepository.GetTransaction(aa.Id);
            //    TransactionDto tran2 = new TransactionDto();
            //    tran2.Id = i.Id;
            //    tran2.Username = i.Username;
            //    tran2.Amount = i.Amount;
            //    tran2.DateOfTransaction = i.DateOfTransaction;
            //    tran2.Status = i.Status;
            //    tran2.TransactionType = i.TransactionType;
            //    tran2.UserId = i.UserId;
            //    var wall = await _walletRepository.GetWallet(i.UserId);
            //    tran2.WalletId = wall.Id;
            //    tran2.TransactionReference = i.TransactionReference;
            //    tran2.Description = "Wimpay Transaction";


            //    var data2 = new Transaction
            //    {
            //        Id = tran2.Id,
            //        WalletId = tran2.WalletId,
            //        UserId = tran2.UserId,
            //        Amount = tran2.Amount,
            //        TransactionType = tran2.TransactionType,
            //        DateOfTransaction = tran2.DateOfTransaction,
            //        TransactionReference = tran2.TransactionReference,
            //        Sender = tran2.Sender,
            //        Status = tran2.Status,
            //        Description = tran2.Description,

            //    };
            //    var result2 = await _transactionRepository.UpdateTransaction(data2);
            //}



            #endregion



            return new PagedList<TransactionDto>(source: transactions, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }


        //get transaction by reference id

        public async Task<PagedList<TransactionDto>> GetAllTransactionsByReferenceId(string userId = null, long? walletId = null, int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<TransactionDto> transactions = new List<TransactionDto>();

            var query = await _transactionRepository.GetAllTransactionsByReferenceId(userId, walletId, status, dateStart, dateEnd, startIndex, count, searchString);

            transactions.AddRange(query.Source.Select(x => new TransactionDto()
            {
                Amount = x.Amount,
                DateOfTransaction = x.DateOfTransaction,
                Id = x.Id,
                Status = x.Status,
                TransactionType = x.TransactionType,
                UserId = x.UserId,
                Username = x.Username,
                WalletId = x.WalletId,
                TransactionReference = x.TransactionReference,
                Description = x.Description
            }));

            return new PagedList<TransactionDto>(source: transactions, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }

        public async Task<TransactionDto> GetTransaction(long id)
        {
            var transaction = await _transactionRepository.GetTransaction(id);

            var data = new TransactionDto
            {
                Amount = transaction.Amount,
                DateOfTransaction = transaction.DateOfTransaction,
                Id = transaction.Id,
                Status = transaction.Status,
                TransactionType = transaction.TransactionType,
                UserId = transaction.UserId,
                Username = transaction.Username,
                WalletId = transaction.WalletId,
                TransactionReference = transaction.TransactionReference,
                Description = transaction.Description
            };

            return data;

        }

        public async Task<TransactionDto> UpdateTransaction(TransactionDto transaction)
        {

            var data = new Transaction
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                UserId = transaction.UserId,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                DateOfTransaction = transaction.DateOfTransaction,
                TransactionReference = transaction.TransactionReference,
                Sender = transaction.Sender,
                Status = transaction.Status,
                Description = transaction.Description


            };

            var result = await _transactionRepository.UpdateTransaction(data);


            return new TransactionDto
            {


                Id = data.Id,
                WalletId = data.WalletId,
                UserId = data.UserId,
                Amount = data.Amount,
                TransactionType = data.TransactionType,
                DateOfTransaction = data.DateOfTransaction,
                TransactionReference = data.TransactionReference,
                Sender = data.Sender,
                Status = data.Status,
                Description = data.Description







            };

        }
    }
}
