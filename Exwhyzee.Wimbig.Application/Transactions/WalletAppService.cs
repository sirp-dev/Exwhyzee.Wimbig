using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Transactions;
using Exwhyzee.Wimbig.Data.Repository.Wallets;

namespace Exwhyzee.Wimbig.Application.Transactions
{
    public class WalletAppService : IWalletAppService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletAppService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<PagedList<WalletDto>> GetAllWallets(int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            List<WalletDto> wallets = new List<WalletDto>();

            var query = await _walletRepository.GetAllWallets(startIndex, count, searchString);

            wallets.AddRange(query.Source.Select(x => new WalletDto()
            {
                Balance = x.Balance,
                DateUpdated = x.DateUpdated,
                Id = x.Id,
                UserId = x.UserId
            }));

            return new PagedList<WalletDto>(source: wallets, pageIndex: startIndex, pageSize: count,
                filteredCount: query.FilteredCount,
                totalCount: query.TotalCount);
        }

        public async Task<WalletDto> GetWallet(string userId)
        {

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var data = await _walletRepository.GetWallet(userId);

            if (data == null)
            {
                data = await _walletRepository.InsertWallet(new Wallet
                {
                    Balance = 0,
                    DateUpdated = DateTime.UtcNow.AddHours(1),
                    UserId = userId
                });
            }
          

            var walletDto = new WalletDto
            {
                Balance = data.Balance,
                DateUpdated = data.DateUpdated,
                Id = data.Id,
                UserId = data.UserId

            };

            return walletDto;
        }

        public async Task<long> InsertWallet(InsertWalletDto wallet)
        {
            var data = new Wallet
            {
              Balance = wallet.Balance,
              DateUpdated = wallet.DateUpdated,
              UserId = wallet.UserId
            };

           var result = await _walletRepository.InsertWallet(data);

            return result.Id;
        }

        public async Task UpdateWallet(WalletDto wallet)
        {
            var data = new Wallet
            {
                Balance = wallet.Balance,
                DateUpdated = wallet.DateUpdated,
                UserId = wallet.UserId,
                Id = wallet.Id
            };

            await _walletRepository.UpdateWallet(data);
        }
    }
}
