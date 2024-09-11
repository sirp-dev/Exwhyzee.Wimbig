using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Notification.Web.Data
{
   
    public class MessageService : IMessageService
    {
        private readonly EmailSetting _mailSetting;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IMessageStoreRepository _messageStoreRepository;

        public MessageService(IHostingEnvironment env, IOptions<EmailSetting> optionsEmail,
            IMessageStoreRepository messageStoreRepository)
        {
            _messageStoreRepository = messageStoreRepository;
        }

        public async Task SendMessageMethod()
        {
            var notificationsSms = await GetMessagesAsync(MessageChannel.SMS);
            await AttemptNotifySMS(notificationsSms);

            var notificationsEmail = await GetMessagesAsync(MessageChannel.Email);

            await AttemptNotifyEmail(notificationsEmail);
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

        private async Task AttemptNotifySMS(List<MessageStore> notifications)
        {
            foreach (var messge in notifications)
            {
                //var channel = messge.MessageChannel;

                var notificationResult = await SendNow(messge);

                // Persist to DB
                await PersistNotification(notificationResult);
            }
        }

        private async Task AttemptNotifyEmail(List<MessageStore> notifications)
        {
            foreach (var messge in notifications)
            {
                //var channel = messge.MessageChannel;

                var notificationResult = await SendNow(messge);

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


        public async Task<MessageStore> SendNow(MessageStore messageStore)
        {
            if (messageStore.MessageChannel == MessageChannel.SMS)
            {
                string response = "";
                try
                {
                    Console.WriteLine("Message ID: {0}, Message Channel: {1}, Address: {2}, Retries: {3}",
                        messageStore.Id, messageStore.MessageChannel.ToString(), messageStore.ToString(), messageStore.Retries);

                    //var uri = "https://account.kudisms.net/api/";
                    //var param = string.Format("?username={0}&password={1}&sender={2}&message={3}&mobiles={4}",
                    //                                _setting.Username
                    //                                , _setting.Password
                    //                                , "Wimbig",
                    //                                messageStore.Message, messageStore.PhoneNumber);
                    // var url = uri + param;
                    messageStore.Message = messageStore.Message.Replace("won", "W=i=N");
                    messageStore.Message = messageStore.Message.Replace("gmail", "Gma1l");
                    messageStore.Message = messageStore.Message.ToLower().Replace("mtn", "M T Network");
                    //   entity.Message = entity.Message.Replace();
                    var api = "http://account.kudisms.net/api/?username=ponwuka123@gmail.com&password=sms@123&message=@@message@@&sender=@@sender@@&mobiles=@@recipient@@";
                    var url = api.Replace("@@sender@@", "WimBig").Replace("@@recipient@@", messageStore.PhoneNumber).Replace("@@message@@", messageStore.Message);



                    Console.WriteLine("HTTP call to {0}", url);
                    //var httpResponseMessage = await _httpClient.GetAsync(url);
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Timeout = 25000;

                    //getting the respounce from the request
                    HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);
                    response = await streamReader.ReadToEndAsync();



                    if (response.ToUpper().Contains("OK"))
                    {
                        //var responseString = await response;
                        Console.WriteLine("HTTP Response Messgae: {0}", response.ToString());
                        //if (!responseString.Contains("error", StringComparison.OrdinalIgnoreCase))
                        //{
                        Console.WriteLine("SMS Message Sent");
                        messageStore.DateSent = DateTime.UtcNow.AddHours(1);
                        messageStore.MessageStatus = MessageStatus.Sent;
                        messageStore.Retries = 0;
                        messageStore.Response = response;
                        return messageStore;


                    }
                    Console.WriteLine("SMS Message not sent");
                    Console.WriteLine("HTTP Response Messgae: {0}", response);
                    messageStore.DateSent = null;
                    messageStore.MessageStatus = MessageStatus.Not_Sent;
                    messageStore.Retries = messageStore.Retries + 1;
                    messageStore.Response = response;

                    return messageStore;
                }
                catch (Exception e)
                {
                    Console.WriteLine("SMS Message not sent");
                    messageStore.DateSent = null;
                    messageStore.MessageStatus = MessageStatus.Not_Sent;
                    messageStore.Retries = messageStore.Retries + 1;
                    messageStore.Response = response;
                    return messageStore;
                }

            }
            else if (messageStore.MessageChannel == MessageChannel.Email)
            {


                try
                {

                    //check if email is null

                    if (string.IsNullOrEmpty(messageStore.EmailAddress) && !string.IsNullOrEmpty(messageStore.PhoneNumber))
                    {

                    }
                    else if (!string.IsNullOrEmpty(messageStore.EmailAddress) && string.IsNullOrEmpty(messageStore.PhoneNumber))
                    {

                    }
                    else
                    {
                        // Returns False
                        Console.WriteLine("Invalid or null adress");

                        messageStore.MessageStatus = MessageStatus.Not_Sent;
                        messageStore.Retries = messageStore.Retries + 10;
                        // messageStore.Retries = messageStore.Retries > 0 ? messageStore.Retries : messageStore.Retries++ ;
                        return messageStore;
                    }

                    var message = ComposeMessage(messageStore);

                    using (var _smtp = CreateSmtpClient(_mailSetting))
                    {
                        await _smtp.SendMailAsync(message);
                    }
                    // For now one cannot know if the email was sent
                    // Returns true
                    messageStore.DateSent = DateTime.UtcNow.AddHours(1);
                    messageStore.MessageStatus = MessageStatus.Sent;
                    messageStore.Retries += 1;
                    await _messageStoreRepository.Update(messageStore);
                    return messageStore;

                }
                catch (Exception ex)
                {
                    // Returns False
                    Console.WriteLine($"FATAL ERROR: {ex.Message} \n\nException: {ex.StackTrace}");
                    messageStore.MessageStatus = MessageStatus.Not_Sent;
                    messageStore.Retries++;
                    await _messageStoreRepository.Update(messageStore);
                    // messageStore.Retries = messageStore.Retries > 0 ? messageStore.Retries : messageStore.Retries++ ;
                    return messageStore;
                }
            }
            return null;
        }
        private static PhysicalFileProvider _fileProvider =
    new PhysicalFileProvider(Directory.GetCurrentDirectory());
        private MailMessage ComposeMessage(MessageStore messageStore)
        {
            //var template = $"~/Html/EmailSample.html";
            //string template = _fileProvider.Watch("EmailSample.html");
            //string template = Path.Combine(_hostingEnv.WebRootPath, "html");
            // string template = File.ReadAllText("~/wwwroot/html/EmailSample.html");
           
                   string contentRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
              
            var template = System.IO.File.ReadAllText(contentRootPath + "/EmailSample.html");

            if (string.IsNullOrEmpty(template))
            {
                throw new NotSupportedException("Emtpy Template");
            }

            var splitMessages = messageStore.Message.Split(";??");

            if (splitMessages.Length != 4)
            {
                throw new NotSupportedException("Message Split Index error");
            }

            var mailTemplate = template
                .Replace("{mailHeader}", splitMessages[1])
                .Replace("{greeting}", splitMessages[2])
                .Replace("{messageBody}", splitMessages[3])
                .Replace("{currentYear}", DateTime.Now.Year.ToString());

            var message = new MailMessage()
            {
                Subject = splitMessages[0],
                Body = mailTemplate,
                IsBodyHtml = true,
                From = new MailAddress("wimbigraffles@wimbig.com"),
            };

            if (messageStore.AddressType == AddressType.Bulk)
            {
                foreach (var address in messageStore.EmailAddress.Split(','))
                {
                    message.To.Add(address);
                }
            }
            else if (messageStore.AddressType == AddressType.Single)
            {
                message.To.Add(messageStore.EmailAddress);
            }
            else
            {
                throw new NotSupportedException("AddressType");
            }

            return message;
        }

        private SmtpClient CreateSmtpClient(EmailSetting mailSetting)
        {
    //        "Host": "smtp.gmail.com",
    //"Port": 587,
    //"Ssl": true,
    //"Username": "wimbigraffles@gmail.com",
    ////"Username": "info@wimbig.com",
    ////"Password": "wbslmail@247"
    //"Password": "jinmcever@123"
            return new SmtpClient
            {
                Host = "mail.wimbig.com",
                //Port = 587,
                //EnableSsl = true,
                //UseDefaultCredentials = false,
                //DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(
                        userName: "wimbigraffles@wimbig.com",
                        password: "Admin@123"
                    )
            };
        }

    }
}
