

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Cities.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]

    public partial class UpdateProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ICityAppService _cityAppService;


        public UpdateProfileModel(
            UserManager<ApplicationUser> userManager,
            ICityAppService cityAppService,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cityAppService = cityAppService;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public CityDto cityData { get; set; }
        public AreaInCityDto AreacityData { get; set; }

        [TempData]
        public string ACity { get; set; }

        [TempData]
        public string City { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Other Names")]
            public string OtherNames { get; set; }

            [Required]
            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Current City")]
            public string CurrentCity { get; set; }

            [Display(Name = "Contact Address")]
            public string ContactAddress { get; set; }

            [Display(Name = "Area In Current City")]
            public string AreaInCurrentCity { get; set; }


        }

        [BindProperty]
        public List<SelectListItem> CityDtoList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                LastName = user.LastName,
                FirstName = user.FirstName,
                DateOfBirth = user.DateOfBirth,
                OtherNames = user.OtherNames,
                CurrentCity = user.CurrentCity,
                AreaInCurrentCity = user.AreaInCurrentCity,
                ContactAddress = user.ContactAddress
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            //
           
                City = user.CurrentCity;
                ACity = user.AreaInCurrentCity;
            

            if (string.IsNullOrEmpty(user.ContactAddress))
            {
                TempData["no conatctaddress"] = "no contact";

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
 List<CityDto> city = new List<CityDto>();
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            long cityId = 0;
            long AreacityId = 0;
           

            var query = await _cityAppService.GetAsync();

            city.AddRange(query.Source.Select(entity => new CityDto()
            {
                Id = entity.Id,
                Name = entity.Name
            }));
           
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            //
            if (Input.CurrentCity == null || City == null)
            {
                TempData["error"] = "City and Area in City required";
                CityDtoList = city.Select(a =>
                               new SelectListItem
                               {
                                   Value = a.Id.ToString(),
                                   Text = a.Name
                               }).ToList();
                return Page();
            }
            try
            {
                 cityId = Convert.ToInt64(Input.CurrentCity);
                 AreacityId = Convert.ToInt64(Input.AreaInCurrentCity);
                 cityData = await _cityAppService.Get(cityId);
                 AreacityData = await _cityAppService.GetAreaInCity(AreacityId);
            }catch(Exception c) { }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.DateOfBirth != user.DateOfBirth)
            {
                user.DateOfBirth = Input.DateOfBirth;
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }
            if (City != null)
            {
                user.CurrentCity = City;
                user.AreaInCurrentCity = ACity;
               
            }
            else
            {
   user.CurrentCity = cityData.Name;
            user.AreaInCurrentCity = AreacityData.Name;
            }
         
            user.ContactAddress = Input.ContactAddress;
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            CityDtoList = city.Select(a =>
                              new SelectListItem
                              {
                                  Value = a.Id.ToString(),
                                  Text = a.Name
                              }).ToList();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";

            
            CityDtoList = city.Select(a =>
                                new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Name
                                }).ToList();
            return RedirectToPage();
        }
    }
}