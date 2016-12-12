using HwInf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Routing;

namespace HwInf
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
