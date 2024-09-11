using Exwhyzee.Caching;
using Exwhyzee.Configuration;
using Exwhyzee.Environment;
using Exwhyzee.Events;
using Exwhyzee.Security;
using Exwhyzee.Tasks;
using Exwhyzee.UI.Notify;
using Exwhyzee.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Exwhyzee.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add ANQ Core services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddExwhyzeeCoreServices(this IServiceCollection services)
        {
            //Common
            services.AddSingleton<IClock, Clock>();

            //Configuration
            services.AddTransient<ISettingsService, SettingsService>();

            //Caching
            services.AddSingleton<ISignal, Signal>();

            //Events
            services.AddTransient<IEventBus, DefaultEventBus>();
            services.AddSingleton<IEventBusState, EventBusState>();

            //Environment
            services.AddSingleton<IApplicationEnvironment, ApplicationEnvironment>();
            
            //UI
            services.AddScoped<INotifier, Notifier>();            
            
            //Security
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IStartupTask, SecurityStartupTask>();

            //Tasks
            services.AddSingleton<IBackgroundTaskService, BackgroundTaskService>();
            services.AddSingleton<BackgroundTasksStarter>();
            services.AddSingleton<StartupTasksExecutor>();

            //Utilities
            services.AddTransient<IJsonConverter, DefaultJsonConverter>();
            services.AddTransient<IHttpApiUtility, HttpApiUtility>();
            services.AddTransient<IClientHostAddressAccessor, ClientHostAddressAccessor>();

            return services;
        }
    }
}
