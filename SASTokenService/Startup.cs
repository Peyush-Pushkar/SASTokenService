using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SasTokenService.Startup))]

namespace SasTokenService
{
    public partial class Startup
    {
        /// <summary>
        /// Method To Load Configuration
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
          
           // ConfigureOAuthMockAuthentication(app);
            ConfigureKeyAuth(app);
            //ConfigureSecManAuthorization(app);
        }
    }
}
