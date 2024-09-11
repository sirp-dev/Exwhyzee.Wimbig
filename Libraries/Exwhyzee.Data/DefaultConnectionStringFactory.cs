using EXWHYZEE.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Exwhyzee.Data
{
    public class DefaultConnectionStringFactory : IConnectionStringFactory
        {
            #region Const

            private const string CONN_STR_NAME = "Exwhyzee";

            #endregion

            #region Fields

            private readonly IConfiguration _configuration;

            #endregion

            #region Ctor

            public DefaultConnectionStringFactory(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            #endregion

            #region Methods

            public string GetConnectionString()
            {
                var connStr = _configuration.GetConnectionString(CONN_STR_NAME);
                if (!string.IsNullOrWhiteSpace(connStr))
                    return connStr;

                //Fall back to use the first connectionstring found
                var connStringsSection = _configuration.GetSection("ConnectionStrings")?.GetChildren();
                if (connStringsSection != null && connStringsSection.Any())
                    connStr = connStringsSection.FirstOrDefault().Value;

                return connStr;
            }

            #endregion
        }
 
}
