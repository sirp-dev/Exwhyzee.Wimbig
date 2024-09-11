using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class EditRaffleModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        private readonly IMapImageToRaffleAppService _mapImageToRaffleApp;
        private readonly IImageFileAppService _imageFileAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICityAppService _cityAppService;

        public string LoggedInUser { get; set; }

        public EditRaffleModel(IRaffleAppService raffleAppService,
             ICityAppService cityAppService, UserManager<ApplicationUser> userManger, 
            IMapImageToRaffleAppService mapImageToRaffleApp, IImageFileAppService imageFileAppService)
        {
            _raffleAppService = raffleAppService;
            _userManager = userManger;
            _cityAppService = cityAppService;
            _mapImageToRaffleApp = mapImageToRaffleApp;
            _imageFileAppService = imageFileAppService;
        }

        [BindProperty]
        public RaffleDto Raffle { get; set; }

        [BindProperty]
        public int sort { get; set; }

        [BindProperty]
        public string CityInfo { get; set; }

        [BindProperty]
        public string AreaInCity { get; set; }

        //[TempData]
        //public string StatusMessage { get; private set; }
        [BindProperty]
        public List<SelectListItem> CityDtoList { get; set; }


        public async Task<IActionResult> OnGetAsync(long id)
        {
            LoggedInUser = _userManager.GetUserId(HttpContext.User);
            var raffle = await _raffleAppService.GetById(id);
            //var raffleImageid = await _mapImageToRaffleApp.GetById(raffle.Id);
          //  var raffleimage = await _imageFileAppService.GetById(raffleImageid.ImageId);
            string imageUrl = "";
            if (raffle == null)
            {
                return NotFound($"Unable to load raffle with the ID '{id}'.");
            }
         

            Raffle = new RaffleDto
            {
                
                DeliveryType = raffle.DeliveryType,
                Description = raffle.Description,
                EndDate = raffle.EndDate,
                HostedBy = raffle.HostedBy,
                Id = raffle.Id,
                Name = raffle.Name,
                NumberOfTickets = raffle.NumberOfTickets,
                PricePerTicket = raffle.PricePerTicket,
                StartDate = raffle.StartDate,
                Status = raffle.Status,
                PaidOut = raffle.PaidOut,
                Archived = raffle.Archived,
                SortOrder = raffle.SortOrder,
                Location = raffle.Location,
                AreaInCity = raffle.AreaInCity




            };

            List<CityDto> city = new List<CityDto>();

            var query = await _cityAppService.GetAsync();

            city.AddRange(query.Source.Select(entity => new CityDto()
            {
                Id = entity.Id,
                Name = entity.Name
            }));
            CityDtoList = city.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Name
                                }).ToList();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                List<CityDto> city = new List<CityDto>();

                var query = await _cityAppService.GetAsync();

                city.AddRange(query.Source.Select(entity => new CityDto()
                {
                    Id = entity.Id,
                    Name = entity.Name
                }));
                CityDtoList = city.Select(a =>
                                    new SelectListItem
                                    {
                                        Value = a.Id.ToString(),
                                        Text = a.Name
                                    }).ToList();
                return Page();
            }
            
            if(CityInfo == null || AreaInCity == null)
            {
                List<CityDto> city = new List<CityDto>();

                var query = await _cityAppService.GetAsync();

                city.AddRange(query.Source.Select(entity => new CityDto()
                {
                    Id = entity.Id,
                    Name = entity.Name
                }));
                CityDtoList = city.Select(a =>
                                    new SelectListItem
                                    {
                                        Value = a.Id.ToString(),
                                        Text = a.Name
                                    }).ToList();
                TempData["error"] = "Kindly enter your city and area in city";
                return Page();
            }
            long cityId = Convert.ToInt64(CityInfo);
            long AreacityId = Convert.ToInt64(AreaInCity);
            var cityData = await _cityAppService.Get(cityId);
            var AreacityData = await _cityAppService.GetAreaInCity(AreacityId);


            var raffle = await _raffleAppService.GetById(id);
            if (raffle == null)
            {
                return NotFound($"Unable to load Raffle with the ID '{id}'.");
            }

            
            raffle.Description = Raffle.Description;
            raffle.SortOrder = Raffle.SortOrder;
            raffle.PaidOut = Raffle.PaidOut;
            raffle.Archived = Raffle.Archived;

            raffle.DeliveryType = Raffle.DeliveryType;
           
            raffle.Name = Raffle.Name;
            raffle.NumberOfTickets = Raffle.NumberOfTickets;
            raffle.PricePerTicket = Raffle.PricePerTicket;
            raffle.Location = cityData.Name;
            raffle.AreaInCity = AreacityData.Name;
           

            await _raffleAppService.Update(raffle);

            //StatusMessage = "The Selected Raffle has been updated";
            return RedirectToPage("./Index");
        }

    }
}