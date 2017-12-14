using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SasTokenService.Model
{
    /// <summary>
    /// Class To hold Details of Token Request. 
    /// </summary>
    public class CreateTokenRequest
    {

        #region -----Generic Fields------
        /// <summary>
        /// Getter Setter for ProviderId
        /// </summary>
        public String ProviderId { get; set; }
        /// <summary>
        /// Getter Setter for TokenIdentifier
        /// </summary>

        public String TokenIdentifier { get; set; }
        #endregion


        #region -----Storage Fields------
        /// <summary>
        /// Getter Setter for AccountName
        /// </summary>
        public String AccountName { get; set; }
        /// <summary>
        /// Getter Setter for Name
        /// </summary>
        public String Name { get; set; }
        #endregion
        #region -----ServiceBus Fields------
        /// <summary>
        /// Getter Setter for EntityType
        /// </summary>
        public String EntityType { get; set; }
        /// <summary>
        /// Getter Setter for Service bus Namespace
        /// </summary>
        public String SbNamespace { get; set; }
        /// <summary>
        /// Getter Setter for Path
        /// </summary>
        public String Path { get; set; }
        #endregion

        #region -----IOT  Fields------
        /// <summary>
        /// Getter Setter for DeviceName
        /// </summary>
        public string DeviceName { get; set; }
        #endregion


    }
}