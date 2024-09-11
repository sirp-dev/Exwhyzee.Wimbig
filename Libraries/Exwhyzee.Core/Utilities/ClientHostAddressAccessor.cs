using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exwhyzee.Utilities
{
    public class ClientHostAddressAccessor : IClientHostAddressAccessor
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public ClientHostAddressAccessor(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
        }

        #endregion

        #region Utils

        private static IEnumerable<string> ParseAddresses(string value)
        {
            return !String.IsNullOrWhiteSpace(value)
                ? value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())
                : Enumerable.Empty<string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the client host address should be read from an HTTP header, specified via <see cref="ClientHostAddressHeaderName"/>.
        /// </summary>
        public bool EnableClientHostAddressHeader { get; set; }

        /// <summary>
        /// The HTTP header name to read the client host address from if <see cref="EnableClientHostAddressHeader"/> is set to true.
        /// If the specified header was not found, the system will fall back to the user host address as provided by the Request object.
        /// </summary>
        public string ClientHostAddressHeaderName { get; set; }

        #endregion

        #region Methods

        public string GetClientAddress()
        {
            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
                return string.Empty;

            var httpContext = _httpContextAccessor.HttpContext;
            var request = httpContext.Request;

            if (EnableClientHostAddressHeader && !String.IsNullOrWhiteSpace(ClientHostAddressHeaderName))
            {
                var headerName = ClientHostAddressHeaderName.Trim();
                var customAddresses = ParseAddresses(request.Headers[headerName]).ToArray();

                if (customAddresses.Any())
                    return customAddresses.First();
            }

            var connInfo = httpContext.Connection;
            var ipAddress = connInfo.RemoteIpAddress.MapToIPv4();
            ipAddress = ipAddress.MapToIPv4();

            return ipAddress.ToString();
        }

        #endregion
    }
}
