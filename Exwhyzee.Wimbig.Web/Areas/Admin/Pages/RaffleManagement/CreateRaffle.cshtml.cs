using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Cities.Dto;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "Admin,mSuperAdmin")]
    public class CreateRaffleModel : PageModel
    {
        private readonly IRaffleAppService _raffleAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IMapRaffleToCategoryAppService _mapRaffleToCategory;
        private readonly IImageFileAppService _imgFileAppSevice;
        private readonly IMapImageToRaffleAppService _mapImageToRaffleService;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly ICityAppService _cityAppService;

        public CreateRaffleModel(IHostingEnvironment env, ICityAppService cityAppService, IRaffleAppService raffleAppService,IMapImageToRaffleAppService mapImageToRaffleAppService, IImageFileAppService imageFileAppService,
            IMapRaffleToCategoryAppService mapRaffleToCategory, IMessageStoreRepository messageStoreRepository, UserManager<ApplicationUser> userManger,ICategoryAppService categoryAppService)
        {
            _cityAppService = cityAppService;
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

        public string ReturnUrl { get; set; }

        public List<CategorySectionDetailsDto> Catgeories { get; set; }

        public List<ImageFile> Images { get; set; }
        public List<ApplicationUser> Users { get; set; }

        [BindProperty]
        public string CityInfo { get; set; }

        [BindProperty]
        public string AreaInCity { get; set; }

        [BindProperty]
        public List<SelectListItem> CityDtoList { get; set; }

        public List<MapImageToRaffle> RaffleImages { get; set; }

        public async Task OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            
            LoggedInUser =  _userManager.GetUserId(HttpContext.User);

            var categories =  await _categoryAppService.GetAsync();
            Catgeories = categories.Source.ToList();
            //get dgas and agents
            var usersA = _userManager.GetUsersInRoleAsync("Agent").Result;
            
            var usersD = _userManager.GetUsersInRoleAsync("DGAs").Result;
            
            var userlist = usersA.Concat(usersD);

            Users = userlist.ToList();

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

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");
                CreateRaffleVm.CreateRaffleDto.Status = Enums.EntityStatus.Active;
                if (ModelState.IsValid)
                {
                   long cityId = Convert.ToInt64(CreateRaffleVm.CreateRaffleDto.Location);
                   long areacityId = Convert.ToInt64(CreateRaffleVm.CreateRaffleDto.AreaInCity);
                    var cityData = await _cityAppService.Get(cityId);
                   var AreacityData = await _cityAppService.GetAreaInCity(areacityId);
                    var raffle = new RaffleDto
                    {
                        DeliveryType = CreateRaffleVm.CreateRaffleDto.DeliveryType,
                        Description = CreateRaffleVm.CreateRaffleDto.Description,
                        EndDate = CreateRaffleVm.CreateRaffleDto.EndDate,
                        HostedBy = CreateRaffleVm.CreateRaffleDto.HostedBy,
                        Name = CreateRaffleVm.CreateRaffleDto.Name,
                        NumberOfTickets = CreateRaffleVm.CreateRaffleDto.NumberOfTickets,
                        PricePerTicket = CreateRaffleVm.CreateRaffleDto.PricePerTicket,
                        StartDate = CreateRaffleVm.CreateRaffleDto.StartDate,
                        DateCreated = CreateRaffleVm.CreateRaffleDto.DateCreated,
                        Status = CreateRaffleVm.CreateRaffleDto.Status,
                        SortOrder = CreateRaffleVm.CreateRaffleDto.SortOrder,
                        Location = cityData.Name,
                        AreaInCity = AreacityData.Name

                    };

                    var raffleId = await _raffleAppService.Add(raffle);  

                    var mapRaffleToCategory = new MapRaffleToCategoryDto
                    {
                        CategoryId = CreateRaffleVm.CreateRaffleDto.CategoryId.Value,
                        CategoryName = await GetCategoryName(CreateRaffleVm.CreateRaffleDto.CategoryId.Value),
                        RaffleId = raffleId,
                        RaffleName = raffle.Name,
                    };
                    var mapId = await _mapRaffleToCategory.Add(mapRaffleToCategory);
                    #region Raffle Image(s)
                    int imgCount = 0;
                    if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count > 0)
                    {
                        var newFileName = string.Empty;
                        var filePath = string.Empty;
                        string pathdb = string.Empty;
                        var files = HttpContext.Request.Form.Files;
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                filePath = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                imgCount++;
                                var now = DateTime.Now;
                                var uniqueFileName = $"{now.Year}{now.Month}{now.Day}_{now.Hour}{now.Minute}{now.Second}{now.Millisecond}".Trim();

                                var fileExtension = Path.GetExtension(filePath);

                                newFileName = uniqueFileName + fileExtension;

                                // if you wish to save file path to db use this filepath variable + newFileName
                                var fileDbPathName = $"/RaffleImages/".Trim();

                                filePath = $"{_hostingEnv.WebRootPath}{fileDbPathName}".Trim();

                                if (!(Directory.Exists(filePath)))
                                    Directory.CreateDirectory(filePath);

                                var fileName = "";
                               fileName = filePath + $"/{newFileName}".Trim();
                            

                                // copy the file to the desired location from the tempMemoryLocation of IFile and flush temp memory
                                using (FileStream fs = System.IO.File.Create(fileName))
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                }

                                #region Save Image Propertie to Db
                                var img = new ImageFile()
                                {
                                    Url = $"{fileDbPathName}/{newFileName}",
                                    Extension = fileExtension,
                                    DateCreated = DateTime.UtcNow.AddHours(1),
                                    Status = EntityStatus.Active,
                                    IsDefault = imgCount == 1 ? true : false,
                                };
                                var saveImageToDb = await _imgFileAppSevice.Insert(img);
                                if (saveImageToDb > 0)
                                {
                                    var mapImageToRaffle = new MapImageToRaffle()
                                    {
                                        ImageId = saveImageToDb,
                                        RaffleId = raffleId,
                                        DateCreated = DateTime.UtcNow.AddHours(1),
                                    };

                                    await _mapImageToRaffleService.InsertMap(mapImageToRaffle);
                                }
                                #endregion

                                if (imgCount >= 5)
                                    break;
                            }
                        }
                    }
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
                    catch(Exception e)
                    {
                       
                    }
                    string imageurl = url + "/" + imge;
                    
                   
                    //var users = _userManager.Users.ToList();
                    List<string> mylist = new List<string>(new string[] { "onwukaemeka41@gmail.com", "judengama@gmail.com" });
                    var users = mylist;
                    foreach(var newemail in users)
                    {
                        string emailMessageBody = "A new Raffle is available for winning Your Raffle Name " + raffledetails.Name + " and Id: "+ raffledetails.Id+ " click to play now: https://www.wimbig.com/Raffles/Raffles/Details/" + raffledetails.Id;
                        string emailMessage = string.Format("{0};??{1};??{2};??{3}", "Raffle Notification", "Play Notification", "" + newemail, emailMessageBody);

                        await SendMessage(emailMessage, newemail, MessageChannel.Email, MessageType.Activation, imageurl);

                    }
                    return RedirectToPage("Index");
                }

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
                return category!= null ? category.Name : "Not Set";
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