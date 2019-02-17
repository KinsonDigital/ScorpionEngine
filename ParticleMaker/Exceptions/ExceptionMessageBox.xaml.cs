using System.Windows;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Interaction logic for ExceptionMessageBox.xaml
    /// </summary>
    public partial class ExceptionMessageBox : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance <see cref="ExceptionMessageBox"/>.
        /// </summary>
        public ExceptionMessageBox()
        {
            InitializeComponent();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Closes the message box.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
