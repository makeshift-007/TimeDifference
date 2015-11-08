using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using TimeDifference.Services.Filters;

namespace TimeDifference.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            //Enabling Cors
            config.EnableCors();

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //Added by Me
            //config.Filters.Add(new AuthorizeAttribute());
            //config.Filters.Add(new ValidationActionFilter());

            //GlobalConfiguration.Configuration.Filters.Add(new ApiAuthenticationFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();



            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

    }
}
