using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.EventNotification;

namespace Mobius.EventNotification.Logging
{
    internal enum Logger
    { 
        Database,
        TextFile,
        Event
    }

    /// <summary>
    /// Provides the ability to log an error.
    /// </summary>    
    internal static class ErrorLogger
    {

        /// <summary>
        /// Logs the error information.
        /// </summary>
        /// <param name="applicationName">The name of the application or component the experienced the error.</param>
        /// <param name="methodName">The method name of the application or component.</param>
        /// <param name="message">The error message.</param>
        internal static void Log(EventLog eventLog, Logger logger)
        {

        }
    }
       
}
