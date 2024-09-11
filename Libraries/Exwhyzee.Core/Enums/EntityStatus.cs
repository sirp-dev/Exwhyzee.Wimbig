using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Exwhyzee.Enums
{
    public enum EntityStatus
    {
        [Description("Active")]
        Active = 1,

        [Description("Deleted")]
        Deleted = 2,

        [Description("Pending")]
        Pending = 3,

        [Description("Success")]
        Success = 4,

        [Description("Failed")]
        Failed = 5,

        [Description("Processing")]
        Processing = 6,

        [Description("Drawn")]
        Drawn = 7,

        [Description("Closed")]
        Closed = 8,
    }

    public enum SubscriberStatusEnum
    {
        [Description("Idle")]
        Idle = 1,

        [Description("Busy")]
        Busy = 2,

        [Description("Not Reachable")]
        Unreachable = 3

    }
    public enum TicketStatusEnum
    {
        [Description("Active")]
        Active = 1,

        [Description("Drawn")]
        Drawn = 2,



    }

    public enum SmsStatusEnum
    {
        [Description("Sent")]
        Sent = 1,

        [Description("Pending")]
        Pending = 2,

        [Description("Not Sent")]
        NotSent = 3,

    }

    public enum AgentStatusEnum
    {
        [Description("Approved")]
        Approved = 1,

        [Description("Pending")]
        Pending = 2,
        
    }

    public enum WinnerReportEnum
    {
        [Description("Confirmed")]
        Confirmed = 1,

        [Description("Pending")]
        Pending = 2,

    }

    public enum PayoutEnum
    {
        [Description("PayedToBank")]
        PayedToBank = 1,

        [Description("Pending")]
        Pending = 2,
        [Description("PayedToWallet")]
        PayedToWallet = 3,

    }

    public enum SideBarnerEnum
    {
        [Description("none")]
        none = 0,
        

        [Description("Side Bar Big")]
        SideBarBig = 10,

        [Description("Side Bar Small")]
        SideBarSmall = 20,

        [Description("Winning Location")]
        WinningLocation = 30,

    }
}
