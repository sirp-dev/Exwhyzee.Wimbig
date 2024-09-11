using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.Data.Repository.Authorization
{
    public class BaseRepository : IDisposable
    {
        private readonly string connectionString;

        public BaseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
