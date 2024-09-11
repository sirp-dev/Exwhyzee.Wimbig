using Exwhyzee.Core.Mvc.ValidationAttributes;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Cities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IWalletAppService _walletAppService;

        private readonly ICityAppService _cityAppService;


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            ICityAppService cityAppService,
            IWalletAppService walletAppService,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
                        RoleManager<ApplicationRole> roleManager,
            IMessageStoreRepository messageStoreRepository)
        {
            _userManager = userManager;
            _cityAppService = cityAppService;
            _walletAppService = walletAppService;
            _signInManager = signInManager;
            _roleManager = roleManager;

            _logger = logger;
            _messageStoreRepository = messageStoreRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public WalletDto Wallet { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Phone"), RegularExpression(@"^[0]\d{10}$", ErrorMessage = "Error! Invalid Phone Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Other Names")]
            public string OtherNames { get; set; }

            public string CodeVerify { get; set; }

            [Required]
            [Display(Name = "Area In Current City")]
            public string AreaInCurrentCity { get; set; }

            public string RoleString { get; set; }


            [Required]
            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            [EnsureMinimumAge(18, ErrorMessage = "Sorry, You are not Eligible to Play Small and Winbig... Date of Birth must be 18years +")]
            public DateTime DateOfBirth { get; set; }

            public long UniqueId { get; set; }

            [Required]
            [Display(Name = "Current City")]
            public string CurrentCity { get; set; }
            
        }

        [BindProperty]
        public List<SelectListItem> CityDtoList { get; set; }

        public async Task OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
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
            returnUrl = returnUrl ?? Url.Content("~/");
            var check = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
            if (ModelState.IsValid)
            {
                //
                if(Input.CurrentCity == null || Input.AreaInCurrentCity == null)
                {
                    TempData["error"] = "City and Area in City required";
                    return Page();
                }
                long cityId = Convert.ToInt64(Input.CurrentCity);
                long AreacityId = Convert.ToInt64(Input.AreaInCurrentCity);
                var cityData = await _cityAppService.Get(cityId);
                var AreacityData = await _cityAppService.GetAreaInCity(AreacityId);

                var unqiueId = check.OrderByDescending(x => x.UniqueId).FirstOrDefault();
                var verificationCode = Exwhyzee.Core.Helpers.CommonHelper.GenerateRandomInteger(8);
                int n = Convert.ToInt32(unqiueId.UniqueId) + 1;
                string number = n.ToString("000000");
                var user = new ApplicationUser
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    OtherNames = Input.OtherNames,
                    DateOfBirth = Input.DateOfBirth,
                    PhoneNumber = Input.PhoneNumber,
                    CodeVerify = verificationCode.ToString(),
                    UniqueId = number,
                    CurrentCity = cityData.Name,
                    AreaInCurrentCity = AreacityData.Name,


                };
                //

                if (check.Select(x => x.Email).Contains(Input.Email))
                {
                    TempData["error"] = "Email already taken";
                   
                    return Page();
                }

                if (check.Select(x => x.PhoneNumber).Contains(Input.PhoneNumber))
                {
                    TempData["error"] = "phone number already taken";
                    
                    return Page();
                }

                //age

                try
                {


                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        var Role = await _roleManager.FindByNameAsync("User");
                        if (Role != null)
                        {
                            await _userManager.AddToRoleAsync(user, Role.Name);

                        }
                        Wallet = await _walletAppService.GetWallet(user.Id);
                        _logger.LogInformation("User created a new account with password.");

                        //update rolestring
                        string rolename = "";
                        var userRole = await _userManager.FindByIdAsync(user.Id);
                        var roleRole = await _userManager.GetRolesAsync(userRole);
                        if (roleRole != null)
                        {
                            rolename = string.Join("; ", roleRole);
                        }
                        userRole.RoleString = rolename;
                        await _userManager.UpdateAsync(userRole);

                        var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.Page(
                        //    "/Account/ConfirmEmail",
                        //    pageHandler: null,
                        //    values: new { userId = user.Id, code = emailCode },
                        //    protocol: Request.Scheme);

                        //var emailMessageBody = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                        var emailMessageBody = $"Please confirm your number is " + verificationCode.ToString();

                        var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Successful", "Your registration was successful", "Welcome " + user.FullName, ".");
                        var smsMessage = string.Format("{0}, Your registration was successful. Play Small WinBig @ https://www.wimbig.com/", user.FullName);
                        _logger.LogInformation("Pusing Email To Store");
                        await SendMessage(emailMessage, user.Email, MessageChannel.Email, MessageType.Activation);

                        var validePhoneNumber = Exwhyzee.Core.Helpers.CommonHelper.IsValidPhoneNumber(user.PhoneNumber);
                        if (validePhoneNumber)
                        {
                            _logger.LogInformation("Pusing SMS To Store");
                            await SendMessage(smsMessage, user.PhoneNumber, MessageChannel.SMS, MessageType.Activation);
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);

                        // return RedirectToPage("./Manage/Verify", new { id = user.Id });
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception e)
                {
                    TempData["error"] = e;
                    

                }
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

        private async Task SendMessage(string message, string address,
            MessageChannel messageChannel, MessageType messageType)
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
                _logger.LogError("MessageChanel Not found");
                return;
            }

            await _messageStoreRepository.Add(messageStore);
        }

        [BindProperty]
        public List<CityDto> CityDtoListDp { get; set; }

        [BindProperty]
        public List<SelectListItem> AreaDtoListDp { get; set; }
        //public async JsonResult OnGetList(long id) 
        //{
        //    var cities = await _cityAppService.GetAreaInCityByCityIdAsync(cityId: id);


        //    return JsonResult(new SelectList(cities.Source.ToArray(), "Name", "Name"));
        //}

      
    }
}
