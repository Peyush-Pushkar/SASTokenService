using System;
using SasTokenService.Entities;
using SasTokenService.Model;

namespace SasTokenService
{
    /// <summary>
    /// Interface to define methods an properties of token Providers.
    /// </summary>
    public interface ISasTokenProvider
    {
        /// <summary>
        /// Holds the configuration for provider.
        /// </summary>
        SasProviderConfiguration Configuration { get; set; }

        /// <summary>
        /// Provider identifier. We can use multiple instances of the same provider.
        /// Every provider instance uses different configuration. Id specifies a mapping between
        /// provider and configuration.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Provider Description
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Method To create Token.
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Token CreateToken(CreateTokenRequest tokenArgs, SasProviderConfiguration configuration);
        

    }
}
