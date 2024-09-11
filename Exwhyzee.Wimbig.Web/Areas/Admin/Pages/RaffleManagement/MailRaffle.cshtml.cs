using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.Count.Dtos;
using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class MailRaffleModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        private readonly IMapImageToRaffleAppService _mapImageToRaffleAppService;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly ICountAppService _countAppService;

        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public string LoggedInUser { get; set; }

        public MailRaffleModel(IRaffleAppService raffleAppService, ICountAppService countAppService, IMapImageToRaffleAppService mapImageToRaffleAppService,
            UserManager<ApplicationUser> userManger, IMessageStoreRepository messageStoreRepository, IHostingEnvironment hostingEnv)
        {
            _raffleAppService = raffleAppService;
            _userManager = userManger;
            _messageStoreRepository = messageStoreRepository;
            _mapImageToRaffleAppService = mapImageToRaffleAppService;
            _hostingEnv = hostingEnv;
            _countAppService = countAppService;
        }

        public PagedList<UsersInRoleDto> UsersInRole { get; set; }

        [BindProperty]
        public RaffleDto Raffle { get; set; }

    

        public async Task<IActionResult> OnGetAsync(long id)
        {
            try
            {
                var raffle = await _raffleAppService.GetById(id);

                if (raffle == null)
                {
                    return NotFound($"Unable to load raffle with the ID '{id}'.");
                }
                var item = await GetItemAsync(raffle.Id);
                string imageurl = "https://www.wimbig.com/" + item.Url;
                UsersInRole = await _countAppService.UsersInRole();
                if (UsersInRole == null)
                {
                    return NotFound($"Unable to load users.");
                }
                //var users = _userManager.Users.ToList();
                // List<string> mylist = new List<string>(new string[] { "onwukaemeka41@gmail.com", "judengama@gmail.com" });

                foreach (var newemail in UsersInRole.Source.ToList())
                {
                    string emailMessageBody = "Do you know, with </br> " + raffle.PricePerTicket + " this item can be yours.<br> Id Number: " + raffle.Id + "<br> Description: <br> "+ raffle.Description +"<br> click to play now: https://www.wimbig.com/Raffles/Raffles/Details/" + raffle.Id;
                    string emailMessage = string.Format("{0};??{1};??{2};??{3}", "New Raffle Notification", raffle.Name, "" + "", emailMessageBody);

                    await SendMessage(emailMessage, newemail.Email, MessageChannel.Email, MessageType.Activation, imageurl);

                }
                TempData["msg"] = "Sent " + UsersInRole.Source.Count() + " emails";
                return Page();
            }catch(Exception c)
            {
                TempData["error"] = c;
                return Page();
            }
        }

        private const string MainFolder = "main";
        private const string ImageFolder = "wimbig";
        private async Task<ImageOfARaffle> GetItemAsync(long raffleId)
        {
            var images = await _mapImageToRaffleAppService.GetAllImagesOfARaffle(raffleId, 1);

            if (images.Count() < 1)
            {
                var path = Path.Combine(_hostingEnv.WebRootPath, MainFolder, ImageFolder);
                return new ImageOfARaffle
                {
                    DateCreated = DateTime.Now,
                    Extension = ".png",
                    Id = raffleId,
                    ImageId = 0,
                    RaffleId = raffleId,
                    Url = $"/main/wimbig/wimbig_drum_2.png"
                };
                //return null;
            }
            return images.OrderBy(x => x.Id).FirstOrDefault();
        }
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