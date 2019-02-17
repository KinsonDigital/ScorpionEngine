using System;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Represents a single event with information about that event and when it occured.
    /// </summary>
    public class Log
    {
        #region Props
        /// <summary>
        /// Gets or sets the data about the event.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the date and time of when the event occurred.
        /// </summary>
        public DateTime DateTimeStamp { get; set; }

        /// <summary>
        /// The unique numbre associated with the error if this is an error log.
        /// </summary>
        public int ErrorNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this log record is an error.
        /// </summary>
        public bool IsError { get; set; }
        #endregion
    }
}
