using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Exwhyzee.UI.Notify
{
    public class Notifier : INotifier
    {
        private readonly IList<NotifyEntry> _entries;
        private readonly ILogger _logger;

        public Notifier(
            ILoggerFactory loggerFactory)
        {
            _entries = new List<NotifyEntry>();
            _logger = loggerFactory.CreateLogger<Notifier>();
        }        

        public void Add(NotifyType type, string message)
        {
            _logger.LogInformation("Notification {0} message: {1}", type, message);
            _entries.Add(new NotifyEntry { Type = type, Message = message });
        }

        public IEnumerable<NotifyEntry> List()
        {
            return _entries;
        }
    }
}
