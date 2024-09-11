using System;

namespace Exwhyzee
{
    public class Clock : IClock
    {
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow.AddHours(1); }
        }
    }
}
