using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SasTokenService.Entities
{
    /// <summary>
    /// Class to Hold Device Details.
    /// </summary>
    public class DeviceQuery
    {
        /// <summary>
        /// Getter Setter for Id Field of Device.
        /// </summary>
        public long? ID { get; set; }
        /// <summary>
        /// Getter Setter for Name Field of Device.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Getter Setter for uri Field of Device.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Getter Setter for IP  Field of Device.
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Getter Setter for Device key Field of Device.
        /// </summary>
        public string DeviceKey { get; set; }
        /// <summary>
        /// Getter Setter for MaxResults Field of Device.
        /// </summary>
        public int MaxResults { get; set; } = 50;

        public string ProviderID { get; set; }

        public string Owner { get; set; }

        public String GeoPosition { get; set; }

        /// <summary>
        /// Getter and Setter For KeyValidity Field
        /// </summary>
        public DateTime KeyValidity { get; set; }


    }
}