using Exwhyzee.Wimbig.Core.MessageStores;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.NotificationService.Abstract
{
    public interface INotificationSender
    {
        Task<MessageStore> Send(MessageStore messageStore);
    }
}
