using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.NotificationService.Abstract;
using Exwhyzee.Wimbig.NotificationService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.NotificationService.Service
{
    public class EmailService : NotificationSender
    {

        private readonly MailSetting _mailSetting;

        public EmailService(IConfiguration config, IOptions<MailSetting> options) : base(config)
        {
            _mailSetting = options.Value;
        }

        public override async Task<MessageStore> Send(MessageStore messageStore)
        {
            try
            {
                Console.WriteLine("Message ID: {0}, Message Channel: {1}, Address: {2}, Retries: {3}",
                               messageStore.Id, messageStore.MessageChannel.ToString(), messageStore.ToString(), messageStore.Retries);

            }
            catch (Exception c)
            {

            }



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
                Console.WriteLine("Email Message Sent");
                return messageStore;

            }
            catch (Exception ex)
            {
                // Returns False
                Console.WriteLine($"FATAL ERROR: {ex.Message} \n\nException: {ex.StackTrace}");
                messageStore.MessageStatus = MessageStatus.Not_Sent;
                messageStore.Retries++;
                // messageStore.Retries = messageStore.Retries > 0 ? messageStore.Retries : messageStore.Retries++ ;
                return messageStore;
            }
        }

        private MailMessage ComposeMessage(MessageStore messageStore)
        {
            var template = Properties.Resources.EmailSample;

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
                From = new MailAddress(_mailSetting.Username),
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

        private SmtpClient CreateSmtpClient(MailSetting mailSetting)
        {
            return new SmtpClient
            {
                Host = mailSetting.Host,
                Port = mailSetting.Port,
                EnableSsl = mailSetting.Ssl,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(
                        userName: mailSetting.Username,
                        password: mailSetting.Password
                    )
            };
        }
    }
}
