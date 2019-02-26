using ParticleMaker.CustomEventArgs;
using System.ComponentModel;
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
        private MyViewModel _viewModel;

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="UserControlTestWindow"/>.
        /// </summary>
        public UserControlTestWindow()
        {
            InitializeComponent();

            _viewModel = new MyViewModel();

            DataContext = _viewModel;
        }
        #endregion


        #region Private Methods
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion
    }


    [ExcludeFromCodeCoverage]
    public class MyViewModel : INotifyPropertyChanged
    {
        private ICommand _deploySetupCommand;
        private ICommand _editDeployPathCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand EditDeployPath
        {
            get
            {
                if (_editDeployPathCommand == null)
                    _editDeployPathCommand = new RelayCommand((param) =>
                    {
                        var paramData = param as DeploySetupEventArgs;

                        MessageBox.Show($"Setup changed to '{paramData.DeploymentPath}'.");
                    }, (param) => true);


                return _editDeployPathCommand;
            }
        }


        public ICommand DeploySetup
        {
            get
            {
                if (_deploySetupCommand == null)
                    _deploySetupCommand = new RelayCommand((param) =>
                    {
                        var paramData = param as DeploySetupEventArgs;

                        MessageBox.Show($"Setup deployed to '{paramData.DeploymentPath}'.");
                    }, (param) => true);


                return _deploySetupCommand;
            }
        }
    }
}
