using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exwhyzee.Tasks
{
    public interface ITask
    {
        Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    /// <summary>
    /// An implementation is instanciated once per host, and reused. <see cref="DoWorkAsync(IServiceProvider, CancellationToken)"/>
    /// is invoked on application start.
    /// </summary>
    public interface IStartupTask : ITask
    {
        int Order { get; }
    }

    /// <summary>
    /// An implementation is instanciated once per host, and reused. <see cref="DoWorkAsync(IServiceProvider, CancellationToken)"/>
    /// is invoked periodically.
    /// </summary>
    public interface IBackgroundTask : ITask
    {       
    }
}
