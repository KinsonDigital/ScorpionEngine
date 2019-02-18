using ParticleMaker.Services;

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

        /// <summary>
        /// Gets or sets a value indicating if a dialog will show when an exception is handled.
        /// </summary>
        public static bool ShowDialogEnabled { get; set; }

        /// <summary>
        /// Gets or sets the logger service that performes the logging of exceptions.
        /// </summary>
        public static ILoggerService Logger { get; set; }
        #endregion
    }
}
