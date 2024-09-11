using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Core.MapImagesToRaffles;
using Exwhyzee.Wimbig.Core.RaffleImages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers
{
    public class ImageToRaffleController : Base.BaseController
    {
        #region Fields
        private readonly IMapImageToRaffleAppService _mapImageToRaffleAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Ctro
        public ImageToRaffleController(IMapImageToRaffleAppService mapImageToRaffleAppService, IHttpContextAccessor httpContextAccessor)
        {
            _mapImageToRaffleAppService = mapImageToRaffleAppService;
            _httpContextAccessor = httpContextAccessor;

        }
        #endregion

        #region Post Update and Delete
        /// <summary>
        /// Create a new map of image to a raffle.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(long))]
        public async Task<IActionResult> MapImageToRaffle([FromBody]MapImageToRaffle model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _mapImageToRaffleAppService.InsertMap(model);
                    if (insertId > 0)
                    {
                        return Ok(insertId);
                    }
                    return BadRequest("Mapping image to raffle not succesfull, Please try again");
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
        public async Task<IActionResult> Delete([FromBody]MapImageToRaffle model)
        {
            try
            {
                if (model.Id > 0)
                {
                    await _mapImageToRaffleAppService.Delete(model.Id);
                    return Ok((int)HttpStatusCode.OK);
                }
                return BadRequest("Your entry is incomplete or invalid, please check entry and try again");
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
		[Produces(typeof(int))]
        public async Task<IActionResult> Update([FromBody]MapImageToRaffle model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mapImageToRaffleAppService.Update(model);
                    return Ok((int)HttpStatusCode.OK);
                }
                return BadRequest((int)HttpStatusCode.NotModified);
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }
        #endregion

        #region Get Actions

        /// <summary>
        /// Get mapped image by its identifier by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(MapImageToRaffle))]
        public async Task<IActionResult> GetMappedImageById([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var mappedImage = await _mapImageToRaffleAppService.GetById(id);
                if (mappedImage != null)
                {
                    return Ok(mappedImage);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        /// Get all images of a raffle. Note if count is set to 1 or null it gets the default image of the raffle else it gets all the images of the raffle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(IEnumerable<ImageOfARaffle>))]
        public async Task<IActionResult> GetImagesOfARaffle([FromQuery]long raffleId,[FromQuery] int count = int.MaxValue)
        {
            try
            {
                if (raffleId < 1)
                    return BadRequest("Invalid Id");

                var images = await _mapImageToRaffleAppService.GetAllImagesOfARaffle(raffleId,count);
                if (images != null)
                {
                    return Ok(images);
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
