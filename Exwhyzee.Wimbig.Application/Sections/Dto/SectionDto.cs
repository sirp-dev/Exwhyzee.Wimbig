using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.Sections.Dto
{
    public class SectionDto
    {
        public long SectionId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }

        public EntityStatus EntityStatus { get; set; }
    }
}
