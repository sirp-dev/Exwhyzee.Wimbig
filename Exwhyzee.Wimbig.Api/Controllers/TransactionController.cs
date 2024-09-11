using Exwhyzee.Wimbig.Application.Transactions;
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
    public class TransactionController: Base.BaseController
    {
        #region Fields
        private readonly ITransactionAppService _transactionAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Ctor
        public TransactionController(ITransactionAppService transactionAppService, IHttpContextAccessor httpContextAccessor)
        {
            _transactionAppService = transactionAppService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Post Update and Delete
        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(TransactionDto))]
        public async Task<IActionResult> CreateTransaction([FromBody]InsertTransactionDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var transaction = await _transactionAppService.CreateTransaction(model);
                    if (transaction != null)
                    {
                        return Ok(transaction);
                    }
                    return BadRequest("Section creation not succesfull, Please try again");
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        ///Update a transaction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Produces(typeof(long))]
        public async Task<IActionResult> UpdateTransaction([FromBody]TransactionDto model)
        {
            try
            {
                if (model != null)
                {
                  var transactionUpdated =  await _transactionAppService.UpdateTransaction(model);
                    if(transactionUpdated != null)
                    {
                        return Ok(transactionUpdated);
                    }
                    return BadRequest("Transaction was not successful, please try again later");
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }
        #endregion

        #region Get Actions

        /// <summary>
        /// Get transaction by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(TransactionDto))]
        public async Task<IActionResult> GetTransaction([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var transaction = await _transactionAppService.GetTransaction(id);
                if (transaction != null)
                {
                    return Ok(transaction);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }


        /// <summary>
        /// Get paggeNatedList of all Transaction
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="walletId"></param>
        /// <param name="status"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(PagedList<TransactionDto>))]
        public async Task<IActionResult> GetAllTransactions(string userId = null, long? walletId = null, int? status = null,
           DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                if (startIndex > int.MaxValue || count > int.MaxValue)
                    return BadRequest($"Invalid startindex or count value. check entry and try again");

                var transactions = await _transactionAppService.GetAllTransactions(userId, walletId, status, dateStart, dateEnd, startIndex,count,searchString);
               
                return Ok(transactions);
               
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }
        #endregion

    }
}
