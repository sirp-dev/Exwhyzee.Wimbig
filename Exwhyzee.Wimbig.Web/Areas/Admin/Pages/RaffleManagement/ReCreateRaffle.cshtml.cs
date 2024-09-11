using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Application.MapRaffleToCategorys;
using Exwhyzee.Wimbig.Application.MapRaffleToCategorys.Dtos;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "Admin,mSuperAdmin")]
    public class ReCreateRaffleModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IMapRaffleToCategoryAppService _mapRaffleToCategory;
        private readonly IImageFileAppService _imgFileAppSevice;
        private readonly IMapImageToRaffleAppService _mapImageToRaffleService;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IMessageStoreRepository _messageStoreRepository;

        public ReCreateRaffleModel(IHostingEnvironment env, IRaffleAppService raffleAppService, IMapImageToRaffleAppService mapImageToRaffleAppService, IImageFileAppService imageFileAppService,
            IMapRaffleToCategoryAppService mapRaffleToCategory, IMessageStoreRepository messageStoreRepository, UserManager<ApplicationUser> userManger, ICategoryAppService categoryAppService)
        {
            _hostingEnv = env;
            _raffleAppService = raffleAppService;
            _categoryAppService = categoryAppService;
            _mapRaffleToCategory = mapRaffleToCategory;
            _userManager = userManger;
            _messageStoreRepository = messageStoreRepository;
            _mapImageToRaffleService = mapImageToRaffleAppService;
            _imgFileAppSevice = imageFileAppService;
        }


        public CreateRaffleDto CreateRaffleDto { get; set; }

        [BindProperty]
        public long ImgId { get; set; }

        [BindProperty]
        public long RaffleId { get; set; }


        public string LoggedInUser { get; set; }
        public long FetchRaffId { get; set; }

        public string ReturnUrl { get; set; }

        public List<CategorySectionDetailsDto> Catgeories { get; set; }

        public List<ImageFile> Images { get; set; }
        public List<ApplicationUser> Users { get; set; }


        public List<MapImageToRaffle> RaffleImages { get; set; }

        public async Task OnGet(long Id, string returnUrl = null)
        {
            FetchRaffId = Id;

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                var raffleItem = await _raffleAppService.GetById(RaffleId);

                returnUrl = returnUrl ?? Url.Content("~/");
                raffleItem.Status = Enums.EntityStatus.Active;
                if (ModelState.IsValid)
                {
                    var raffle = new RaffleDto
                    {
                        DeliveryType = raffleItem.DeliveryType,
                        Description = raffleItem.Description,
                        EndDate = DateTime.UtcNow.AddDays(7),
                        HostedBy = raffleItem.HostedBy,
                        Name = raffleItem.Name,
                        NumberOfTickets = raffleItem.NumberOfTickets,
                        PricePerTicket = raffleItem.PricePerTicket,
                        StartDate = DateTime.UtcNow,
                        DateCreated = DateTime.UtcNow,
                        Status = EntityStatus.Active,
                        SortOrder = 1,
                        Location = raffleItem.Location,
                        AreaInCity = raffleItem.AreaInCity

                    };

                    var raffleId = await _raffleAppService.Add(raffle);
                    //
                    var mapCategory = await _mapRaffleToCategory.GetByRaffleId(raffleItem.Id);

                    var mapRaffleToCategory = new MapRaffleToCategoryDto
                    {
                        CategoryId = mapCategory.CategoryId,
                        CategoryName = await GetCategoryName(mapCategory.CategoryId),
                        RaffleId = raffleId,
                        RaffleName = raffleItem.Name,
                    };
                    var mapId = await _mapRaffleToCategory.Add(mapRaffleToCategory);
                    #region Raffle Image(s)

                        var mapImageToRaffle = new MapImageToRaffle()
                        {
                            ImageId = ImgId,
                            RaffleId = raffleId,
                            DateCreated = DateTime.UtcNow.AddHours(1),
                        };

                        await _mapImageToRaffleService.InsertMap(mapImageToRaffle);

                    
                    #endregion
                    string url = HttpContext.Request.Host.ToString();
                    string imge = "";
                    var raffledetails = await _raffleAppService.GetById(raffleId);
                    //
                    try
                    {
                        var images = await _mapImageToRaffleService.GetAllImagesOfARaffle(raffleId, 1);

                        imge = images.FirstOrDefault().Url;
                    }
                    catch (Exception e)
                    {

                    }
                    string imageurl = url + "/" + imge;


                    //var users = _userManager.Users.ToList();
                    List<string> mylist = new List<string>(new string[] { "onwukaemeka41@gmail.com", "judengama@gmail.com" });
                    var users = mylist;
                    foreach (var newemail in users)
                    {
                        string emailMessageBody = "A new Raffle is available for winning Your Raffle Name " + raffledetails.Name + " and Id: " + raffledetails.Id + "click to play now: https://www.wimbig.com/Raffles/Raffles/Details/" + raffledetails.Id;
                        string emailMessage = string.Format("{0};??{1};??{2};??{3}", "Raffle Notification", "Play Notification", "" + newemail, emailMessageBody);

                        await SendMessage(emailMessage, newemail, MessageChannel.Email, MessageType.Activation, imageurl);

                    }
                    return RedirectToPage("Index");
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Helper Agents
        private static string CurrentYear()
        {
            var currentYear = DateTime.Now;
            return currentYear.Year.ToString();
        }

        private async Task<string> GetCategoryName(long id)
        {
            try
            {
                var category = await _categoryAppService.Get(id);
                return category != null ? category.Name : "Not Set";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private async Task SendMessage(string message, string address,
       MessageChannel messageChannel, MessageType messageType, string imageurl)
        {
            try
            {


                var messageStore = new MessageStore()
                {
                    MessageChannel = messageChannel,
                    MessageType = messageType,
                    Message = message,
                    AddressType = AddressType.Single,
                    ImageUrl = imageurl
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
            catch (Exception e)
            {

            }
        }
    }
}