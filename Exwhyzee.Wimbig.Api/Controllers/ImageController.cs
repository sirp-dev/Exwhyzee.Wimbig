using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Core.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers
{
    public class ImageController: Base.BaseController
    {
        #region Fields
        private readonly IImageFileAppService _imageFileAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Constructor
        public ImageController(IImageFileAppService imageFileAppService, IHttpContextAccessor httpContextAccessor)
        {
            _imageFileAppService = imageFileAppService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

    
        #region Post Update and Delete
        /// <summary>
        /// Create a new section.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(long))]
        public async Task<IActionResult> InsertImageDetail([FromBody]ImageFile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _imageFileAppService.Insert(model);
                    if (insertId > 0)
                    {
                        return Ok(insertId);
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
        /// Delete a section
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Produces(typeof(int))]
        public async Task<IActionResult> DeleteImgae([FromBody]ImageFile model)
        {
            try
            {
                if (model.Id > 0)
                {
                    await _imageFileAppService.Delete(model.Id);
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
        /// Update a section 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Produces(typeof(int))]
        public async Task<IActionResult> UpdateImage([FromBody]ImageFile model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _imageFileAppService.Upate(model);
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
        /// Get Image Detail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(ImageFile))]
        public async Task<IActionResult> GetById([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var image = await _imageFileAppService.GetById(id);
                if (image != null)
                {
                    return Ok(image);
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
