using Exwhyzee.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Core.Agent
{
    public class Agents
    {
        public long Id { get; set; }
        public string Fullname { get; set; }
        public string EmailAddress { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }
        public string AreYouNewToWimbig { get; set; }
        public string ContactAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string CurrentOccupation { get; set; }
        public string PhoneNumber { get; set; }
        public string ShopLocation { get; set; }
        public string Gender { get; set; }
        public AgentStatusEnum Status { get; set; }



    }
}
