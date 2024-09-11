using Exwhyzee.Wimbig.Core.MessageStores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.MessageStores
{
    public interface IMessageStoreRepository : IRepository<MessageStore>
    {
        Task<List<MessageStore>> FetchNotificationsToBeSent(MessageStatus messageStatus, MessageChannel messageChannel, int retriesLimit);

        Task<PagedList<MessageStore>> GetAsyncAllMessege(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null);


        //Task SendMessageMethod();
    }
}
