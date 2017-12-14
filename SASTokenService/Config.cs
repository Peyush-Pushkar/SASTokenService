using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SasTokenService
{
    /// <summary>
    /// Class To Hold Deatils for Configuration Details
    /// </summary>
    public class SasProviderConfiguration
    {
        /// <summary>
        /// Provider qualified name.
        /// </summary>
        public string ProviderQN { get; set; }
        /// <summary>
        /// Getter and Setter for Provided Id
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// List of values.
        /// </summary>
        public Dictionary<string, string> Values { get; set; }

        /// <summary>
        /// List of mappings to values.
        /// </summary>
        public Dictionary<string, Mapping> Mappings { get; set; }

    }

    /// <summary>
    /// Defines how to map sensitive data.
    /// For example Topic KeyName 'CAN_SEND' could be mapped as:
    /// MapFrom: 'BLAKEY' to MapTo: 'CAN_SEND'.
    /// Client would send 'BLA_KEY', which does not exist in the service.
    /// SAS TokenService would map BLA_KEY to CAN_SEND key, which exists in Event Hub.
    /// </summary>
    public class Mapping
    {
        /// <summary>
        ///  Getter and Setter for MapFrom
        /// </summary>
        public string MapFrom { get; set; }
        /// <summary>
        /// Getter and Setter for MapTo
        /// </summary>
        public string MapTo { get; set; }
    }
}