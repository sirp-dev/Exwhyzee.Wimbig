﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core;
using Exwhyzee.Wimbig.Core.Paystack;
using Exwhyzee.Wimbig.Core.Paystack.Models;
using Newtonsoft.Json;

namespace Exwhyzee.Wimbig.Application.Paystack
{
    public class PaystackTransactionAppService : IPaystackTransactionAppService
    {
        [JsonProperty("custom_fields")]
        public IList<CustomField> CustomFields { get; set; }

        public HttpClient CreateClient(string secretKey)
        {
            ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new HttpClient()
            {
                BaseAddress = new Uri(AppConstants.PayStackBaseEndPoint)
            };

            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(AppConstants.ContentTypeHeaderJson));

            client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AppConstants.AuthorizationHeaderType, secretKey);

            return client;
        }

        public async Task<PaymentInitalizationResponse> InitializeTransaction(string secretKey, string email, int amount,long transactionId, string firstName = null, 
            string lastName = null, string callbackUrl = null, string reference = null, bool makeReferenceUnique = false)
        {
            var client = CreateClient(secretKey);

            CustomFields = new List<CustomField>();

            var refId = new CustomField("Transaction Id", "transaction_id", transactionId.ToString());
            var test = new CustomField("Cart Item", "cart_item", "Money");
            CustomFields.Add(refId);
            CustomFields.Add(test);

            //var bodyKeyValues = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("email", email),
            //    new KeyValuePair<string, string>("amount", amount.ToString()),
            //    new KeyValuePair<string, string>("transaction-id", transactionId.ToString())
            //};

            var paystack = new PaystackRequest
            {
                Amount = amount,
                Email = email,
                MetaData = new Core.Paystack.Metadata {
                    Referrer = transactionId.ToString(), CustomFields =  CustomFields, 
            }
        };

            //Optional Params

            //if (!string.IsNullOrWhiteSpace(firstName))
            //{
            //    bodyKeyValues.Add(new KeyValuePair<string, string>("first_name", firstName));
            //}
            //if (!string.IsNullOrWhiteSpace(lastName))
            //{
            //    bodyKeyValues.Add(new KeyValuePair<string, string>("last_name", lastName));
            //}

            //if (!string.IsNullOrWhiteSpace(callbackUrl))
            //{
            //    bodyKeyValues.Add(new KeyValuePair<string, string>("callback_url", callbackUrl));
            //}

            //var formContent = new FormUrlEncodedContent(bodyKeyValues);

            var formContent = JsonConvert.SerializeObject(paystack);

            var stringContent = new StringContent(formContent, UnicodeEncoding.UTF8, "application/json");
            var response = await client.PostAsync("transaction/initialize", stringContent);

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PaymentInitalizationResponse>(json);

        }

        public async Task<TransactionResponseModel> VerifyTransaction(string reference, string secretKey)
        {
            var client = CreateClient(secretKey);
            var response = await client.GetAsync($"transaction/verify/{reference}");

            var json = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<TransactionResponseModel>(json);
        }
    }

}
