using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Wallets
{
    public class IndexModel : PageModel
    {
        private readonly IWalletAppService _walletAppService;

        public IndexModel(IWalletAppService walletAppService)
        {
            _walletAppService = walletAppService;
        }

        public PagedList<WalletDto> Wallets { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Wallets = await _walletAppService.GetAllWallets();
            return Page();
        }
    }
}