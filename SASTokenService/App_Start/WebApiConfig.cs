using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SasTokenService
{
    /// <summary>
    /// Class to Load Web API related configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Method to Load Web Api Configuration
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
