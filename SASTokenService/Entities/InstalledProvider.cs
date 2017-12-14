using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SasTokenService.Entities
{
    /// <summary>
    /// Class to Hold Details Of all Avaialable Providers.
    /// </summary>
    public class InstalledProvider
    {
        /// <summary>
        /// Getter and Settter for ID Field oF Provider.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Getter and Settter for Description Field oF Provider.
        /// </summary>
        public string Description { get; set; }
    }
}