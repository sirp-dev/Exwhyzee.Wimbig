using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Utilities
{
    public class HttpApiUtility : IHttpApiUtility
    {
        private const string JsonContentType = "application/json";
        private readonly ILogger _logger;

        public HttpApiUtility(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpApiUtility>();
        }

        public async Task<TResponse> CallApiAsync<TResponse>(string requestUri) where TResponse : class
        {
            var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<TResponse>(data,
                        new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                    return deserializedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(0, e, e.Message);

                return null;
            }
        }

        public async Task<TResponse> PostApiAsync<TRequest, TResponse>(string requestUri, TRequest model) where TResponse : class
        {
            var client = new HttpClient();
            try
            {
                var serializedModel = JsonConvert.SerializeObject(model,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                var stringContent = new StringContent(serializedModel, Encoding.UTF8, JsonContentType);

                var response = await client.PostAsync(requestUri, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<TResponse>(data,
                        new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                    return deserializedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(0, e, e.Message);

                return null;
            }
        }
    }
}
