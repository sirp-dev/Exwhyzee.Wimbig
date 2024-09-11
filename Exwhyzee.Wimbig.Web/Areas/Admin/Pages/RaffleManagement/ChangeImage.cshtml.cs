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
    public class ChangeImageModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IMapRaffleToCategoryAppService _mapRaffleToCategory;
        private readonly IImageFileAppService _imgFileAppSevice;
        private readonly IMapImageToRaffleAppService _mapImageToRaffleService;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IMessageStoreRepository _messageStoreRepository;

        public ChangeImageModel(IHostingEnvironment env, IRaffleAppService raffleAppService, IMapImageToRaffleAppService mapImageToRaffleAppService, IImageFileAppService imageFileAppService,
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
        public CreateRaffleVM CreateRaffleVm { get; set; }

        public class CreateRaffleVM
        {
            public CreateRaffleDto CreateRaffleDto { get; set; }
            public List<ImageFile> RaffleImages { get; set; }
        }

        public string LoggedInUser { get; set; }

        [BindProperty]
        public long RaffleIdNumber { get; set; }

        [BindProperty]
        public long ImageId { get; set; }


        public List<CategorySectionDetailsDto> Catgeories { get; set; }

        public List<ImageFile> Images { get; set; }
        public List<ApplicationUser> Users { get; set; }


        public List<MapImageToRaffle> RaffleImages { get; set; }

        public async Task OnGet(long id)
        {

            LoggedInUser = _userManager.GetUserId(HttpContext.User);
            RaffleIdNumber = id;


        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var raffle = await _raffleAppService.GetById(id);
                    long imgId = Convert.ToInt64(ImageId);

                    if (imgId > 0)
                    {
                        var mapImageToRaffle = new MapImageToRaffle()
                        {
                            ImageId = imgId,
                            RaffleId = raffle.Id,
                            DateCreated = DateTime.UtcNow.AddHours(1),
                        };

                        await _mapImageToRaffleService.InsertMap(mapImageToRaffle);
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