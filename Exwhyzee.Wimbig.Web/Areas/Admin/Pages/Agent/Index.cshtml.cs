using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exwhyzee.Wimbig.Application.Agent.Dto;
using Exwhyzee.Wimbig.Application.Agent;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Agent
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private IAgentAppService _agentAppService;
        public IndexModel (IAgentAppService agentAppService)
        {
            _agentAppService = agentAppService;
        }
        public PagedList<AgentDto> Agents { get; set; }
       
        public async Task<IActionResult> OnGet()
        {
            Agents = await _agentAppService.GetAsync();
            return Page();
        }

    }
}