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
    public class ComposeModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICountAppService _countAppService;
        private readonly ISmsMessageAppService _smsMessageAppService;
       // private readonly HttpClient _httpClient;

        public ComposeModel(
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

        [TempData]
        public string Receipient { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {

            var userdata = await _userManager.FindByIdAsync(id);
            
            if (userdata != null)
            {
                //user list
                Receipient = userdata.PhoneNumber;
              
                TempData["id"] = userdata.Id;
                TempData["userdata"] = "Send a message to " + userdata.FullName + "(username: " + userdata.UserName + ")";
            }
            else if (id.Contains("Location"))
            {
                //remove location from id

                id = id.Remove(0, 8);
                //user list
                var Userlist = await _countAppService.UsersInRole();
                var itemContacts = Userlist.Source.Where(x=>x.CurrentCity == id).Select(x => x.PhoneNumber);
                Receipient = string.Join(",", itemContacts.ToList());
            }
            else if (id == "All")
            {
                //user list
                var Userlist = await _countAppService.UsersInRole();
                var itemContacts = Userlist.Source.Select(x => x.PhoneNumber);
                Receipient = string.Join(",", itemContacts.ToList());
            }
            else if (id == "DGA")
            {
                //dga list

                var usersD = _userManager.GetUsersInRoleAsync("DGAs").Result;
                var itemContacts = usersD.Select(x => x.PhoneNumber);
                Receipient = string.Join(",", itemContacts.ToList());
            }
            else if (id == "Agent")
            {
                //agents

                var usersA = _userManager.GetUsersInRoleAsync("Agent").Result;
                var itemContacts = usersA.Select(x => x.PhoneNumber);
                Receipient = string.Join(",", itemContacts.ToList());

            }
            else if (id == "Supervisor")
            {
                //supervisor
                var usersS = _userManager.GetUsersInRoleAsync("Supervisor").Result;
                var itemContacts = usersS.Select(x => x.PhoneNumber);
                Receipient = string.Join(",", itemContacts.ToList());
            }
            else if (id == "Admin")
            {
                //supervisor
                var usersS = _userManager.GetUsersInRoleAsync("Admin").Result;
                var itemContacts = usersS.Select(x => x.PhoneNumber);
                Receipient = string.Join(",", itemContacts.ToList());
            }
            else if (id == "Stakeholders")
            {
                //supervisor
                var usersS = _userManager.GetUsersInRoleAsync("SuperAdmin").Result;
                var itemContacts = usersS.Select(x => x.PhoneNumber);
                Receipient = string.Join(",", itemContacts.ToList());
            }


            if (string.IsNullOrEmpty(Receipient))
            {
                ViewData["message"] = "No Contact Found";
                return Redirect("./Compose");
            }



            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Input.Status = SmsStatusEnum.Pending;
                Input.DateCreated = DateTime.UtcNow.AddHours(1);
                //join numbers
                Input.RecepientNumbers = Input.RecepientNumbers +","+  Input.NumberValue;
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