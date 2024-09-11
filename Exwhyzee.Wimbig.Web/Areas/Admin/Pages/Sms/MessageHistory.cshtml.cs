using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.Count.Dtos;
using Exwhyzee.Wimbig.Application.SmsMessages;
using Exwhyzee.Wimbig.Application.SmsMessages.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Data.Repository.Count;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Sms
{
    [Authorize]
    public class MessageHistory : PageModel
    {
        private readonly ISmsMessageAppService _smsMessageAppService;

        public MessageHistory(
            ISmsMessageAppService smsMessageAppService
            )
        {
            _smsMessageAppService = smsMessageAppService;
        }

        public PagedList<SmsMessageDto> SmsMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string status)
        {
          if(status != null)
            {
ViewData["message"] = "Message Sent Successful";
            }
            
   

            SmsMessage = await _smsMessageAppService.GetAsync();

            return Page();
        }
    }
}