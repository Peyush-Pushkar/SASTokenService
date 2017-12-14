using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasTokenService.KeyAuth.Owin
{
    /// <summary>
    /// Class To Validate Headers in Requets.
    /// </summary>
    public class KeyAuthOptions
    {
        /// <summary>
        /// Method to Validate Headers . 
        /// </summary>
        /// <param name="onValidateHeader"></param>
        public KeyAuthOptions(Func<long, string, bool> onValidateHeader)
        {
            if (onValidateHeader == null)
                throw new Exception("onValidateHeader must be set.");

            this.ErrorOnMissingHeader = @"{'Message':'Authorization has been denied for this request.'}";
            this.AuthHeaderName = "DeviceCredentials";
            this.OnValidateHeader = onValidateHeader;
        }

        /// <summary>
        /// Getter For AuthHeaderName field of type String
        /// </summary>
        public string AuthHeaderName { get; internal set; }

        /// <summary>
        /// Getter For ErrorOnMissingHeader field of type String
        /// </summary>
        public string ErrorOnMissingHeader { get; internal set; }

        /// <summary>
        /// Getter Setter for OnValidateHeader method
        /// </summary>
        public Func<long, string, bool> OnValidateHeader { get; set; }
        /// <summary>
        ///  /// <summary>
        /// Getter For ProtectedPath field of type String
        /// </summary>
        /// </summary>
        public string ProtectedPath { get; internal set; }
    }
}
