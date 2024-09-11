using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Exwhyzee.Core.Mvc.ValidationAttributes;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "mSuperAdmin,Agent,DGAs,Supervisors")]

    public class WimRegistrationModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWalletAppService _walletAppService;


        public WimRegistrationModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IWalletAppService walletAppService,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _walletAppService = walletAppService;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public WalletDto Wallet { get; set; }
        public List<ApplicationRole> Roles { get; set; }
        public List<ApplicationUser> UserList { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "User Role")]
            public string RoleId { get; set; }

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
            [Display(Name = "Phone")]
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }

            [Display(Name = "Other Names")]
            public string OtherNames { get; set; }

            [Display(Name = "Description")]
            public string UserDescription { get; set; }

            [Display(Name = "Reference")]
            public string ReferenceId { get; set; }

            [Required]
            [Display(Name = "Current City")]
            public string CurrentCity { get; set; }


            [Required]
            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            [EnsureMinimumAge(18, ErrorMessage = "Sorry, You are not Eligible to Play Small and Winbig... Date of Birth must be 18years +")]
            public DateTime DateOfBirth { get; set; }

            public long UniqueId { get; set; }



        }

        public async Task OnGet(string returnUrl = null)
        {
            List<ApplicationRole> roles = new List<ApplicationRole>();

            roles = _roleManager.Roles.ToList();
            var usersa = await _userManager.GetUsersInRoleAsync("Supervisors");
            var usersb = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            UserList = usersa.Concat(usersb).ToList();
            if (User.IsInRole("SuperAdmin") || User.IsInRole("mSuperAdmin") || User.IsInRole("Admin"))
            {
                Roles = roles.Where(x=>x.Name != "mSuperAdmin").ToList();
            }
            else
            {
                Roles = roles.Where(x => x.Name == "User").ToList();

            }

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            var check = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!User.IsInRole("SuperAdmin") || User.IsInRole("mSuperAdmin"))
            {
                Input.ReferenceId = userid;
            }

            ///
            List<ApplicationRole> roles = new List<ApplicationRole>();

            roles = _roleManager.Roles.ToList();
            var usersa = await _userManager.GetUsersInRoleAsync("Supervisors");
            var usersb = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            UserList = usersa.Concat(usersb).ToList();

            var unqiueId = check.OrderByDescending(x => x.UniqueId).FirstOrDefault();
            int n = Convert.ToInt32(unqiueId.UniqueId) + 1;
            string number = n.ToString("000000");
            if (ModelState.IsValid)
            {
                try
                {

                    var user = new ApplicationUser
                    {
                        UserName = Input.UserName,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        OtherNames = Input.OtherNames,
                        DateOfBirth = Input.DateOfBirth,
                        PhoneNumber = Input.PhoneNumber,
                        ReferenceId = Input.ReferenceId,
                        UserDescription = Input.UserDescription,
                        UniqueId = number,
                        CurrentCity = Input.CurrentCity
                    };

                    if (check.Select(x => x.Email).Contains(Input.Email))
                    {
                        TempData["error"] = "Email already taken";
                        if (User.IsInRole("SuperAdmin") || User.IsInRole("mSuperAdmin") || User.IsInRole("Admin"))
                        {
                            Roles = roles;
                        }
                        else
                        {
                            Roles = roles.Where(x => x.Name == "User").ToList();

                        }
                        ReturnUrl = returnUrl;
                        return Page();
                    }

                    if (check.Select(x => x.PhoneNumber).Contains(Input.PhoneNumber))
                    {
                        TempData["error"] = "phone number already taken";
                        if (User.IsInRole("SuperAdmin") || User.IsInRole("mSuperAdmin") || User.IsInRole("Admin"))
                        {
                            Roles = roles;
                        }
                        else
                        {
                            Roles = roles.Where(x => x.Name == "User").ToList();

                        }
                        ReturnUrl = returnUrl;
                        return Page();
                    }


                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Input.RoleId);
                        Wallet = await _walletAppService.GetWallet(user.Id);
                        _logger.LogInformation("User created a new account with password.");

                        if (User.IsInRole("SuperAdmin") || User.IsInRole("mSuperAdmin"))
                        {
                            return Redirect("/UserManagement/Users/Index");
                        }
                        else
                        {
                            return Redirect("/StakeHolders/UserData/ReferencedUser");
                        }

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception e)
                {

                    TempData["error"] = "";
                }

            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("mSuperAdmin") || User.IsInRole("Admin"))
            {
                Roles = roles;
            }
            else
            {
                Roles = roles.Where(x => x.Name == "User").ToList();

            }
            ReturnUrl = returnUrl;

            return Page();
        }
    }
}