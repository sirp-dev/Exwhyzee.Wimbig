using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Exwhyzee.Enums
{
    public enum DeliveryTypeEnum
    {
        [Description("Delivery")]
        Delivery = 1,

        [Description("Collection")]
        Collection = 2,

        [Description("Delivery or Collection")]
        DeliveryOrCollection = 3
    }
}
