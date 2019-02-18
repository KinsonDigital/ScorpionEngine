using System;
using System.Windows;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Shows a dialog to display an exception to the user.
    /// </summary>
    public interface IExceptionMessage
    {
        #region Methods
        /// <summary>
        /// Shows the given <paramref name="exception"/> to the user.
        /// </summary>
        /// <param name="exception">The exception to display.</param>
        /// <param name="owner">The owner of the message box.</param>
        void ShowMessage(Exception exception, Window owner);
        #endregion
    }
}
