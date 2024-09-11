using Exwhyzee.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Exwhyzee.Extensions.DependencyInjection
{
    public class ExwhyzeeCoreOptions
    {
        /// <summary>
        /// Sets or gets a value indication if startup tasks are enabled.
        /// </summary>
        public bool EnableStartupTasks { get; set; }

        /// <summary>
        /// Sets or gets a value indication if background tasks are enabled.
        /// </summary>
        public bool EnableBackgroundTasks { get; set; }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExwhyzeeeCore(this IApplicationBuilder app)
        {
            app.UseExwhyzeeeCore(options: null);

            return app;
        }

        public static IApplicationBuilder UseExwhyzeeeCore(this IApplicationBuilder app, ExwhyzeeCoreOptions options)
        {
            bool hasOptions = options != null;

            if (hasOptions && options.EnableStartupTasks)
                UseStartupTasks(app);

            if (hasOptions && options.EnableBackgroundTasks)
                UseBackgroundTasks(app);

            return app;
        }

        #region Utils

        private static IApplicationBuilder UseStartupTasks(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            
            //Startup tasks
            var _executor = serviceProvider.GetService<StartupTasksExecutor>();
            var executorTask = _executor.ExecuteAsync();
            Task.WaitAll(executorTask);

            return app;
        }

        private static IApplicationBuilder UseBackgroundTasks(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            //Background tasks
            var _starter = serviceProvider.GetService<BackgroundTasksStarter>();
            Task.Run(async () => await _starter.ActivatedAsync());

            return app;
        }

        #endregion
    }
}
