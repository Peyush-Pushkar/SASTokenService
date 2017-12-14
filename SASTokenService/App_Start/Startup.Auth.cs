using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using System.Configuration;
using SasTokenService.KeyAuth.Owin;

namespace SasTokenService
{
    /// <summary>
    /// Class to Load the Devices.
    /// </summary>
    public partial class Startup
    {
    /// <summary>
    /// Method To Validate the User
    /// </summary>
    /// <param name="app"></param>
        public void ConfigureKeyAuth(IAppBuilder app)
        {

            //   app.UseTwitterAuthentication()

            //
            // This setups AAD authentication additionally to device key auth. (see below)
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                { 
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters { ValidAudience = ConfigurationManager.AppSettings["ida:Audience"] },
                    Tenant = ConfigurationManager.AppSettings["ida:Tenant"]
                });

            //
            // Setups the middleware for device key authentication.
            app.UseKeyAuthMiddleware(new KeyAuthOptions(
                //
                // This action is invoked on every request, which is supposed 
                // to be protected by device key authentication.
                // Method should validate the deviceId and deviceKey as
                // credentials. It returns a TRUE if successfully validated.
                (deviceId, deviceKey) => {
                    var validityToken = DeviceKeyGenerator.GetDeviceValidator(deviceId, deviceKey);
                    if (validityToken)
                    {
                        return true;
                    }
                    else
                        return false;
                    
                })
            {
                  // 
                  ProtectedPath = "api/sastoken"
            });
        }

    }
}
