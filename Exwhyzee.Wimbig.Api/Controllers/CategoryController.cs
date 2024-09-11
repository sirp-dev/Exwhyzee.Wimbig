using Exwhyzee.Wimbig.Api.Controllers.Base;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Api.Controllers
{
   
    public class CategoryController: BaseController
    {
        #region Fields
        private readonly ICategoryAppService _categoryAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ILogger _logger;
        #endregion

        #region Ctro
        public CategoryController(ICategoryAppService categoryAppService, IHttpContextAccessor httpContextAccessor)
        {
            _categoryAppService = categoryAppService;
            _httpContextAccessor = httpContextAccessor;
           
        }
        #endregion

        #region ActionMethods
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
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var insertId = await _categoryAppService.Add(model);
                    if(insertId > 0)
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
		[Produces(typeof(string))]
        public async Task<IActionResult> DeleteCatgeory([FromBody]CategoryDto model)
        {
            try
            {
                if (model.CategoryId > 0 )
                {
                    await _categoryAppService.Delete(model.CategoryId);
                    return Ok(new {message="Sucess! Deleted successfully"});                 
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
		[Produces(typeof(string))]
        public async Task<IActionResult> UpdateCategory([FromBody]CategoryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryAppService.Update(model);
                    return Ok(new { message = "Sucess! Updated successfully" });
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
        [Produces(typeof(CategoryDto))]
        public async Task<IActionResult> GetCategoryById([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var category = await _categoryAppService.Get(id);
                if (category != null)
                {                 
                    return Ok(category);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Error!, request didnot complete successfully, please try again");
            }
        }

        /// <summary>
        /// Get all categories by section ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces(typeof(IEnumerable<CategoryDto>))]
        public async Task<IActionResult> GetCategoriesBySectionId([FromQuery]long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest("Invalid Id");

                var categories = await _categoryAppService.GetCategoriesBySection(id);
                if (categories != null)
                {
                    return Ok(categories);
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
        [Produces(typeof(PagedList<CategoryDto>))]
        public async Task<IActionResult> GetCategories(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            try
            {
                if (startIndex > int.MaxValue || count > int.MaxValue)
                    return BadRequest($"Invalid startindex or count value. check entry and try again");

                var categories = await _categoryAppService.GetAsync( status, dateStart,dateEnd,startIndex,count,searchString);
                if (categories != null)
                {
                    return Ok(categories);
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
