using Exwhyzee.Wimbig.Core.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.Wallets
{
    public interface IWalletRepository
    {
        Task<PagedList<Wallet>> GetAllWallets(int startIndex = 0, int count = int.MaxValue, string searchString = null);

        Task<Wallet> InsertWallet(Wallet wallet);

        Task<Wallet> UpdateWallet(Wallet wallet);

        Task<Wallet> GetWallet(string userId);

    }
}
