using Exwhyzee.Wimbig.Application.MapRaffleToCategorys;
using Exwhyzee.Wimbig.Application.MapRaffleToCategorys.Dtos;
using Exwhyzee.Wimbig.Core.MapRaffleToCategorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers
{
    public class RaffleToCategoryController: Base.BaseController
    {
        #region Fields
        private readonly IMapRaffleToCategoryAppService _mapRaffleToCategoryAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Constructor
        public RaffleToCategoryController(IMapRaffleToCategoryAppService mapRaffleToCategoryAppService, IHttpContextAccessor httpContextAccessor)
        {
            _mapRaffleToCategoryAppService = mapRaffleToCategoryAppService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Action Method
        #region Post Update and Delete
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
        public async Task<IActionResult> MapRaffleToCategory([FromBody]MapRaffleToCategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _mapRaffleToCategoryAppService.Add(model);
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

        /// <summary>
        /// Unmap RaffleFrom Category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Produces(typeof(int))]
        public async Task<IActionResult> UnMapRaffleFromCategpory([FromBody]MapRaffleToCategory model)
        {
            try
            {
                if (model.Id > 0)
                {
                    await _mapRaffleToCategoryAppService.Delete(model.Id);
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
        /// Update mapping of raffle to category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Produces(typeof(int))]
        public async Task<IActionResult> UpdateMapping([FromBody]MapRaffleToCategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mapRaffleToCategoryAppService.Update(model);
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
        /// Get a single raffleToCategory Mapping by an Identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(MapRaffleToCategory))]
        public async Task<IActionResult> GetRaffleToCatgeoryMapById([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var mapping = await _mapRaffleToCategoryAppService.Get(id);
                if (mapping != null)
                {
                    return Ok(mapping);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        /// Get all raffles mapped all catgeories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(PagedList<MapRaffleToCategory>))]
        public async Task<IActionResult> GetAll(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                if (startIndex > int.MaxValue || count > int.MaxValue)
                    return BadRequest($"Invalid startindex or count value. check entry and try again");

                var mappedRafflesToCategory = await _mapRaffleToCategoryAppService.GetAsync(status,dateStart,dateEnd,startIndex,count,searchString);
                if (mappedRafflesToCategory != null)
                {
                    return Ok(mappedRafflesToCategory);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        /// Get all raffles mapped to a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(PagedList<MapRaffleToCategory>))]
        public async Task<IActionResult> GetAllRafflesMappedToACategory([FromQuery]long categoryId)
        {
            try
            {
                if (categoryId > long.MaxValue || categoryId < 1 )
                    return BadRequest($"Invalid startindex or count value. check entry and try again");

                var rafflesOfaCategory = await _mapRaffleToCategoryAppService.GetRafflesByCategory(categoryId);
                if (rafflesOfaCategory != null)
                {
                    return Ok(rafflesOfaCategory);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }
        #endregion
        #endregion
    }
}
