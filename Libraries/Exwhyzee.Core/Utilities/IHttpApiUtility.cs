using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace Exwhyzee.Utilities
{
    /// <summary>
    /// Provides services to interact with http endpoints.
    /// </summary>
    public interface IHttpApiUtility
    {
        /// <summary>
        /// Calls API endpoint using HTTP GET.
        /// </summary>
        /// <typeparam name="TResponse">The response type.</typeparam>
        /// <param name="requestUri">The request uri.</param>
        /// <returns><typeparamref name="TResponse"/></returns>
        Task<TResponse> CallApiAsync<TResponse>(string requestUri) where TResponse : class;
        
        /// <summary>
        /// Calls an API endpoint using HTTP POST.
        /// </summary>
        /// <typeparam name="TRequest">The request type.</typeparam>
        /// <typeparam name="TResponse">The response type.</typeparam>
        /// <param name="requestUri">The request uri.</param>
        /// <param name="model">A model object of type <typeparamref name="TRequest"/></param>
        /// <returns></returns>
        Task<TResponse> PostApiAsync<TRequest, TResponse>(string requestUri, TRequest model) where TResponse : class;
    }
}
