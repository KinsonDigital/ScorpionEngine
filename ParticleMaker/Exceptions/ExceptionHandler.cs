using System;
using ParticleMaker.Services;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Handles exceptions that are thrown by displaying them to the user and 
    /// logging them for later inspection.
    /// </summary>
    public static class ExceptionHandler
    {
        #region Fields
        private static bool _loggingEnabled;
        private static bool _showMessageBoxEnabled;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets a value indicating if logging of exceptions is enabled.
        /// </summary>
        public static bool LoggingEnabled
        {
            get => Logger != null && _loggingEnabled;
            set => _loggingEnabled = value;
        }

        /// <summary>
        /// Gets or sets the logger service that performes the logging of exceptions.
        /// </summary>
        public static ILoggerService Logger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if a dialog will show when an exception is handled.
        /// </summary>
        public static bool ShowMessageBoxEnabled
        {
            get => ExceptionMessageBox != null && _showMessageBoxEnabled;
            set => _showMessageBoxEnabled = value;
        }

        /// <summary>
        /// Gets or sets the message box dialog to be shown when an exception is handled.
        /// </summary>
        public static IExceptionMessage ExceptionMessageBox { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Handles the given <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        public static void Handle(Exception exception)
        {
            if (ShowMessageBoxEnabled)
            {
                ExceptionMessageBox.ShowMessage(exception, null);
            }
            else
            {
                if (LoggingEnabled)
                    Logger.LogError(exception.Message, DateTime.Now, exception.HResult);

                throw exception;
            }

            if (LoggingEnabled)
                Logger.LogError(exception.Message, DateTime.Now, exception.HResult);
        }
        #endregion
    }
}
