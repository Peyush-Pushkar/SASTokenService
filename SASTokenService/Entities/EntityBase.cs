using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SasTokenService.Entities
{
    /// <summary>
    /// Class to Store Genric Parameters
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Getter and Setter for CreatedAt Field of Type DateTime
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Getter and Setter for CreatedAt Field of Type DateTime
        /// </summary>
        
        public DateTime ChangedAt { get; set; }
    }
}