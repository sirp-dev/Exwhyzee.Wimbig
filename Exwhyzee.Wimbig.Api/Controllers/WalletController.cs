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
    public class WalletController: Base.BaseController
    {
        #region Fields
        private readonly IWalletAppService _walletAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Constructor
        public WalletController(IWalletAppService walletAppService, IHttpContextAccessor httpContextAccessor)
        {
            _walletAppService = walletAppService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Actions
        /// <summary>
        ///  create a wallet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(long))]
        public async Task<IActionResult> InsertWallet(InsertWalletDto wallet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _walletAppService.InsertWallet(wallet);
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
        /// Update a wallet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(int))]
        public async Task<IActionResult> UpdateTicket(WalletDto wallet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _walletAppService.UpdateWallet(wallet);
               
                    return Ok((int)HttpStatusCode.OK);                  
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        /// Get paginated list of all wallets
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(PagedList<WalletDto>))]
        public async Task<IActionResult> GetAllWallets( int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                if (startIndex < 0 || count > int.MaxValue || count < 0)
                {
                    var wallets = await _walletAppService.GetAllWallets(startIndex, count, searchString);
                    return Ok(wallets);
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(WalletDto))]
        public async Task<IActionResult> GetWallet(string userId)
        {
            try
            {
                if (!(string.IsNullOrEmpty(userId)))
                {
                    var wallet = await _walletAppService.GetWallet(userId);
                    return Ok(wallet);
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }
        #endregion
    }
}
