using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JobsityChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string hostname = ConfigurationManager.AppSettings["hostname"];
            string rabbitUsername = ConfigurationManager.AppSettings["username"];
            string rabbitPassword = ConfigurationManager.AppSettings["password"];
            RabbitMQ.StartConsumer(hostname, rabbitUsername, rabbitPassword);
        }
    }
}
