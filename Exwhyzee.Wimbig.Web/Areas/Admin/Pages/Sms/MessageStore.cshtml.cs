using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.Count.Dtos;
using Exwhyzee.Wimbig.Application.MessageStores;
using Exwhyzee.Wimbig.Application.MessageStores.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Data.Repository.Count;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Sms
{
    [Authorize(Roles = "Admin,SuperAdmin,mSuperAdmin")]
    public class MessageStoreModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageStoreAppService _messageStoreAppService;
        
        public MessageStoreModel(
            UserManager<ApplicationUser> userManger, IMessageStoreAppService messageStoreAppService
            )
        {
            _messageStoreAppService = messageStoreAppService;
            _userManager = userManger;
        }
       
       

        public PagedList<MessageStoreDto> Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            Message = await _messageStoreAppService.GetAsync(count: 500);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            string response = "";
            try
            {
                var message = await _messageStoreAppService.Get(id);
                if(message.EmailAddress != null)
                {

                }else if(message.PhoneNumber != null)
                {
                    var uri = "https://account.kudisms.net/api/";
                    var param = string.Format("?username={0}&password={1}&sender={2}&message={3}&mobiles={4}",
                                                    "ponwuka123@gmail.com"
                                                    , "sms@123"
                                                    , "WimBig",
                                                    message.Message, message.PhoneNumber);
                    var url = uri + param;

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.ContentType = "application/json";

                    //getting the respounce from the request
                    HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);
                     response = await streamReader.ReadToEndAsync();



                    if (response.Contains("OK"))
                    {
                        TempData["success"] = "successfull" + response;
                    }
                    else
                    {

                        TempData["fail"] = "not sent" + response;
                    }

                }
                else
                {
                    TempData["fail"] = "not found";
                }

                message.Response = response;
                message.MessageStatus = Core.MessageStores.MessageStatus.Sent;
                await _messageStoreAppService.Update(message);

            }
            catch (Exception ex)
            {
                ViewData["error 2"] = "Error" + ex.ToString();
                return Page();

            }
            return RedirectToPage("./MessageStore");
        }
    }
}