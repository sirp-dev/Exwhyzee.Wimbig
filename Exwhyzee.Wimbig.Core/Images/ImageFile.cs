using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Images
{
    public class ImageFile
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Extension { get; set; }
        public DateTime DateCreated { get; set; }
        public EntityStatus Status { get; set; }
        public bool IsDefault { get; set; } =                                                                                                                                                                                                                                                                              false;
    }
}
