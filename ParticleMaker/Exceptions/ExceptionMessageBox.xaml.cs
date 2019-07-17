using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Interaction logic for the <see cref="ExceptionMessageBox"/> dialog.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ExceptionMessageBox : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance <see cref="ExceptionMessageBox"/>.
        /// <paramref name="exception">The exception information to display.</paramref>
        /// </summary>
        public ExceptionMessageBox(Exception exception)
        {
            InitializeComponent();

            Title = exception == null?
                    "Exception" :
                    $"Exception - {exception.GetType().ToString()}";

            Message = exception.Message;

            StackTrace = exception.StackTrace;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the exception message to be displayed.
        /// </summary>
        private string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Message"/> property.
        /// </summary>
        private static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(ExceptionMessageBox), new PropertyMetadata(""));


        /// <summary>
        /// Gets or sets the stack trace of the exception.
        /// </summary>
        private string StackTrace
        {
            get => (string)GetValue(StackTraceProperty);
            set => SetValue(StackTraceProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="StackTrace"/> property.
        /// </summary>
        private static readonly DependencyProperty StackTraceProperty =
            DependencyProperty.Register(nameof(StackTrace), typeof(string), typeof(ExceptionMessageBox), new PropertyMetadata(""));
        #endregion


        #region Private Methods
        /// <summary>
        /// Closes the message box.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e) => Close();


        /// <summary>
        /// Copies the exception information to the clipboard.
        /// </summary>
        private void CopyButton_Click(object sender, RoutedEventArgs e) => Clipboard.SetText($"Exception Message:\r\n\t{Message}\r\n\r\nStack Trace:\r\n\t{StackTrace}");
        #endregion
    }
}
