using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EXWHYZEE.Data
{
    public interface IDatabaseProviderFactory
    {
        IDbConnection CreateConnection();
    }
}
