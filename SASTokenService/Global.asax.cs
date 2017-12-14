using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace SasTokenService
{
    /// <summary>
    /// Class To Initialize the Application
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Method to Start the Application
        /// </summary>
        protected void Application_Start()
        {
           // Database.SetInitializer<TodoListServiceContext>(new TodoListServiceInitializer());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
