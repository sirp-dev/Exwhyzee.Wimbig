using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.NotificationService.Abstract;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace Exwhyzee.Wimbig.NotificationService.Service
{
    public class NotificationSerice : IMicroService
    {
        private readonly IMicroServiceController _controller;
        private readonly IMessageStoreRepository _messageStoreRepository;
        private readonly Func<MessageChannel, INotificationSender> _senderFactory;


        private readonly Timer timer = new Timer(3000);

        public NotificationSerice(IMicroServiceController controller,
            IMessageStoreRepository messageStoreRepository,
            Func<MessageChannel, INotificationSender> fetcherFactory)
        {
            _controller = controller;
            _messageStoreRepository = messageStoreRepository;
            _senderFactory = fetcherFactory;
        }

        public void Start()
        {
            Console.WriteLine("Notification Service Started.");
            timer.Start();

            timer.Elapsed += NotifyAsync;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async void NotifyAsync(object sender, ElapsedEventArgs e)
        {
            await NotifyEmailAsync();
            await NotifySmsAsync();
        }

        private async Task NotifySmsAsync()
        {
            //Fetch Notifications to be sent
            try
            {
                Console.WriteLine("Fetch SMS Notifications to be sent");
                timer.Enabled = false;
                var notifications = await GetMessagesAsync(MessageChannel.SMS);

                Console.WriteLine($"{notifications.Count} SMS Notifications were found");

                if (notifications.Count < 1)
                {
                    Console.WriteLine("No message found");
                    timer.Enabled = true;
                    return;
                }
                //Send Notification and Persisit
                await AttemptNotify(notifications);

                timer.Enabled = true;
            }
            catch (Exception)
            {
                timer.Enabled = true;
            }
        }

        private async Task NotifyEmailAsync()
        {
            //Fetch Notifications to be sent
            try
            {
                Console.WriteLine("Fetch Email Notifications to be sent");

                timer.Enabled = false;
                var notifications = await GetMessagesAsync(MessageChannel.Email);

                Console.WriteLine($"{notifications.Count} Email Notifications were found");

                if (notifications.Count < 1)
                {
                    Console.WriteLine("No message found");
                    timer.Enabled = true;
                    return;
                }
                //Send Notification and Persisit
                await AttemptNotify(notifications);

                timer.Enabled = true;
            }
            catch (Exception c)
            {
                timer.Enabled = true;
            }
        }

        private async Task<List<MessageStore>> GetMessagesAsync(MessageChannel messageChannel)
        {
            Console.WriteLine("Fetching Messages from Repo");
            // This repo returns a List<T>
            var fetchedNotifications = await _messageStoreRepository.FetchNotificationsToBeSent(messageStatus: MessageStatus.Not_Sent,
                                                                            retriesLimit: 5,
                                                                            messageChannel: messageChannel);
            return fetchedNotifications;
        }

        private async Task AttemptNotify(List<MessageStore> notifications)
        {
            foreach (var messge in notifications)
            {
                //var channel = messge.MessageChannel;

                var notificationResult = await SendNotification(messge);

                // Persist to DB
                await PersistNotification(notificationResult);
            }
        }

        private async Task PersistNotification(MessageStore result)
        {
            Console.WriteLine("Persisting to DB...");
            await _messageStoreRepository.Update(result);
            Console.WriteLine("Persisted to DB");
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private async Task<MessageStore> SendNotification(MessageStore message)
        {
            var result = new MessageStore();
            var channel = message.MessageChannel;
            switch (channel)
            {
                case MessageChannel.Email:
                    Console.WriteLine("Email Switch");
                    result = await _senderFactory(channel).Send(message);
                    break;
                case MessageChannel.SMS:
                    Console.WriteLine("SMS Switch");
                    result = await _senderFactory(channel).Send(message);
                    break;
                default:
                    Console.WriteLine("We haven't implemented that yet");
                    break;
            }

            return result;
        }
    }
}
