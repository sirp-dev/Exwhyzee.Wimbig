using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Wimbank;
using Exwhyzee.Wimbig.Application.Wimbank.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.SuperAdmin.Pages.WimBank
{
    [Authorize(Roles = "mSuperAdmin")]
    public class TopUpModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWimbankAppService _wimbankAppService;
        private readonly IHostingEnvironment _hostingEnv;

        public TopUpModel(IHostingEnvironment env, IWimbankAppService wimbankAppService, UserManager<ApplicationUser> userManger)
        {
            _hostingEnv = env;
            _wimbankAppService = wimbankAppService;
            _userManager = userManger;

        }


        public WimbankDto wimbankDto { get; set; }

        [BindProperty]
        public AddMoneyV addMoneyV { get; set; }

        public class AddMoneyV
        {
            public string UserId { get; set; }

            public decimal Amount { get; set; }

            public DateTime DateOfTransaction { get; set; }

            public TransactionTypeEnum Status { get; set; }

            public string Note { get; set; }
        }

        public string LoggedInUser { get; set; }

        public string ReturnUrl { get; set; }

        public decimal Balance { get; set; }

        public async Task OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            try
            {
            var lastBank = await _wimbankAppService.WimbankLastRecord();
            Balance = lastBank.Balance;
            }
            catch
            {
                Balance = Convert.ToDecimal(0);
            }
            

            LoggedInUser = _userManager.GetUserId(HttpContext.User);


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                decimal availableBal = Convert.ToDecimal(0);

                if (ModelState.IsValid)
                {
                    //get last record
                   
                   try {
                        var lastBank = await _wimbankAppService.WimbankLastRecord();
                         availableBal = lastBank.Balance + addMoneyV.Amount;
                    }
                    catch
                    {
                        availableBal = addMoneyV.Amount;

                    }
                    addMoneyV.Status = TransactionTypeEnum.Credit;
                    var add = new WimbankDto
                    {
                        Amount = addMoneyV.Amount,
                        TransactionStatus = addMoneyV.Status,
                        UserId = addMoneyV.UserId,
                        DateOfTransaction = DateTime.UtcNow.AddHours(1),
                        Note = addMoneyV.Note,
                        Balance = availableBal, 
                        ReceiverId = addMoneyV.UserId
                    };

                    await _wimbankAppService.CreateWimbank(add);


                    return RedirectToPage("Index");
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}