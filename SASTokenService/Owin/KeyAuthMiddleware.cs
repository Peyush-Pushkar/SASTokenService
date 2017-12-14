using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SasTokenService.KeyAuth.Owin
{
    /// <summary>
    /// Class to provide validation Methods For Password
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Installs the <see coderef="SecManAuthorizationMiddleware"/> in the OWIN pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static void UseKeyAuthMiddleware(this IAppBuilder app, KeyAuthOptions options = null)
        {
            if (options == null)
                throw new ArgumentException("SecManMdwOptions must be specified!");

            app.Use<KeyAuthMiddleware>(options);
        }
    }


    /// <summary>
    /// MiddleWare for authenticating the given key for creating sastoken
    /// </summary>
    public class KeyAuthMiddleware : OwinMiddleware
    {
        /// <summary>
        /// We store in owin pipeline under this key all required authorization options.
        /// </summary>
        internal const string OptionsKey = "KeyAuthMdw.Options";

        private KeyAuthOptions m_Opts;

        private OwinMiddleware m_Next;

        /// <summary>
        /// Creates the middleware instance.
        /// </summary>
        /// <param name="next">Nex middleware in chain.</param>
        /// <param name="opts"></param>
        public KeyAuthMiddleware(OwinMiddleware next, KeyAuthOptions opts)
            : base(next)
        {
            m_Opts = opts;
            m_Next = next;
        }

        /// <summary>
        /// Setups the authorization settings relevant for Security Manager.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Method != "OPTIONS")
            {
                if (m_Opts.ProtectedPath != null && context.Request.Uri.AbsoluteUri.ToLower().Contains(m_Opts.ProtectedPath))
                {
                    if (context.Environment.ContainsKey(OptionsKey) == false)
                        context.Environment.Add(OptionsKey, m_Opts);

                    if (context.Authentication.User != null && !(context.Authentication.User.Identity is GenericIdentity))
                    {
                        if (context.Authentication.User.Identity.AuthenticationType != "device")
                        {
                            if (!context.Request.Headers.ContainsKey(m_Opts.AuthHeaderName))
                            {
                                await context.Response.WriteAsync(m_Opts.ErrorOnMissingHeader);
                                context.Response.StatusCode = 401;
                                return;
                            }
                            else
                            {
                                var hVal = context.Request.Headers[m_Opts.AuthHeaderName];
                                var tokens = hVal.Split(',');

                                if (tokens.Length != 2)
                                {
                                    await context.Response.WriteAsync("Invalid token format.");
                                    context.Response.StatusCode = 401;
                                    return;
                                }

                                long id;

                                if (long.TryParse(tokens[0], out id))
                                {
                                    if (this.m_Opts.OnValidateHeader.Invoke(id, tokens[1]))
                                    {
                                        GenericIdentity identity = new GenericIdentity("device", "device");
                                        context.Authentication.User = new System.Security.Claims.ClaimsPrincipal(identity);
                                    }
                                    else
                                    {
                                        await context.Response.WriteAsync("Invalid DeviceKey or DeviceId");
                                        context.Response.StatusCode = 401;
                                        return;
                                    }
                                }
                                else
                                {
                                    await context.Response.WriteAsync("Invalid deviceId in token.");
                                    context.Response.StatusCode = 401;
                                    return;
                                }
                            }


                        }
                    }
                }
            }

            await m_Next.Invoke(context);
        }
    }

}
