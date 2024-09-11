using Exwhyzee.Wimbig.Application.Transactions.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Transactions
{
    public interface IWalletAppService
    {
        Task<PagedList<WalletDto>> GetAllWallets(int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<long> InsertWallet(InsertWalletDto wallet);

        Task<WalletDto> GetWallet(string userId);

        Task UpdateWallet(WalletDto wallet);
    }
}
