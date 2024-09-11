using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Services.Users
{
    public class UserAppService : IUserAppService
    {
        #region Field
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly ILogger _logger;
        #endregion

        #region Ctor
        public UserAppService(WimbigDbContext wimbigDbContext, IMessageStoreRepository messageStoreRepository, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<UserAppService> logger, RoleManager<ApplicationRole> roleManager)
        {
            _messageStoreRepository = messageStoreRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        #endregion

        #region Methods

        #region Authorization and Authentication
        /// <summary>
        /// Authenticate a user against his provided password without siging the user.
        /// If the user credentials are valid then the user object is returned else it returns null 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passsword"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> Authenticate(string userId, string passsword)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var user = _userManager.Users.FirstOrDefault(x => x.Id.Equals(userId));

            if (user == null)
                return null;

            // validate the user againt provided password 

            var validate = await _signInManager.CheckPasswordSignInAsync(user, passsword, false);
            if (validate.Succeeded)
            {
                return new ApplicationUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                };
            }
            // if it got here, authentication failed
            return null;
        }

        /// <summary>
        /// sign in a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        public async Task<SignInResult>SignIn(ApplicationUser user, string password,bool rememberMe)
        {
            try
            {               
                 var signIn = await _signInManager.PasswordSignInAsync(user.UserName, password,rememberMe , lockoutOnFailure: true);
                return signIn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// sign out a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool>SignOut(ApplicationUser user, string password)
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        #endregion

        #region User Managemenent
        /// <summary>
        /// Return the Id of the created user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> Create(ApplicationUser user, string password)
        {
            // validate 
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "value cannot be null or empty");

            if (_userManager.Users.Any(x => x.Email.Equals(user.Email)))
                throw new ArgumentException(nameof(user.Email), "Email is already registered");

            var result = await _userManager.CreateAsync(user,password);
            if (result.Succeeded)
            {              
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var smsCode = Exwhyzee.Core.Helpers.CommonHelper.GenerateRandomInteger(8);
                var callbackUrl = $"/Account/ConfirmEmail/{new { userId = user.Id, code = emailCode }}";
                //                      Url.Page(
                //                    "/Account/ConfirmEmail",
                //                    pageHandler: null,
                //                    values: new { userId = user.Id, code = emailCode },
                //                    protocol: Request.Scheme);

                var emailMessageBody = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

                var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Winbig Email Confirmation", "Confirm your email", "Thanks " + user.FullName, emailMessageBody);
                var smsMessage = string.Format("{0}, \n Your Confirmation Code: {1}", user.FullName, smsCode);
              
                await SendMessage(emailMessage, user.Email, MessageChannel.Email, MessageType.Activation);
                var phoneNumber = Exwhyzee.Core.Helpers.CommonHelper.FormatPhoneNumber(user.PhoneNumber, true);
                //_logger.LogInformation("Composing SMS");
                await SendMessage(smsMessage, phoneNumber, MessageChannel.SMS, MessageType.Activation);
                var userCreated = await _userManager.FindByEmailAsync(user.Email);

                await _signInManager.SignInAsync(user, isPersistent: false);

                return userCreated.Id;
            }
            return null;

        }

        /// <summary>
        /// Delete a user and return boolean result
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> Delete(ApplicationUser user)
        {
            try
            {
               var removeUser = await _userManager.DeleteAsync(user);
                if (removeUser.Succeeded)
                    return true;
            
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            try
            {
                var users =  _userManager.Users.Where(x => x.UserName != "mJinmcever");
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Update a user profile
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Update(ApplicationUser user, string password)
        {
            try
            {
                var updateUser = await _userManager.UpdateAsync(user);
                if (updateUser.Succeeded)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        #endregion

        #region Role Mnagement
        /// <summary>
        /// Create a new role and returns  boolean success or failure result
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> CreateRole(ApplicationRole role)
        {
            try
            {
                var createdRole = await _roleManager.CreateAsync(role);
                if (createdRole.Succeeded)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Assign a user to a role 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> CreateUserRole(ApplicationUser user, string roleName)
        {
            try
            {
                var createdUserRole = await _userManager.AddToRoleAsync(user, roleName);
                if (createdUserRole.Succeeded)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        #endregion

        #region Local Utils

        // Send Message on creation of a new user.
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
               // _logger.LogError("MessageChanel Not found");
                return;
            }

            await _messageStoreRepository.Add(messageStore);
        }

        // This verifies the provided password againt the passwordhash value available in the database after cryptographic decryption
        private static bool VerifyPassWordHash(string password, byte[] storedPasswordHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(nameof(password), "value cannot be null, empty or be whitespace only");

            if (storedPasswordHash.Length != 64)
                throw new ArgumentException(nameof(storedPasswordHash), "invalid length of password hash");
            //if (storedPasswordSalt.Length != 128)
            //    throw new ArgumentException(nameof(storedPasswordSalt), "Invalid length of password hash");

            using (var passCryptoHash = new System.Security.Cryptography.HMACSHA512(storedPasswordHash))
            {
                var computedHash = passCryptoHash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i< computedHash.Length; i++)
                {
                    if (computedHash[i] != storedPasswordHash[i])
                        return false;
                }
                return true;
            }

        }
        #endregion

        #endregion
    }
}
