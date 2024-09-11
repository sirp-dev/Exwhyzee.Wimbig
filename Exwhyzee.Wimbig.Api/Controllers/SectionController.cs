using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Sections;
using Exwhyzee.Wimbig.Application.Sections.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers
{
    public class SectionController : Base.BaseController
    {
        #region Fields
        private readonly ISectionAppService _sectionAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Ctro
        public SectionController(ISectionAppService sectionAppService, IHttpContextAccessor httpContextAccessor)
        {
            _sectionAppService = sectionAppService;
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
        public async Task<IActionResult> CreateSection([FromBody]CreateSectionDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _sectionAppService.Add(model);
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
        public async Task<IActionResult> DeleteSection([FromBody]SectionDto model)
        {
            try
            {
                if (model.SectionId > 0)
                {
                    await _sectionAppService.Delete(model.SectionId);
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
        public async Task<IActionResult> UpdateCategory([FromBody]SectionDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _sectionAppService.Update(model);
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
        /// Get category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(SectionDto))]
        public async Task<IActionResult> GetSectionById([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var section = await _sectionAppService.Get(id);
                if (section != null)
                {
                    return Ok(section);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

  
        /// <summary>
        /// Get list of categories created
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
        [Produces(typeof(IEnumerable<SectionDto>))]
        public async Task<IActionResult> GetSections(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                if (startIndex > int.MaxValue || count > int.MaxValue)
                    return BadRequest($"Invalid startindex or count value. check entry and try again");

                var sections = await _sectionAppService.GetAsync(status, dateStart, dateEnd, startIndex, count, searchString);
                if (sections != null)
                {
                    return Ok(sections);
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
