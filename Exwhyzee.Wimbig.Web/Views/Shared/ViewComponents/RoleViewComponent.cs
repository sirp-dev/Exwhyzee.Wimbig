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
    public class RoleViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleViewComponent(
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
            )
        {
           
            _userManager = userManager;
           
            _roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var item = await GetRoleAsync(id);
            TempData["roles"] = item;
            return View();
        }



        private async Task<string> GetRoleAsync(string id)
        {
            string rolename = "";
            var user = await _userManager.FindByIdAsync(id);
            var role = await _userManager.GetRolesAsync(user);
            if (role != null)
            {
                rolename = string.Join("; ", role);
                }
           
            return rolename;
        }

    }
}
