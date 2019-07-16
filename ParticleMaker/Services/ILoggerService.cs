using System;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides capibility to log information.
    /// </summary>
    public interface ILoggerService
    {
        #region Methods
        /// <summary>
        /// Logs general information using the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data/info to log.</param>
        /// <param name="dateTime">The date time stamp of when the event occured.</param>
        void Log(string data, DateTime dateTime);


        /// <summary>
        /// Logs an error using the given information.
        /// </summary>
        /// <param name="data">The data/info to log.</param>
        /// <param name="dateTime">The date time stamp of when an error has occured.</param>
        /// <param name="errorNumber">The error number associated with the error.</param>
        void LogError(string data, DateTime dateTime, int errorNumber);
        #endregion
    }
}
