using ParticleMaker.Dialogs;
using System.Windows;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlTestWindow.xaml
    /// </summary>
    public partial class UserControlTestWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="UserControlTestWindow"/>.
        /// </summary>
        public UserControlTestWindow()
        {
            InitializeComponent();
        }
        #endregion


        #region Private Methods
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog("Title", "Msg", "DefaultValue")
            {
                Owner = TestButton.FindParent<Window>()
            };

            inputDialog.ShowDialog();
        }
        #endregion
    }
}
