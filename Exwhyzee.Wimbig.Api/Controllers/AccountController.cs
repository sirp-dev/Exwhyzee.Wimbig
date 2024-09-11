using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Api.Controllers.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Exwhyzee.Wimbig.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IWalletAppService _walletAppService;
        private IConfiguration _config;

        public AccountController(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            ILogger<AccountController> logger, IConfiguration config,
             RoleManager<ApplicationRole> roleManager,
            IMessageStoreRepository messageStoreRepository,
            IWalletAppService walletAppService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _config = config;
            _walletAppService = walletAppService;
            _roleManager = roleManager;
            _messageStoreRepository = messageStoreRepository;
        }

        #region Loging
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Token([FromBody]LoginDto login)
        {
            var user = await Authenticate(login);

            if(user == null)
            {
                return BadRequest("Authentication Failed");
            }

            var token = BuildToken(user);

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            return Ok(token);
        }

        private string BuildToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ApplicationUser> Authenticate(LoginDto login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if(user == null)
            {
                return null;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (result.Succeeded)
            {
                return user;
            }

            return null;
        }

        #endregion

        #region Register
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            
            var check = _userManager.Users.Where(x => x.UserName != "mJinmcever").ToList();
            if (ModelState.IsValid)
            {
                var unqiueId = check.OrderByDescending(x => x.UniqueId).FirstOrDefault();
                var verificationCode = Exwhyzee.Core.Helpers.CommonHelper.GenerateRandomInteger(8);
                int n = Convert.ToInt32(unqiueId.UniqueId) + 1;
                string number = n.ToString("000000");
                var user = new ApplicationUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    OtherNames = registerDto.OtherNames,
                    DateOfBirth = registerDto.DateOfBirth,
                    PhoneNumber = registerDto.PhoneNumber,
                    CodeVerify = verificationCode.ToString(),
                    UniqueId = number,
                    CurrentCity = registerDto.CurrentCity

                };
                //

                if (check.Select(x => x.Email).Contains(registerDto.Email))
                {
                     return BadRequest("Email already taken.");
                }

                if (check.Select(x => x.PhoneNumber).Contains(registerDto.PhoneNumber))
                {
                    return BadRequest("phone number already taken");
                }

                //age

                try
                {


                    var result = await _userManager.CreateAsync(user, registerDto.Password);
                    if (result.Succeeded)
                    {
                        var Role = await _roleManager.FindByNameAsync("User");
                        if (Role != null)
                        {
                            await _userManager.AddToRoleAsync(user, Role.Name);

                        }
                        var wallet = await _walletAppService.GetWallet(user.Id);
                        _logger.LogInformation("User created a new account with password.");

                        var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

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

                       
                        return Ok(registerDto);

                        // return RedirectToPage("./Manage/Verify", new { id = user.Id });
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest("Account setup failed.");
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
        #endregion
    }
}