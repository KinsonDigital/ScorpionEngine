using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Handles exceptions that are thrown by displaying them to the user and 
    /// logging them for later inspection.
    /// </summary>
    public static class ExceptionHandler
    {
        #region Props
        /// <summary>
        /// Gets or sets a value indicating if logging of exceptions is enabled.
        /// </summary>
        public static bool LoggingEnabled { get; set; }
        #endregion
    }
}
