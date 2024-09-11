using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Transactions
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]

    public class IndexModel : PageModel
    {
        private readonly ITransactionAppService _transactionAppService;

        public IndexModel(ITransactionAppService transactionAppService)
        {
            _transactionAppService = transactionAppService;
        }

        public PagedList<TransactionDto> Transactions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Transactions = await _transactionAppService.GetAllTransactions();

            return Page();
        }
    }
}