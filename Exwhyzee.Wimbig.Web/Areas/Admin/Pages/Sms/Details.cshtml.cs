using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.SmsMessages;
using Exwhyzee.Wimbig.Application.SmsMessages.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Sms
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ISmsMessageAppService _smsMessageAppService;

        public DetailsModel(
            ISmsMessageAppService smsMessageAppService
            )
        {
            _smsMessageAppService = smsMessageAppService;
        }
        
        public SmsMessageDto Message { get; set; }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            Message = await _smsMessageAppService.Get(id);

            if (Message == null)
            {
                return RedirectToPage("/Index", new  { message= "Error User Not Found"});
            }
        
            return Page();
        }
    }
}