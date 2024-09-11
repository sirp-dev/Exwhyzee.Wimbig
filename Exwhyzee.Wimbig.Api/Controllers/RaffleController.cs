using Exwhyzee.Wimbig.Api.Controllers.Base;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers
{
    public class RaffleController: Base.BaseController
    {
        #region Fields
        private readonly IRaffleAppService _raffleAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Ctro
        public RaffleController(IRaffleAppService raffleAppService, IHttpContextAccessor httpContextAccessor)
        {
            _raffleAppService = raffleAppService;
            _httpContextAccessor = httpContextAccessor;

        }
        #endregion

        #region Post Update and Delete
        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(long))]
        public async Task<IActionResult> Create([FromBody]RaffleDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _raffleAppService.Add(model);
                    if (insertId > 0)
                    {
                        return Ok(insertId);
                    }
                    return BadRequest("Category creation not succesfull, Please try again");
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Produces(typeof(int))]
        public async Task<IActionResult> Delete([FromBody]RaffleDto model)
        {
            try
            {
                if (model.Id > 0)
                {
                    await _raffleAppService.Delete(model.Id);
                    return Ok((int)HttpStatusCode.OK);
                }
                return BadRequest((int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody]RaffleDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _raffleAppService.Update(model);
                    return Ok((int)HttpStatusCode.OK);
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
        /// Get raffle by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(RaffleDto))]
        public async Task<IActionResult> GetRaffleById([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var raffle = await _raffleAppService.GetById(id);
                if (raffle != null)
                {
                    return Ok(raffle);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }


        /// <summary>
        /// Get list of raffles 
        /// </summary>
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
        [Produces(typeof(PagedList<RaffleDto>))]
        public async Task<IActionResult> GetAllRaflles(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                if (startIndex > int.MaxValue || count > int.MaxValue)
                    return BadRequest($"Invalid startindex or count value. check entry and try again");

                PagedList<RaffleDto> raffles = await _raffleAppService.GetAll(status, dateStart, dateEnd, startIndex, count, searchString);

				return Ok(raffles);
                
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
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[Produces(typeof(PagedList<RaffleDto>))]
		public async Task<IActionResult> GetAllRafllesWithImage([FromServices] IMapImageToRaffleAppService raffleImageAppService,int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
		{
			try
			{
				if (count > int.MaxValue)
					return BadRequest($"Invalid count value. count value too long and change try again");

				PagedList<RaffleDto> raffles = await _raffleAppService.GetAll(status, dateStart, dateEnd, startIndex, count, searchString);
				if(raffles.Source.Count() > 0)
				{
					foreach (var item in raffles.Source)
					{
						var image = await raffleImageAppService.GetAllImagesOfARaffle(item.Id, 1);
						item.ImageUrl = image.FirstOrDefault()?.Url;
					}
					
				}

				return Ok(raffles);

			}
			catch (Exception ex)
			{
				return BadRequest($"Error!, request didnot complete successfully, please try again {ex}");
			}
		}

		/// <summary>
		/// Get raffle by status and count
		/// </summary>
		/// <param name="status"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		[HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(IEnumerable<RaffleDto>))]
        public async Task<IActionResult> GetRaffleByStatus([FromQuery]int status, int count)
        {
            try
            {
                if (status > int.MaxValue || count > int.MaxValue)
                    return BadRequest("Invalid status id or count,input value is out of range, validate your entry and try again");

                var raffle = await _raffleAppService.GetRaffleByStatus(status, count);
                if (raffle != null)
                {
                    return Ok(raffle);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        /// Get all raffles created by agent using the userId being HestedBy Id
        /// </summary>
        /// <param name="hostedBy"></param>
        /// <param name="status"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(PagedList<RaffleDto>))]
        public async Task<IActionResult> GetRaffleByHostedBy([FromQuery]string hostedBy, int? status = null, int startIndex = 0, int count = int.MaxValue)
        {
            try
            {
                if (status > int.MaxValue || count > int.MaxValue)
                    return BadRequest("Invalid status id or count,input value is out of range, validate your entry and try again");

                PagedList<RaffleDto> raffles = await _raffleAppService.GetRafflesByHostedBy(hostedBy,status,startIndex,count);
                if (raffles != null)
                {
                    return Ok(raffles);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }

        }
        #endregion
    }
}
