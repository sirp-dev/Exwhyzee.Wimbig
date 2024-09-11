using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Categories.Dtos
{
    public class CategoryDto
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public EntityStatus EntityStatus { get; set; }

        // foreign key to section
        public long SectionId { get; set; }
    }
}
