using Daenet.SecurityManager.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SasTokenService.Entities;
using System.Web.Http.Cors;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using SasTokenService.Model;

namespace SasTokenService
{

    /// <summary>
    /// Implements the WebAPI controller, which can creates various SAS tokens.
    /// This controller does not create token by itself. It uses implementations if <see cref="ISasTokenProvider"/> interfaces.
    /// </summary>
    [RoutePrefix("api/SasToken")]
    public class SasTokenController : ApiController
    {
        private static List<ISasTokenProvider> m_TokenProviders = new List<ISasTokenProvider>();

        private static List<SasProviderConfiguration> m_Configs;
        private void traceError(string msg, Exception ex)
        {
            Debug.WriteLine($"{msg}:{ex}");
        }

        /// <summary>
        /// In this version we do not support full-loader for providers.
        /// Right now we load providers dynamically from configuration.
        /// </summary>
        static SasTokenController()
        {
            loadConfigFromFile();
            // loadConfigFromCode();

            buildTokenPoviders();
        }

        private static void loadConfigFromFile()
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                var cfgString = File.ReadAllText(filePath);
                m_Configs = JsonConvert.DeserializeObject<List<SasProviderConfiguration>>(cfgString);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void loadConfigFromCode()
        {
            try
            {
                m_Configs = new List<SasProviderConfiguration>()
            {
                new SasProviderConfiguration()
                {
                    ProviderId = "STORAGE.SAS.01",

                     ProviderQN = typeof(StorageSasTokenProvider).AssemblyQualifiedName,

                Mappings = new Dictionary<string, SasTokenService.Mapping>
                     {
                         { "AccountName", new Mapping { MapFrom = "VirtualAccountName", MapTo = "AccountName" } },
                     },

                     Values = new Dictionary<string, string>
                     {
                         { "AccountKey_TableStorage"  , "nASwjTjH7Bym4hSVmi9bV3L9Bp9TZAjoYyokJ03wFCgBOqm0zvfuv+dcZRZmviFqUtP7yxqWD9eMJYO9ndcJ/A==" },
                         {"AccountKey_QueueStorage","dOvi3s5XAuUK78jWm6rsOpCaBYC3fBK1Ei6yJg8afKA="},
                         {"ConnectionStringBloB","DefaultEndpointsProtocol=https;AccountName=beststudents2;AccountKey=nASwjTjH7Bym4hSVmi9bV3L9Bp9TZAjoYyokJ03wFCgBOqm0zvfuv+dcZRZmviFqUtP7yxqWD9eMJYO9ndcJ/A==" }

                     },
                },

                new SasProviderConfiguration()
                {
                    ProviderQN = typeof(ServiceBusSasTokenProvider).AssemblyQualifiedName,

                    ProviderId = "SB.SAS.01",

                    Mappings = new Dictionary<string, SasTokenService.Mapping>
                     {
                         { "AccountName", new Mapping { MapFrom = "VirtualAccountName", MapTo = "AccountName" } },
                     },

                     Values = new Dictionary<string, string>
                     {
                         {"ConnectionStringServiceBus", "Endpoint=sb://iotlab2.servicebus.windows.net/;SharedAccessKeyName=sas;SharedAccessKey=dOvi3s5XAuUK78jWm6rsOpCaBYC3fBK1Ei6yJg8afKA=" },
                         {"keyName_Topic","can_receive" },
                         {"keyName_Queue","can_read_send" },
                         {"keyName_EventHub","sas_ehub_key" },
                         {"Provider", ".servicebus.windows.net / " }
                     },
                },

                  new SasProviderConfiguration()
                {
                    ProviderQN = typeof(IoTHubSasTokenProvider).AssemblyQualifiedName,

                    ProviderId = "IoTHub.SAS.01",
                     Mappings = new Dictionary<string, SasTokenService.Mapping>
                     {
                         { "AccountName", new Mapping { MapFrom = "VirtualAccountName", MapTo = "AccountName" } },
                     },

                     Values = new Dictionary<string, string>
                     {
                          {"IoTHubUri",  "protocoladapterhub.azure-devices.net" },
                         {"DeviceKey", "ASnh6qSwCz0iOsO4Q1mvnfnOzn3u/jkNDHBUNbDoZ2E=" }

                     },
                },
            };

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while getting Account Details" + ex);


            }
        }

        /// <summary>
        /// Creates the instance of different Type of Providers
        /// </summary>
        private static void buildTokenPoviders()
        {
            foreach (var cfg in m_Configs)
            {
                var tp = Type.GetType(cfg.ProviderQN);

                // null check before using the type
                if (tp != null)
                {
                    ISasTokenProvider provider = Activator.CreateInstance(tp) as ISasTokenProvider;
                    provider.Configuration = cfg;
                    m_TokenProviders.Add(provider);
                }
            }
        }



        /// <summary>
        /// Returns the list of all installed token providers.
        /// </summary>
        /// <returns></returns>
        // GET: api/SasToken
        [HttpGet]
        public IEnumerable<InstalledProvider> GetTokenProviders()
        {
            try
            {
                List<InstalledProvider> installedProviders = new List<InstalledProvider>();

                foreach (var prov in m_TokenProviders)
                {
                    InstalledProvider p = new InstalledProvider()
                    {
                        Description = prov.Description,
                        Id = prov.Id
                    };
                    installedProviders.Add(p);
                }

                return installedProviders.AsEnumerable();
            }
            catch (Exception ex)
            {
                traceError("Error While Getting Token.", ex);
                return null;

            }

        }


        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="request">Request which contains all required data for token generation.</param>
        /// <returns>Token value.</returns>
        [HttpPost]
        [Route("create")]
        [Authorize()]
        public Token CreateToken(CreateTokenRequest request)
        {
            try
            {
                var providerId = request.ProviderId;
                var token = new Token();

                var provider = m_TokenProviders.FirstOrDefault(x => x.Id == providerId);
                SasProviderConfiguration configuration = provider.Configuration;
                token = provider.CreateToken(request, configuration);
                if (token == null)
                    throw new SasTokenServiceException("Error While Fetching Token");
                else
                    return token;
            }
            catch (Exception ex)
            {

                traceError("An Error occured while creating the token", ex);
                return null;

            }
        }

    }
}
