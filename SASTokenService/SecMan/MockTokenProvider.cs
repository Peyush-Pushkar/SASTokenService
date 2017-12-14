using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SasTokenService
{
    /// <summary>
    /// This class provides a build-in OAuth token provider.
    /// </summary>
    public class MockTokenProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Method To Grant Authorization to Context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
        {
            return base.GrantAuthorizationCode(context);
        }

        /// <summary>
        /// Method To GrantClientCredentials
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            return base.GrantClientCredentials(context);
        }
        /// <summary>
        /// Method To get GrantCustomExtension
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            return base.GrantCustomExtension(context);
        }
        /// <summary>
        /// Method To Get ValidateClientAuthentication
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.FromResult(context.Validated());

            var scope = context.Parameters["scope"];

            if (scope != null && scope == "ValuesController")
                await Task.FromResult(context.Validated());
            else
                context.SetError("Invalid scope.");
        }

        /// <summary>
        /// Method Validate Password .For Testing Purpose
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (context.Password != "password")
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");

                context.Rejected();
            }

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim("user_name", "context.UserName"));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "context.UserName"));
           
            // Add a Role Claim:
            identity.AddClaim(new Claim(ClaimTypes.Role, "TestRoleFromTokenProvider"));

            context.Validated(identity);

            var t = new Task(()=>{});
            t.Start();
            return t;
        }
    }

}
