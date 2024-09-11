using Dapper;
using Exwhyzee.Caching;
using Exwhyzee.Data;
using Exwhyzee.Wimbig.Core.MessageStores;
using Hangfire;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Data.Repository.MessageStores
{
    public class MessageStoreRepository : IMessageStoreRepository
    {
        #region Const
        private const string CACHE_MESSAGESTORE = "exwhyzee.wimbig.messageStore";
        private const int CACHE_EXPIRATION_MINUTES = 30;
        #endregion

        #region Fields
        private readonly IStorage _storage;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;
        private readonly EmailSetting _mailSetting;

        private readonly IHostingEnvironment _hostingEnv;
        #endregion

        #region Ctor
        public MessageStoreRepository(IStorage storage, IMemoryCache memoryCache,
          ISignal signal, IClock clock,
          IHostingEnvironment env, IOptions<EmailSetting> optionsEmail)
        {
            _storage = storage;
            _memoryCache = memoryCache;
            _signal = signal;
            _clock = clock;
        }
        #endregion

        public async Task<long> Add(MessageStore entity)
        {
            try
            {
                entity.Message = entity.Message.Replace("0", "O");
                entity.Message = entity.Message.Replace("won", "W=i=N");
                entity.Message = entity.Message.Replace("gmail", "Gma1l");
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                entity = _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spMessageStoreInsert @emailAddress,@phoneNumber,@channel,@type,@addressType,@message,@imageUrl";

                    entity.Id = conn.ExecuteScalar<long>(sql, new
                    {
                        emailAddress = entity.EmailAddress,
                        phoneNumber = entity.PhoneNumber,
                        channel = (int)entity.MessageChannel,
                        type = (int)entity.MessageType,
                        addressType = (int)entity.AddressType,
                        message = entity.Message,
                        imageUrl = entity.ImageUrl


                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_MESSAGESTORE);
                
                
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
            //BackgroundJob.Schedule(() => SendMessageMethod(), TimeSpan.FromTicks(60));
            var msg = SendNow(entity);
            //RecurringJob.AddOrUpdate(recurringJobId: "message-send", methodCall: () => this.SendMessageMethod(), Cron.Minutely);
            return await Task.FromResult(entity.Id);
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
                    await Update(messageStore);
                    return messageStore;

                }
                catch (Exception ex)
                {
                    // Returns False
                    Console.WriteLine($"FATAL ERROR: {ex.Message} \n\nException: {ex.StackTrace}");
                    messageStore.MessageStatus = MessageStatus.Not_Sent;
                    messageStore.Retries++;
                    await Update(messageStore);
                    // messageStore.Retries = messageStore.Retries > 0 ? messageStore.Retries : messageStore.Retries++ ;
                    return messageStore;
                }
            }
            return null;
        }

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
            string imgurl = "";
            if (String.IsNullOrEmpty(messageStore.ImageUrl))
            {
                messageStore.ImageUrl = "https://www.wimbig.com/Barner//201921_123652891.jpg";
            }

            var mailTemplate = template
                .Replace("{mailHeader}", splitMessages[1])
                .Replace("{greeting}", splitMessages[2])
                .Replace("{messageBody}", splitMessages[3])
                .Replace("{currentYear}", DateTime.Now.Year.ToString())
                .Replace("{img}", messageStore.ImageUrl);

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

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MessageStore>> FetchNotificationsToBeSent(MessageStatus messageStatus, MessageChannel messageChannel, int retriesLimit)
        {
            try
            {
                //string cacheKey = $"{CACHE_MESSAGESTORE}.FetchNotificationsToBeSent.{(int)messageStatus}.{retriesLimit}";
                //var messageStores = _memoryCache.GetOrCreate(cacheKey, (entry) =>
                //{
                //entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                //entry.ExpirationTokens.Add(_signal.GetToken(CACHE_MESSAGESTORE));

                var messageStores = _storage.UseConnection(conn =>
               {
                   string sql = $"dbo.spFetchNotificationsToBeSent @messageStatus,@messageChannel,@retries";
                   return conn.Query<MessageStore>(sql, new
                   {
                       messageStatus = 20,
                       messageChannel = (int)messageChannel,
                       retries = retriesLimit
                   }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);
               });
                // });

                return await Task.FromResult(messageStores.AsList());
            }
            catch (Exception ex)
            {
                string message = $"{ex.Message}";
                return null;
            }
        }

       
        public async Task Update(MessageStore entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                _storage.UseConnection(conn =>
                {
                    var sql = $"dbo.spUpdateNotificationStatus @messageStatus,@dateSent,@id,@retries,@response";

                    conn.Execute(sql,
                        new
                        {
                            messageStatus = (int)entity.MessageStatus,
                            dateSent = entity.DateSent,
                            id = entity.Id,
                            retries = entity.Retries,
                            response = entity.Response
                        },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return entity;
                });

                _signal.SignalToken(CACHE_MESSAGESTORE);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex}");
            }
        }

        public async Task<PagedList<MessageStore>> GetAsyncAllMessege(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {

            string cacheKey = $"{CACHE_MESSAGESTORE}.getasyncmessageStore.{status}.{dateStart}.{dateEnd}.{startIndex}.{count}.{searchString}";
            var data = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_MESSAGESTORE));
                return _storage.UseConnection(conn =>
                {
                    string query = $"dbo.spMessageStoreGetAll @status,@dateStart,@dateEnd, @startIndex, @count, @searchString";

                    var result = conn.Query<MessageStore>(query, new
                    {
                        status = status,
                        dateStart = dateStart,
                        dateEnd = dateEnd,
                        startIndex = startIndex,
                        count = count,
                        searchString = searchString,
                    }, commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS);

                    return result;
                });
            });

            var filterCount = data.AsList().Count;
            var paggedResult = new PagedList<MessageStore>(source: data,
                                pageIndex: startIndex,
                                pageSize: count,
                                filteredCount: filterCount,
                                totalCount: filterCount);
            _signal.SignalToken(CACHE_MESSAGESTORE);
            return await Task.FromResult(paggedResult);
        }

        public Task<PagedList<MessageStore>> GetAsync(int? status = null, DateTime? dateStart = null, DateTime? dateEnd = null, int startIndex = 0, int count = int.MaxValue, string searchString = null)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageStore> Get(long id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));

            string cacheKey = $"{CACHE_MESSAGESTORE}.messagestorebyid:{id}";
            var item = _memoryCache.GetOrCreate(cacheKey, (entry) =>
            {
                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_MESSAGESTORE));
                return _storage.UseConnection(conn =>
                {
                    string sql = $"dbo.spMessageStoreGetById @id";
                    return conn.QueryFirstOrDefault<MessageStore>(sql,
                        new { id },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );
                });
            });
           
            return await Task.FromResult(item);
        }

       
    }
}
