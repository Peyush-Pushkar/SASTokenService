using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SasTokenService.Entities
{
    /// <summary>
    /// Class To hold Response of Web Api
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Getter and Setter for Token .
        /// </summary>
        public string RawToken { get; set; }
        /// <summary>
        /// Getter and Setter for Time Duration
        /// </summary>
        public double ExpiresInSeconds { get; set; }
    }
}