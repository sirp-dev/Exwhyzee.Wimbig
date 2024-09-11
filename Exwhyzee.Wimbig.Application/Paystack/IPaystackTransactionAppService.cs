using Exwhyzee.Wimbig.Core.Paystack;
using Exwhyzee.Wimbig.Core.Paystack.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Application.Paystack
{
    public interface IPaystackTransactionAppService
    {
        Task<PaymentInitalizationResponse> InitializeTransaction(string secretKey,string email, int amount, long transactionId, string firstName = null,
            string lastName = null, string callbackUrl = null, string reference = null, bool makeReferenceUnique = false);

        Task<TransactionResponseModel> VerifyTransaction(string reference, string secretKey);

        // HttpClient CreateClient(string secretKey);
    }
}
