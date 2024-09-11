using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class OthersModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;

        public OthersModel(IRaffleAppService raffleAppService)
        {
            _raffleAppService = raffleAppService;
        }

        public PagedList<Raffle> Raffles { get; set; }
        List<Raffle> raffles = new List<Raffle>();

        public async Task<IActionResult> OnGetAsync()
        {
            var paggedRaffles = await _raffleAppService.GetAllRaffles();
            var paggedSource = paggedRaffles.Source.Where(x=>x.Status != Enums.EntityStatus.Active && x.Status != Enums.EntityStatus.Drawn).ToList();


            raffles.AddRange(paggedSource.Select(x => new Raffle()
            {
                DeliveryType = x.DeliveryType,
                Description = x.Description,
                EndDate = x.EndDate,
                HostedBy = x.HostedBy,
                Id = x.Id,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets,
                PricePerTicket = x.PricePerTicket,
                StartDate = x.StartDate,
                Status = x.Status,
                Username = x.Username,
                DateCreated = x.DateCreated,
                TotalSold = x.TotalSold,
                SortOrder = x.SortOrder,
                Archived = x.Archived,
                PaidOut = x.PaidOut,
                Location = x.Location

            }));

             Raffles = new PagedList<Raffle>(source: raffles, pageIndex: paggedRaffles.PageIndex,
                                            pageSize: paggedRaffles.PageSize, filteredCount: paggedRaffles.FilteredCount, totalCount:
                                            paggedRaffles.TotalCount);
            return Page();
        }

    }
}