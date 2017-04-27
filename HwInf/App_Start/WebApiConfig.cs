using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace HwInf
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiWithAction", 
                routeTemplate: "api/{controller}/{action}"
                );

            config.Routes.MapHttpRoute(
                name: "Print",
                routeTemplate: "{controller}/{action}"
                );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.MessageHandlers.Add(new AuthHandler());
        }
    }
}
