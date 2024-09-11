using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Application.Wimbank.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Wimbank
{
    public interface IWimbankAppService
    {
        Task<PagedList<WimbankDto>> GetAllWimbank(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<WimbankDto> CreateWimbank(WimbankDto wimbig);

        Task<WimbankDto> UpdateWimbank(WimbankDto wimbig);

        Task<WimbankDto> GetWimbank(long id);
        Task<WimbankDto> WimbankLastRecord();


        Task<PagedList<WimbankTransferDto>> GetAllWimbankTransfer(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<long> CreateWimbankTransfer(WimTransferDto wimbig);

        Task<long> CreateWimPay(WimpayDto wimpay);

        Task<WimbankTransferDto> UpdateWimbankTransfer(WimbankTransferDto wimbig);

        Task<WimbankTransferDto> GetWimbankTransfer(long id);

        Task<long> WimBankUpdateOfWallet(TransactionDto transaction, WalletDto wallet);
    }
}
