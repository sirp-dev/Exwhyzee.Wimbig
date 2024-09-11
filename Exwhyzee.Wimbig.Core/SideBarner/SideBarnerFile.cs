using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.SideBarner
{
    public class SideBarnerFile
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Extension { get; set; }
        public DateTime DateCreated { get; set; }
        public EntityStatus Status { get; set; }
        
        public bool IsDefault { get; set; } =                                                                                                                                                                                                                                                                              false;

        public string TargetLocation { get; set; }
    }
}
