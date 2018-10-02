using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Blockchaninaccounts
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()  
        {
            Application["users"] = 0;
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
       
        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["users"] = (int)Application["users"] + 1;
            Application.UnLock();
        }
        protected void Session_Stop(object sender, EventArgs e)
        {
            Application.Lock();
            Application["users"] = (int)Application["users"] - 1;
            Application.UnLock();
            Session.Abandon();
        }
        protected void Application_Stop()
        {

        }
    }
}
