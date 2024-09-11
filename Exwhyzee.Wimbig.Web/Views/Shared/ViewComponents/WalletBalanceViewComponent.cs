using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class WalletBalanceViewComponent : ViewComponent
    {
        private readonly IWalletAppService _walletAppService;
        
        public WalletBalanceViewComponent(
           
            IWalletAppService walletAppService)
        {

           
            _walletAppService = walletAppService;

        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            try
            {
                var item = await _walletAppService.GetWallet(id);
                ViewData["balance"] = item.Balance;
                
               
            }
            catch (Exception u)
            {
               
            }

            return View();
        }
        
    }
}
