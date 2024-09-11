using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Hangfire.Core.SMS
{
    public interface ISmsService
    {
        void RunDemo();
    }

    public class SmsService : ISmsService
    {
        public void RunDemo()
        {
            Console.WriteLine("peter");
        }
    }
}
