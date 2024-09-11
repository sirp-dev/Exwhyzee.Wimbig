using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Categories.Dtos
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(1);

        public string Description { get; set; } 
        public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;

        // foreign key to section
        public long SectionId { get; set; }
    }
}
