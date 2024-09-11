using Exwhyzee.Wimbig.Application.Paystack;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    public class FundingModel : PageModel
    {
       


        public async Task<IActionResult> OnGetAsync()
        {

          

            return Page();
        }


      

    }
}