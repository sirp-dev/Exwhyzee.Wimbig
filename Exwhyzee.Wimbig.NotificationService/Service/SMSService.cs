using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.NotificationService.Abstract;
using Exwhyzee.Wimbig.NotificationService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RestClientDotNet;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace Exwhyzee.Wimbig.NotificationService.Service
{
    public class SMSService : NotificationSender
    {
        private readonly SMSSetting _setting;
        private readonly HttpClient _httpClient;

        public SMSService(IConfiguration config, IOptions<SMSSetting> options, HttpClient httpClient)
            : base(config)
        {
            _setting = options.Value;
            _httpClient = httpClient;
        }

        public override async Task<MessageStore> Send(MessageStore messageStore)
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
                //   entity.Message = entity.Message.Replace();
                var api = "http://account.kudisms.net/api/?username=" + _setting.Username + "&password=" + _setting.Password + "&message=@@message@@&sender=@@sender@@&mobiles=@@recipient@@";
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
                messageStore.Retries = messageStore.Retries+1;
                messageStore.Response = response;


                //infibip
                //string username = "Exwhyzee";
                //string password = "XyZ*2019SMS";

                //byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
                //string header = System.Convert.ToBase64String(concatenated);

                //var client = new RestSharp.RestClient("https://km6me.api.infobip.com");

                //var request = new RestRequest(Method.POST);
                //request.AddHeader("accept", "application/json");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("authorization", header);
                //request.AddParameter("application/json", "{\"from\":\"WimBig\", \"to\":[  \"+2348165680904\",\"+2348079328532\"],\"text\":\"Test SMS.\"}", ParameterType.RequestBody);

                //IRestResponse response = client.Execute(request);


                //if (response.IsSuccessful)
                //{
                //    //var responseString = await response;
                //    Console.WriteLine("HTTP Response Messgae: {0}", response.ToString());
                //    //if (!responseString.Contains("error", StringComparison.OrdinalIgnoreCase))
                //    //{
                //        Console.WriteLine("SMS Message Sent");
                //        messageStore.DateSent = DateTime.UtcNow.AddHours(1);
                //        messageStore.MessageStatus = MessageStatus.Sent;
                //        messageStore.Retries = 0;
                //        return messageStore;


                //}
                //Console.WriteLine("SMS Message not sent");
                //Console.WriteLine("HTTP Response Messgae: {0}", response.ToString());
                //messageStore.DateSent = null;
                //messageStore.MessageStatus = MessageStatus.Not_Sent;
                //messageStore.Retries = messageStore.Retries > 0 ? messageStore.Retries : messageStore.Retries++;


                //twillo
                //const string accountSid = "AC06d4de856b4a83b0fa924de5c10f5886";
                //const string authToken = "847fe0e788e359e8d1bd4dd4213186b2";

                //TwilioClient.Init(accountSid, authToken);
                ////formatr numbers

                //string phone = "";
                //if (messageStore.PhoneNumber.StartsWith("0"))
                //{
                //    phone = messageStore.PhoneNumber.Substring(1);
                //    phone = "+234" + phone;
                //    messageStore.PhoneNumber = phone;
                //}

                //var message = MessageResource.Create(
                //    body: messageStore.Message,
                //    from: new Twilio.Types.PhoneNumber("+13174492801"),
                //    to: new Twilio.Types.PhoneNumber("+2348037915777")
                //);
                //if (message.Sid != null)
                //{
                //    Console.WriteLine("SMS Message Sent");
                //    messageStore.DateSent = DateTime.UtcNow.AddHours(1);
                //    messageStore.MessageStatus = MessageStatus.Sent;
                //    messageStore.Retries = 0;
                //    return messageStore;
                //}
                //else
                //{
                //    Console.WriteLine("SMS Message not sent");
                //    messageStore.DateSent = null;
                //    messageStore.MessageStatus = MessageStatus.Not_Sent;
                //    messageStore.Retries = messageStore.Retries > 0 ? messageStore.Retries : messageStore.Retries++;

                //    return messageStore;
                //}




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
    }
}

