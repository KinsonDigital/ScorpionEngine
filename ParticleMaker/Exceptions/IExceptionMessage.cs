using System;
using System.Windows;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Shows an exception message dialog to the user.
    /// </summary>
    public interface IExceptionMessage
    {
        #region Methods
        /// <summary>
        /// Shows the given <paramref name="exception"/> message to the user.
        /// </summary>
        /// <param name="exception">The exception to display.</param>
        /// <param name="owner">The owner of the message box.</param>
        void ShowExceptionDialog(Exception exception, Window owner);
        #endregion
    }
}
