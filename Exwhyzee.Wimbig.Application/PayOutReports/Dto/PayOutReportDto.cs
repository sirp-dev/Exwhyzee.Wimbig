﻿using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Application.PayOutReports.Dto
{
    public class PayOutReportDto
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public decimal PercentageAmount { get; set; }


        public int Percentage { get; set; }

        public int Reference { get; set; }

        public PayoutEnum Status { get; set; }

        public string Note { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string UserId { get; set; }
        public string userName { get; set; }
        public string RoleName { get; set; }


    }
}
