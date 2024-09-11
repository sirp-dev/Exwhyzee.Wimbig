using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exwhyzee.Wimbig.Web.Models;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Web.Controllers.Base;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Barner;
using Exwhyzee.Enums;
using Microsoft.AspNetCore.Identity;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Hangfire.Core.SMS;
using Hangfire;

namespace Exwhyzee.Wimbig.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRaffleAppService raffleAppService;
        private readonly IMapImageToRaffleAppService mapImageToRaffleAppService;
        private readonly IPurchaseTicketAppService purchaseTicketAppService;
        private readonly IBarnerAppService _barnerAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly ICityAppService _cityAppService;
        private readonly ISmsService _smsService;


        public HomeController(IRaffleAppService raffleAppService, ICityAppService cityAppService, 
            IMessageStoreRepository messageStoreRepository, UserManager<ApplicationUser> userManger, 
            IBarnerAppService barnerAppService, IMapImageToRaffleAppService mapImageToRaffleAppService, 
            IPurchaseTicketAppService purchaseTicketAppService, ISmsService smsService)
        {
            this.raffleAppService = raffleAppService;
            this.mapImageToRaffleAppService = mapImageToRaffleAppService;
            this.purchaseTicketAppService = purchaseTicketAppService;
            _barnerAppService = barnerAppService;
            _userManager = userManger;
            _messageStoreRepository = messageStoreRepository;
            _cityAppService = cityAppService;
            _smsService = smsService;
        }




        public async Task<IActionResult> Index()
        {
            var rafflesitem = await raffleAppService.GetAll(count: 100, status: 1);

            var raffles1 = rafflesitem.Source.Where(x => x.Location == "Global").ToList();
            var raffles2 = rafflesitem.Source.Where(x => x.Location == null).ToList();
            var raffles = raffles1.Concat(raffles2).ToList();

            var user = User.Identity.Name;

            if (user != null)
            {
                var userinfo = await _userManager.FindByNameAsync(user);
                if(userinfo.PhoneNumberConfirmed == false)
                {
 if (userinfo.DateRegistered.AddMinutes(10) > DateTime.UtcNow)
                {
                    ViewBag.pop = "SHowPage";
                }
                }
               
            }



            return View(raffles.Take(15));
        }

        public async Task<IActionResult> IndexGame()
        {
            var rafflesitem = await raffleAppService.GetAll(count: 100, status: 1);
            // var items = rafflesitem.Source.Where(x => x.Location)
            var raffles1 = rafflesitem.Source.Where(x => x.Location == "Global").ToList();
            ViewBag.global = raffles1;
            var raffles2 = rafflesitem.Source.Where(x => x.Location == null).ToList();
            ViewBag.nonglob = raffles2;

            var raffles3 = rafflesitem.Source.ToList();
            ViewBag.active = raffles3;
            var raffles = raffles1.Concat(raffles2).ToList();
            return View(raffles.Take(15));
        }
        public async Task<IActionResult> Winners()
        {
            var winner = await purchaseTicketAppService.GetAllWinners();
            return View(winner);
        }

        [Authorize(Roles = "Admin,SuperAdmin,mSuperAdmin,NewsLetter")]
        public async Task<IActionResult> UserEmails()
        {
            var users = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
            return View(users);
        }

        public IActionResult About()
        {

            return View();
        }

        public IActionResult Agent()
        {

            return View();
        }

        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Career()
        {
            return View();
        }

        public IActionResult Licence()
        {
            return View();
        }


        public IActionResult FAQs()
        {
            return View();
        }

        public IActionResult TermandConditions()
        {
            return View();
        }

        public IActionResult Howtoplay()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult AgentNationwide()
        {
            var usersD = _userManager.GetUsersInRoleAsync("DGAs").Result;

            var usersA = _userManager.GetUsersInRoleAsync("Agent").Result;
            var users = usersA.Concat(usersD).ToList();

            return View(users);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public class TlsAttribute : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        var request = filterContext.HttpContext.Request;
        //        if (request.IsHttps)
        //        {
        //            filterContext.HttpContext.Response.Headers("Strict-Transport-Security", "max-age=15552000");
        //        }
        //        else if (!request..IsLocal && request.Headers["Upgrade-Insecure-Requests"] == "1")
        //        {
        //            var url = new Uri("https://www." + request.Url.GetComponents(UriComponents.Host | UriComponents.PathAndQuery, UriFormat.Unescaped), UriKind.Absolute);
        //            filterContext.Result = new RedirectResult(url.AbsoluteUri);
        //        }
        //    }
        //}

        public async Task<ActionResult> Sendsms()
        {
            var user = User.Identity.Name;

            if (user != null)
            {
                var userinfo = await _userManager.FindByNameAsync(user);

                await SendMessage(userinfo.PhoneNumber + "New User wimbig", "08165680904,08168885804", MessageChannel.SMS, MessageType.Activation);

                userinfo.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(userinfo);

            }
            return RedirectToAction("Index", "Home");

        }
        private async Task SendMessage(string message, string address,
        MessageChannel messageChannel, MessageType messageType)
        {

            try
            {
                var messageStore = new MessageStore()
                {
                    MessageChannel = messageChannel,
                    MessageType = messageType,
                    Message = message,
                    AddressType = AddressType.Single
                };

                if (messageChannel == MessageChannel.Email)
                {
                    messageStore.EmailAddress = address;
                }
                else if (messageChannel == MessageChannel.SMS)
                {
                    messageStore.PhoneNumber = address;
                }
                else
                {


                }

                await _messageStoreRepository.Add(messageStore);
            }
            catch (Exception c) { }
        }
        public List<SelectListItem> AreaDtoListDp { get; set; }
        public async Task<IActionResult> AreaCityList(long id)
        {
            List<AreaInCityDto> city = new List<AreaInCityDto>();

            var query = await _cityAppService.GetAreaInCityByCityIdAsync(cityId: id);

            city.AddRange(query.Source.Select(entity => new AreaInCityDto()
            {
                Id = entity.Id,
                Name = entity.Name


            }));

            AreaDtoListDp = city.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Name
                                }).ToList();
            return new JsonResult(AreaDtoListDp);
        }

        //public ActionResult Fire()
        //{
        //    RecurringJob.AddOrUpdate(recurringJobId:"Sms-Job", methodCall:() => _smsService.RunDemo(), Cron.Minutely);

        //    return View();
        //}
    }
}
