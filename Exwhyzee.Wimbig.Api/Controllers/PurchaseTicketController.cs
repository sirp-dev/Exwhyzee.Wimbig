using Exwhyzee.Wimbig.Api.Controllers.Dtos.PuchaseTickerDtos;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers
{
    public class PurchaseTicketController: Base.BaseController
    {
        #region Fields
        private readonly IPurchaseTicketAppService _purchaseTicketAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Constructor
        public PurchaseTicketController(IPurchaseTicketAppService purchaseTicketAppService, IHttpContextAccessor httpContextAccessor)
        {
            _purchaseTicketAppService = purchaseTicketAppService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Actions
        /// <summary>
        ///Map a raffle to a category.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(long))]
        public async Task<IActionResult> ProcessTicket(PurchaseTicketDto purchaseTicket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _purchaseTicketAppService.ProcessTicket(purchaseTicket.Transaction,purchaseTicket.Wallet,purchaseTicket.Tickets);
                    if (insertId > 0)
                    {
                        return Ok(insertId);
                    }
                    return BadRequest("Error! Ticket was not processed succesfully");
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        ///Map a raffle to a category.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(long))]
        public async Task<IActionResult> UpdateTicket(long ticketId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _purchaseTicketAppService.UpdateTicket(ticketId);
                    if (insertId > 0)
                    {
                        return Ok(insertId);
                    }
                    return BadRequest((long)HttpStatusCode.BadRequest);
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        /// Update mapping of raffle to category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(PagedList<TicketDto>))]
        public async Task<IActionResult> GetAllTickets(DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null, long? raffleId = null)
        {
            try
            {
                var purchaseTickets = await _purchaseTicketAppService.GetAllTickets(dateStart,dateEnd,startIndex,count,searchString);
                return Ok(purchaseTickets);
             
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }
        #endregion
    }
}
