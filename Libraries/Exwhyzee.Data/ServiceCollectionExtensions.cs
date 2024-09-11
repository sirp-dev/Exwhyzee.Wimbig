using Exwhyzee.Data;
using Microsoft.Extensions.DependencyInjection;


namespace Exwhyzee.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Exwhyzee Data services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddExwhyzeeData(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionStringFactory, DefaultConnectionStringFactory>();
            services.AddTransient<IStorage, MsSqlStorage>();
            services.AddTransient<IDatabaseProviderFactory, MsSqlDatabaseProviderFactory>();

            return services;
        }
    }
}
