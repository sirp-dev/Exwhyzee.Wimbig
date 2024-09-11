using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendPasswordCodeModel : PageModel
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMessageStoreRepository _messageStoreRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public ResendPasswordCodeModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IMessageStoreRepository messageStoreRepository
           )
        {
            _messageStoreRepository = messageStoreRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnGetAsync(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            System.Random randomInteger = new System.Random();
            int genNumber = randomInteger.Next(1000000);
            user.CodeVerify = genNumber.ToString();
            await _userManager.UpdateAsync(user);


            ///
            var emailMessageBody = $"Please confirm your number is " + genNumber;

            var emailMessage = string.Format("{0};??{1};??{2};??{3}", "Account Confirmation", "Confirm your email", "Thanks " + user.FullName, emailMessageBody);
            var smsMessage = string.Format("{0}, Your Confirmation Number: {1}", user.FullName, genNumber);
            await SendMessage(emailMessage, user.Email, MessageChannel.Email, MessageType.Activation);

            
                await SendMessage(smsMessage, user.PhoneNumber, MessageChannel.SMS, MessageType.Activation);



           
            return RedirectToPage("./ForgotPasswordConfirmation", new { area = "Identity", id = user.Id });


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
               
                return;
            }

            await _messageStoreRepository.Add(messageStore);
        }
    }
}