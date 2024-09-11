using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Exwhyzee.Wimbig.Notification.Web.Controllers
{
    public class MessageController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Error(string errorId)
        //{
        //    var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        //    if (exceptionFeature != null)
        //    {
        //        Log.Error(exceptionFeature.Error, exceptionFeature.Error.Message);
        //    }
        //}

    }
}