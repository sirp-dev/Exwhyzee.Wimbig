using System.Threading.Tasks;

namespace Exwhyzee.Tasks
{
    public class BackgroundTasksStarter
    {
        private readonly IBackgroundTaskService _backgroundService;

        public BackgroundTasksStarter(IBackgroundTaskService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        public Task ActivatedAsync()
        {
            _backgroundService.Activate();

            return Task.CompletedTask;
        }

        public Task ActivatingAsync()
        {
            return Task.CompletedTask;
        }

        public Task TerminatedAsync()
        {
            return Task.CompletedTask;
        }

        public Task TerminatingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
