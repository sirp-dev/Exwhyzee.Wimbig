using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Tickets.Dtos;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Transactions.Dto;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Views.Shared.ViewComponents
{
    public class SmsBalanceViewComponent : ViewComponent
    {
        
       
   
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await GetWinnerAsync();
            TempData["balance"] = item;
            return View();
        }



        private async Task<string> GetWinnerAsync()
        {
            try
            {
                var url = "https://account.kudisms.net/api/?username=wimbigraffles@gmail.com&password=wbsl@123&action=balance";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Timeout = 25000;

                //getting the respounce from the request
                HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string response = await streamReader.ReadToEndAsync();

                return response.Substring(12, 9);

            }
            catch (Exception e)
            {
                return "";
            }
           
        }
       
    }
}
