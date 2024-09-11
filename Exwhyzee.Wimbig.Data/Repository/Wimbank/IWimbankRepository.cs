using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Core.WimBank;
using System;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Wimbank
{
    public interface IWimbankRepository
    {
        Task<PagedList<WimBank>> GetAllWimbank(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<WimBank> InsertWimbank(WimBank wimbank);

        Task<WimBank> UpdateWimbank(WimBank wimbank);

        Task<WimBank> GetWimbank(long wimbankId);

        Task<WimBank> GetWimbankLastRecord();


        Task<PagedList<WimbankTransfer>> GetAllWimbankTransfer(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);
        Task<WimbankTransfer> InsertWimbankTransfer(WimbankTransfer wimtransfer, WimBank wimBank, Wallet wallet, Transaction transaction);

        Task<WimbankTransfer> UpdateWimbankTransfer(WimbankTransfer wimbank);

        Task<WimbankTransfer> GetWimbankTransfer(long wimbankTransfeId);

        Task<Transaction> WimbankUpdateUser(Wallet wallet, Transaction transaction);

       Task<Transaction> WimpayUpdate(Transaction transactionDataSender, Transaction transactionDataReceiver, Wallet walletDataSender, Wallet walletDataReceiver);

    }
}
