using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Exwhyzee.Wimbig.Web.Areas.SuperAdmin.Pages.Dashboard
{
    [Authorize(Roles = "SuperAdmin,mSuperAdmin")]
    public class IndexModel : PageModel
    {

        private readonly IPurchaseTicketAppService _purchaseTicketAppService;


        public IndexModel(IPurchaseTicketAppService purchaseTicketAppService)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
        }

        [BindProperty]
        public List<TicketVM> Input { get; set; }
        public class TicketVM
        {
            public string City { get; set; }
            public int Count { get; set; }
        }
        public async Task<IActionResult> OnGet(int day = 0, DateTime? date = null)
        {
            

            var query = await _purchaseTicketAppService.GetAllTickets();
            List<TicketDto> tickets = new List<TicketDto>();
            List<TicketDto> items = new List<TicketDto>();
            if (day == 0 && date != null)
            {
                DateTime oDate = Convert.ToDateTime(date);
                if (date != null)
                {
                    //string iDate = "05/05/2005";

                    TempData["range"] = "(Date)" + date.Value.Date.ToString("ddd dd MMM, yyyy");

                }
                if (date != null)
                {
                    items = query.Source.Where(x => x.Date.Date == date.Value.Date).ToList();
                }
            }
            else if (day > 0 && date == null)
            {


                if (day > 1)
                {
                    TempData["range"] = "From Today " + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy") + " to " + DateTime.UtcNow.AddDays(-day).Date.ToString("ddd dd MMM, yyyy") + "( " + day + "days Ago)";

                }
                else
                {
                    TempData["range"] = "(Today)" + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy");

                }
                if (day > 1)
                {
                    items = query.Source.Where(x => x.Date.Date >= DateTime.UtcNow.AddDays(-day).Date && x.Date.Date <= DateTime.UtcNow.Date).ToList();
                }
                else
                {
                    items = query.Source.Where(x => x.Date.Date == DateTime.UtcNow.Date).ToList();
                }
            }
            else if (day == 0 && date == null)
            {

                TempData["range"] = "(Today)" + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy");

                items = query.Source.Where(x => x.Date.Date == DateTime.UtcNow.Date).ToList();

            }
            else
            {
                TempData["error"] = "Choose either Date or Days. Thank you";


                TempData["range"] = "(Today)" + DateTime.UtcNow.Date.ToString("ddd dd MMM, yyyy");

                items = query.Source.Where(x => x.Date.Date == DateTime.UtcNow.Date).ToList();

            }


            var reports = items.GroupBy(r => r.CurrentLocation).Take(10)
               .Select(r => new TicketVM()
               {
                   City = r.Key,
                   Count = r.Count()
               });
                     

        var list = (from ticks in reports
                    select new TicketVM
                    {
                       City = ticks.City,
                       Count = ticks.Count
                    }).ToList();
            Input = list.OrderByDescending(x=>x.Count).ToList();
            return Page();
        }
    }
}