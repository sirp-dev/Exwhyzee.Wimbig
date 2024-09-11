using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Categories
{
    public class Category
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
