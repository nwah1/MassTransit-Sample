using Demo.Model;
using MassTransit;
using MassTransit.Util;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Demo.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IBusControl BusControl { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var rabbitMqRootUri = new Uri(Consts.RabbitMqAddress);

            if (BusControl == null)
            {
                BusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
                {
                    rabbit.Host(rabbitMqRootUri, settings =>
                    {
                        settings.Username(Consts.User);
                        settings.Password(Consts.Pass);
                    });
                });
            }

            TaskUtil.Await(() => BusControl.StartAsync());
        }
    }
}
