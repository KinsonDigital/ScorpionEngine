using ParticleMaker.CustomEventArgs;
using ParticleMaker.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlTestWindow.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class UserControlTestWindow : Window
    {
        private ICommand _renameCommand;
        private ICommand _deleteCommand;
        private ICommand _saveCommand;


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="UserControlTestWindow"/>.
        /// </summary>
        public UserControlTestWindow()
        {
            InitializeComponent();
        }
        #endregion


        public ICommand RenameCommand
        {
            get
            {
                if (_renameCommand == null)
                    _renameCommand = new RelayCommand((param) =>
                    {
                        var paramData = param as RenameItemEventArgs;

                        MessageBox.Show($"Setup '{paramData.OldName}' has been renamed to '{paramData.NewName}'.");
                    }, (param) => true);


                return _renameCommand;
            }
        }


        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand((param) =>
                    {
                        var paramData = param as ItemEventArgs;

                        MessageBox.Show($"Setup '{paramData.Name}' has been delete at path '{paramData.Path}'.");
                    }, (param) => true);


                return _deleteCommand;
            }
        }


        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand((param) =>
                    {
                        var paramData = param as ItemEventArgs;

                        MessageBox.Show($"Setup '{paramData.Name}' has been saved at path '{paramData.Path}'.");
                    }, (param) => true);


                return _saveCommand;
            }
        }

        #region Private Methods
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ExceptionMessageBox("")
            {
                Owner = this
            };

            dialog.ShowDialog();
        }
        #endregion
    }
}
