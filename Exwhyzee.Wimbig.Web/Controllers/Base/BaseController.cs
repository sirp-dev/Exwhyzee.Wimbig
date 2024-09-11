using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exwhyzee.Wimbig.Web.Controllers.Base
{
    public class BaseController : Controller
    {
        public static string LoggedInUser { get; set; } = null;
        public static  string LoggedInUserRole { get; set; } = null;

        // include other generalized logic and members here
    }
}
