using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.SmsMessages;
using Exwhyzee.Wimbig.Application.SmsMessages.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Sms
{
    public class NewMessage : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICountAppService _countAppService;
        private readonly ISmsMessageAppService _smsMessageAppService;
       // private readonly HttpClient _httpClient;

        public NewMessage(
            UserManager<ApplicationUser> userManger, ICountAppService countAppService,
            ISmsMessageAppService smsMessageAppService
            )
        {
            _countAppService = countAppService;
            _userManager = userManger;
            _smsMessageAppService = smsMessageAppService;
            
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public string SenderId { get; set; }
            public string Message { get; set; }
            public string RecepientNumbers { get; set; }
            public string NumberValue { get; set; }
            public DateTime DateCreated { get; set; }
            public SmsStatusEnum Status { get; set; }

            public string Response { get; set; }
            
        }

       
        public async Task<IActionResult> OnGetAsync()
        {

            
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Input.Status = SmsStatusEnum.Pending;
                Input.DateCreated = DateTime.UtcNow.AddHours(1);
                //join numbers
               
                ///remove duplicate
                Input.RecepientNumbers = Input.RecepientNumbers.Replace("\r\n", ",");
                IList<string> numbers = Input.RecepientNumbers.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                numbers.Distinct().ToList();
                string phonenumbers = string.Join(",", numbers.ToList());


                //
                var message = new SmsMessageDto
                {
                    SenderId = Input.SenderId,
                    DateCreated = Input.DateCreated,
                    Recipient = phonenumbers,
                    Message = Input.Message,
                    Status = Input.Status

                };
                var id = await _smsMessageAppService.Add(message);

                //send message

                var uri = "https://account.kudisms.net/api/";
                var param = string.Format("?username={0}&password={1}&sender={2}&message={3}&mobiles={4}",
                                                "ponwuka123@gmail.com"
                                                , "sms@123"
                                                , Input.SenderId,
                                                Input.Message, phonenumbers);
                var url = uri + param;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";

                //getting the respounce from the request
                HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string response = await streamReader.ReadToEndAsync();

               
                var messageInfo = await _smsMessageAppService.Get(id);
                if (response.Contains("OK"))
                {
                    messageInfo.Response = response.ToString();
                    messageInfo.Status = SmsStatusEnum.Sent;

                    await _smsMessageAppService.Update(messageInfo);
                    ViewData["message"] = "Message Sent Successful";
                    string statusMsg = "Success";
                    return RedirectToPage("./MessageHistory", new { status = statusMsg });
                }
                else
                {
                    messageInfo.Response = response.ToString();
                    messageInfo.Status = SmsStatusEnum.NotSent;
                    await _smsMessageAppService.Update(messageInfo);
                    ViewData["error"] = "Message not sent" + response.ToString();

                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["error 2"] = "Error"+ ex.ToString();
                return Page();

            }
        }
    }
}