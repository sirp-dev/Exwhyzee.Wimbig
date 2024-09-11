using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Exwhyzee.Events
{
    public interface IEventBus
    {
        Task NotifyAsync(string message, IDictionary<string, object> arguments);
        void Subscribe(string message, Func<IServiceProvider, IDictionary<string, object>, Task> action);
    }
}
