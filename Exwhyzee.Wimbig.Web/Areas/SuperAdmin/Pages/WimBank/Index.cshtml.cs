using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Wimbank;
using Exwhyzee.Wimbig.Application.Wimbank.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.WimBank
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IWimbankAppService _wimbankAppService;

        public IndexModel(IWimbankAppService wimbankAppService)
        {
            _wimbankAppService = wimbankAppService;
        }

        public PagedList<WimbankDto> Results { get; set; }
        public decimal Balance { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //get last record
            try
            {


                var lastBank = await _wimbankAppService.WimbankLastRecord();
                if (lastBank == null)
                {
                    return Redirect("/SuperAdmin/WimBank/TopUp");
                }
                Balance = lastBank.Balance;
                Results = await _wimbankAppService.GetAllWimbank();
                return Page();
            }catch(Exception c)
            {
                return Redirect("/SuperAdmin/WimBank/TopUp");
            }
        }

    }
}