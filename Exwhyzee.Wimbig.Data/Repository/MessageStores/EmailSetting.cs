using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Data.Repository.MessageStores
{
    public class EmailSetting
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
