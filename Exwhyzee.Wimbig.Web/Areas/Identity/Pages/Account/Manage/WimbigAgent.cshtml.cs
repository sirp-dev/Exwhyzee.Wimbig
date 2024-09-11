using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Agent;
using Exwhyzee.Wimbig.Application.Agent.Dto;
using Exwhyzee.Wimbig.Data.Repository.Categorys.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Exwhyzee.Wimbig.Web.Areas.Identity.Pages.Account.Manage
{
    public class WimbigAgentModel : PageModel
    {
        
        private readonly IAgentAppService _agentService;
        private readonly IHostingEnvironment _hostingEnv; 

        public WimbigAgentModel(IHostingEnvironment env, IAgentAppService agentAppService)
        {
            _agentService = agentAppService;
        }

        [BindProperty]
        public AgentDto AgentDto { get; set; }

       

        public async Task OnGet(string returnUrl = null)
        {
           
           
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                   
                    var agentId = await _agentService.Add(AgentDto);


                    TempData["output"] = "Form Submitted Successful";
                    return RedirectToAction("Agent", "Home", new { area = "" });
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}