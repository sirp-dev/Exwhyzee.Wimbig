using Exwhyzee.Wimbig.Application.Categories.Dtos;
using Exwhyzee.Wimbig.Application.Sections.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.CategoryManagement.ViewModel
{
    public class CreateCategoryViewModel
    {
        public CreateCategoryDto CreateCategoryDto { get; set; }

        public long SectionId { get; set; }
    }
}
