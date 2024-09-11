using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exwhyzee.Tasks
{
    public class BackgroundProcess : BackgroundProcess<bool>
    {

    }

    public class BackgroundProcess<TOptions> : IBackgroundTask
    {
        private readonly Action<IServiceProvider, CancellationToken> _process;
        private readonly TOptions _options;
       // private Task _task;

        public BackgroundProcess()
        {
            //no op
        }

        public BackgroundProcess(TOptions options, Action<IServiceProvider, CancellationToken> process)
        {
            _options = options;
            _process = process ?? throw new ArgumentNullException(nameof(process));
        }

        public Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return new Task(() => _process(serviceProvider, cancellationToken));
        }
    }
}
