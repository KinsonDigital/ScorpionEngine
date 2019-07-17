using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Shows an exception message dialog to the user.
    /// </summary>
    public class ExceptionMessage : IExceptionMessage
    {
        #region Public Methods
        /// <summary>
        /// Shows the given <paramref name="exception"/> message to the user.
        /// </summary>
        /// <param name="exception">The exception to display.</param>
        /// <param name="owner">The owner of the message box.</param>
        [ExcludeFromCodeCoverage]
        public void ShowExceptionDialog(Exception exception, Window owner = null)
        {
            var messageBox = new ExceptionMessageBox(exception);

            if (owner != null)
                messageBox.Owner = owner;

            messageBox.ShowDialog();
        }
        #endregion
    }
}
