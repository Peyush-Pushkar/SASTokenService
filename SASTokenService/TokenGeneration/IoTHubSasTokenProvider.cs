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
    /// Class to Provide Token For Iot Token
    /// </summary>

    public class IoTHubSasTokenProvider : ISasTokenProvider
    {
        /// <summary>
        /// Getter Setter for Configuration of Provider class.
        /// </summary>
        public SasProviderConfiguration Configuration { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return this.Configuration.ProviderId; }
        }

        /// <summary>
        /// Method to return Description value
        /// </summary>
        public string Description
        {
            get { return "Generates SAS tokens for Microsoft IOT Hub."; }
        }





        /// <summary>
        /// This method is to create SAS token to  be retieved based on the user input.
        /// </summary>
        /// <param name="tokenArgs"></param>
        /// <param name="configuration"></param>
        /// <returns>token of Type Token</returns>

        public Token CreateToken(CreateTokenRequest tokenArgs, SasProviderConfiguration configuration)
        {
            IoTHubTokenArgs args = getArgs(tokenArgs, configuration);

            var sasToken = createIotHubToken(args);

            return new Entities.Token() { RawToken = sasToken, ExpiresInSeconds = args.Duration.TotalSeconds };
        }

        private IoTHubTokenArgs getArgs(CreateTokenRequest tokenArgs, SasProviderConfiguration configuration)
        {
            return new IoTHubTokenArgs()
            {

                IoTHubUri = configuration.Values["IoTHubUri"],
                DeviceName = tokenArgs.DeviceName,
                DeviceKey = configuration.Values["DeviceKey"],
                Duration = TimeSpan.FromDays(1)
            };
        }

        private static string createIotHubToken(IoTHubTokenArgs args)
        {
            string resourceUri = $"{args.IoTHubUri}/devices/{args.DeviceName}";

            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + args.Duration.TotalSeconds);
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(args.DeviceKey));

            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture,
            "sr={0}&sig={1}&se={2}",
                HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry);

            return sasToken;
        }
    }
}