 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blockchaninaccounts.Controllers.Dashboard
{
    public class BaseController : Controller
    {
        public static Guid  session { get; set; }
        protected string SessionToken
        {
            get
            {

                var token = SessionTokenGenerationcs.session;

                return token != null ? token : null;

            }
        }
        // GET: Base


    }
}