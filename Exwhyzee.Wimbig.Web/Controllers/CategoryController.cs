using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Sections;
using Exwhyzee.Wimbig.Application.Sections.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Exwhyzee.Wimbig.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly ISectionAppService _sectionAppService;

        public CategoryController(ICategoryAppService categoryAppService, ISectionAppService sectionAppService)
        {
            _sectionAppService = sectionAppService;
            _categoryAppService = categoryAppService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}