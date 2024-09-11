using System;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.MessageStores;
using Microsoft.Extensions.Configuration;

namespace Exwhyzee.Wimbig.NotificationService.Abstract
{
    public abstract class NotificationSender : INotificationSender
    {
        protected readonly IConfiguration _config;

        protected NotificationSender(IConfiguration config)
        {
            _config = config;
        }

        public virtual async Task<MessageStore> Send(MessageStore messageStore)
        {
            return  messageStore;
        }
    }
}
