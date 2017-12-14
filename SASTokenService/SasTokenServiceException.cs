using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SasTokenService
{
    /// <summary>
    ///  Class To Handle Exception in Service. Currently is only logs the Exception.  
    /// </summary>
    public class SasTokenServiceException : Exception
    {
        /// <summary>
        ///  Method To logs Exception
        /// </summary>
        /// <param name="msg"></param>
        public SasTokenServiceException(string msg) : base(msg)
        {
            Debug.WriteLine($"{msg}");

        }
    }
}