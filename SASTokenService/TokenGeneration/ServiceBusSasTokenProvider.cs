using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SasTokenService.Entities;
using System.Data.SqlClient;
using SasTokenService.Model;

namespace SasTokenService
{
    /// <summary>
    /// Class to Provide Token For Service Bus Service. This Class Provides Service Bus for EVENTHUB,TOPIC and Queue
    /// </summary>
    public class ServiceBusSasTokenProvider : ISasTokenProvider
    {
        /// <summary>
        /// Constant String for Event Hub
        /// </summary>
        public const string EVENTHUB = "EVENTHUB";
        /// <summary>
        /// Constant String for Topic
        /// </summary>
        public const string TOPIC = "TOPIC";
        /// <summary>
        /// Constant String for Queue
        /// </summary>
        public const string QUEUE = "QUEUESB";
        /// <summary>
        /// Constant String for Request Validation Error
        /// </summary>
        public const string RequestValidationError = "Request is missing key values to obtain the Requested Token";
        /// <summary>
        /// Constant String for Https
        /// </summary>
        public const string HTTPS = "http://";
        /// <summary>
        /// Constant String for New Line
        /// </summary>
        public const string Newline = "\n";
      
        /// <summary>
        ///  Method to Get Configuration . Id must be different for each class.
        /// </summary>
        public SasProviderConfiguration Configuration { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return "SB.SAS.01"; }
        }


        /// <summary>
        /// Method to return Description value
        /// </summary>
        public string Description
        {
            get { return "Generates SAS tokens for Microsoft Azure Service Bus."; }
        }



        /// <summary>
        /// This method is to identify which SAS token to  be retieved based on the user input.
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <param name="configuration"></param>
        /// <returns>token of Type Token</returns>
        public Token CreateToken(CreateTokenRequest tokenArgs, SasProviderConfiguration configuration)
        {
            ServiceBusTokenArgs args = getArgs(tokenArgs, configuration);
            var token = new Token();

            if (tokenArgs.TokenIdentifier.ToLower() == EVENTHUB.ToLower())
            {
                token = createTokenforServiceBusExplorerEventHub(args);
            }

            else if (tokenArgs.TokenIdentifier.ToLower() == TOPIC.ToLower())
            {
                token = createTokenforServiceBusExplorerTopic(args);
            }

            else if (tokenArgs.TokenIdentifier.ToLower() == QUEUE.ToLower())
            {
                token = createTokenforServiceBusExplorerQueue(args);
            }

            return token;            
        }

        #region Private Methods

        /// <summary>
        /// Creates ServiceBusTokenArgs from dynamic type.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="configuration"></param>
        /// <returns>tokenArgs of Type ServiceBusTokenArgs</returns>
        private ServiceBusTokenArgs getArgs(CreateTokenRequest request, SasProviderConfiguration configuration)
        {
         

            var tokenArgs = new ServiceBusTokenArgs();
            tokenArgs.Provider = configuration.Values["Provider"];
            tokenArgs.ConnectionString = configuration.Values["ConnectionStringServiceBus"];


            if (request.TokenIdentifier != null)
            {
                String TokenIdentifier = request.TokenIdentifier;
                if (request.TokenIdentifier.ToLower() == EVENTHUB.ToLower())
                {
                    createArgsForEventHub(request, tokenArgs);
                    tokenArgs.KeyName = configuration.Values["keyName_EventHub"];
                }

                if (request.TokenIdentifier.ToLower() == TOPIC.ToLower())
                {
                    createArgsForTopic(request, tokenArgs);
                    tokenArgs.KeyName = configuration.Values["keyName_Topic"];
                }


                if (request.TokenIdentifier.ToLower() == QUEUE.ToLower())
                {
                    createArgsForQueue(request, tokenArgs);
                    tokenArgs.KeyName = configuration.Values["keyName_Queue"];
                }
            }


            return tokenArgs;
        }
        /// <summary>
        /// Creates ServiceBusTokenArgs object for Topic
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tokenArgs"></param>
        private static void createArgsForTopic(CreateTokenRequest request, ServiceBusTokenArgs tokenArgs)
        {
            if (request.SbNamespace != null)
            {
                tokenArgs.SbNamespace = request.SbNamespace;
            }
            else
                throw new SasTokenServiceException(RequestValidationError);

            if (request.Path != null )
            {
                tokenArgs.Path = request.Path;
            }

            else
                throw new SasTokenServiceException(RequestValidationError);
            if (request.EntityType != null)
            {
                tokenArgs.EntityType = request.EntityType;
            }
            else throw new SasTokenServiceException(RequestValidationError);


        }
        /// <summary>
        /// Creates ServiceBusTokenArgs object for  EventHub
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tokenArgs"></param>
        private static void createArgsForEventHub(CreateTokenRequest request, ServiceBusTokenArgs tokenArgs)
        {
            if (request.SbNamespace != null)
            {
                tokenArgs.SbNamespace = request.SbNamespace;
            }
            else throw new SasTokenServiceException(RequestValidationError);

            if (request.Path != null)
            {
                tokenArgs.Path = request.Path;
            }
            else throw new SasTokenServiceException(RequestValidationError);


            if (request.EntityType != null)
            {
                tokenArgs.EntityType = request.EntityType;
            }
            else throw new SasTokenServiceException(RequestValidationError);
        }
        /// <summary>
        /// Creates ServiceBusTokenArgs object for Queue
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tokenArgs"></param>
        private static void createArgsForQueue(CreateTokenRequest request, ServiceBusTokenArgs tokenArgs)
        {
            if (request.SbNamespace != null)
            {
                tokenArgs.SbNamespace = request.SbNamespace;
            }
            else
                throw new SasTokenServiceException(RequestValidationError);

            if (request.Path != null)
            {
                tokenArgs.Path = request.Path;
            }
            else throw new SasTokenServiceException(RequestValidationError);


            if (request.EntityType != null)
            {
                tokenArgs.EntityType = request.EntityType;
            }
            else
                throw new SasTokenServiceException(RequestValidationError);

        }


        /// <summary>
        /// Method to create token for ServiceBusExplorerEventHub
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <returns>rawtokwn of Type Token</returns>
        private Token createTokenforServiceBusExplorerEventHub(ServiceBusTokenArgs tokenArgs)
        {
            
            var uri = HTTPS + tokenArgs.SbNamespace + tokenArgs.Provider + tokenArgs.Path;

            var key = getKeyFromSB(tokenArgs);

            var ts = TimeSpan.FromMinutes(500);//TODO. THis must be entered by user when click on create.

            var rawToken = createToken(uri, tokenArgs.KeyName, key, ts);

            return new Token()
            {
                ExpiresInSeconds = ts.TotalSeconds,
                RawToken = rawToken
            };
        }


        /// <summary>
        /// Method to create Token for ServiceBus Topic
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <returns>Rawtoken of Type Token</returns>
        private Token createTokenforServiceBusExplorerTopic(ServiceBusTokenArgs tokenArgs)
        {
            var uri = HTTPS + tokenArgs.SbNamespace + tokenArgs.Provider + tokenArgs.Path;

            var key = getKeyFromSB(tokenArgs);

            var ts = TimeSpan.FromMinutes(500);//TODO. This must be entered by user when click on create.

            var rawToken = createToken(uri, tokenArgs.KeyName, key, ts);

            return new Token()
            {
                ExpiresInSeconds = ts.TotalSeconds,
                RawToken = rawToken
            };
        }


        /// <summary>
        /// Method to create Token for ServiceBus Queue
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <returns>Rawtoken of Type Token</returns>
        private Token createTokenforServiceBusExplorerQueue(ServiceBusTokenArgs tokenArgs)
        {
            
            var uri = HTTPS + tokenArgs.SbNamespace + tokenArgs.Provider + tokenArgs.Path;

            var key = getKeyFromSB(tokenArgs);

            var ts = TimeSpan.FromMinutes(500);//TODO. THis must be entered by user when click on create.

            var rawToken = createToken(uri, tokenArgs.KeyName, key, ts);

            return new Token()
            {
                ExpiresInSeconds = ts.TotalSeconds,
                RawToken = rawToken
            };
        }


        /// <summary>
        /// method to Get Primary key fot Various EntityType, eg: Queue, Service bus , Event hub
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <returns>primarykey of Type String</returns>
        public string getKeyFromSB(ServiceBusTokenArgs tokenArgs)
        {
            string primarykey = String.Empty;
            string m_ConnStr = tokenArgs.ConnectionString;
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(m_ConnStr);
            if (tokenArgs.EntityType.ToLower() == EVENTHUB.ToLower())
            {
                var ehDescEvnetHub = namespaceManager.GetEventHub(tokenArgs.Path);
                foreach (var rule in ehDescEvnetHub.Authorization)
                {
                    SharedAccessAuthorizationRule r = rule as SharedAccessAuthorizationRule;
                    if (r != null && r.KeyName.ToLower() == tokenArgs.KeyName.ToLower())
                    {
                        primarykey = r.PrimaryKey;
                    }

                }

            }
            
            else if (tokenArgs.EntityType.ToLower() == TOPIC.ToLower())
            {
                var ehDescTopic = namespaceManager.GetTopic(tokenArgs.Path);
                foreach (var rule in ehDescTopic.Authorization)
                {

                    SharedAccessAuthorizationRule r = rule as SharedAccessAuthorizationRule;
                    if (r != null && r.KeyName.ToLower() == tokenArgs.KeyName.ToLower())
                    {
                        primarykey = r.PrimaryKey;
                    }
                }
            }

            else if (tokenArgs.EntityType.ToLower() == QUEUE.ToLower())
            {
                var ehDescQueuesSB = namespaceManager.GetQueue(tokenArgs.Path);
                foreach (var rule in ehDescQueuesSB.Authorization)
                {
                    SharedAccessAuthorizationRule r = rule as SharedAccessAuthorizationRule;
                    if (r != null && r.KeyName.ToLower() == tokenArgs.KeyName.ToLower())
                    {
                        primarykey = r.PrimaryKey;
                    }

                }
            }


            return primarykey;
            throw new SasTokenServiceException(String.Format("The '{0}' of entity '{2}' cannot be found on '{2}",
                tokenArgs.KeyName, tokenArgs.EntityType, tokenArgs.Path));
        }

        /// <summary>
        /// Returns the token 
        /// </summary>
        /// <param name="resourceUri"></param>
        /// <param name="keyName"></param>
        /// <param name="key"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        private static string createToken(string resourceUri, string keyName, string key, TimeSpan expiresIn)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + expiresIn.TotalSeconds); //EXPIRES in 1h 
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + Newline + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));

            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture,
            "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);

            return sasToken;
        }
        #endregion
    }
}