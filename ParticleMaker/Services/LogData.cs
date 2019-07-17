using System.Collections.Generic;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Holds logs of general information and errors.
    /// </summary>
    public class LogData
    {
        #region Props
        /// <summary>
        /// Gets or sets the list of logs.
        /// </summary>
        public List<Log> Logs { get; set; }
        #endregion
    }
}
