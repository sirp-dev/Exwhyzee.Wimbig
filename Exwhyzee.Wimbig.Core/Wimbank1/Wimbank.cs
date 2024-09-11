﻿using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Wimbank
{
    public class Wimbank
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateOfTransaction { get; set; }

        public EntityStatus Status { get; set; }

        public string Username { get; set; }
        
    }
}