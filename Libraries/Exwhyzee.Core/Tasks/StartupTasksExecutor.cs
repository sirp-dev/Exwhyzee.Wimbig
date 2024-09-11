using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Tasks
{
    public class StartupTasksExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly IEnumerable<IStartupTask> _startupTasks;

        public StartupTasksExecutor(
            IServiceProvider serviceProvider,
            IApplicationLifetime applicationLifetime,
            IEnumerable<IStartupTask> startupTasks)
        {
            _serviceProvider = serviceProvider;
            _applicationLifetime = applicationLifetime;
            _startupTasks = startupTasks;
        }

        public async Task ExecuteAsync()
        {
            //Startup tasks
            var orderedTasks = _startupTasks.OrderBy(x => x.Order).ToList();
            var cancellationToken = _applicationLifetime.ApplicationStopping;

            for (int i = 0; i < orderedTasks.Count; i++)
            {
                await orderedTasks[i].DoWorkAsync(_serviceProvider, cancellationToken);
            }
        }
    }
}
