using ParticleMaker.CustomEventArgs;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System;
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
        private ICommand _deployCommand;
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


        public ICommand DeployCommand
        {
            get
            {
                if (_deployCommand == null)
                    _deployCommand = new RelayCommand((param) =>
                    {
                        var paramData = param as DeploySetupEventArgs;

                        MessageBox.Show($"Setup deployed to '{paramData.DeploymentPath}'.");
                    }, (param) => true);


                return _deployCommand;
            }
        }


        #region Private Methods
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion
    }
}
