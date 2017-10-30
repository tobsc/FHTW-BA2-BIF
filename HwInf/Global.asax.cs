using HwInf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.Common.Interfaces;

namespace HwInf
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();        }

        protected void Application_Start()
        {
            PdfSharp.Fonts.OpenType.FontDataConfig.ResourceAssembly = typeof(WebApiApplication).Assembly;
            log4net.Config.XmlConfigurator.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Autofac, Dependency Injection
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<BusinessLayer>().As<IBusinessLayer>();
            builder.RegisterType<HwInfContext>().As<IDataAccessLayer>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
