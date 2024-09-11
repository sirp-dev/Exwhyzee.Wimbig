using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exwhyzee.Events
{
    /// <summary>
    /// Registrations are shared across all EventBus instances for a single tenant
    /// </summary>
    public interface IEventBusState
    {
        ConcurrentDictionary<string, ConcurrentBag<Func<IServiceProvider, IDictionary<string, object>, Task>>> Subscribers { get; }

        void Add(string message, Func<IServiceProvider, IDictionary<string, object>, Task> action);
    }
}
