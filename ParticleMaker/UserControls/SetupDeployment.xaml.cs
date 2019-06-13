using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinDialogResult = System.Windows.Forms.DialogResult;
using FolderDialog = System.Windows.Forms.FolderBrowserDialog;
using ParticleMaker.CustomEventArgs;
using System.IO;
using System.Windows.Input;
using System.ComponentModel;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupDeployment.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class SetupDeployment : UserControl
    {
        #region Public Events
        /// <summary>
        /// Inovked when the edit button has been clicked.
        /// </summary>
        public event EventHandler<DeploySetupEventArgs> EditClicked;

        /// <summary>
        /// Invoked when the deploy setup button has been clicked.
        /// </summary>
        public event EventHandler<DeploySetupEventArgs> DeployClicked;
        #endregion


        #region Fields
        private readonly Task _refreshTask;
        private readonly CancellationTokenSource _refreshTokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupDeployment"/>.
        /// </summary>
        public SetupDeployment()
        {
            InitializeComponent();

            Dispatcher.ShutdownStarted += (sender, e) => _refreshTokenSrc.Cancel();

            _refreshTokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _refreshTokenSrc.Token);
            _refreshTask.Start();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the directory path to where a setup deployment will occur.
        /// </summary>
        public string DeploymentPath
        {
            get => (string)GetValue(DeploymentPathProperty);
            set => SetValue(DeploymentPathProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="DeploymentPath"/> property.
        /// </summary>
        public static readonly DependencyProperty DeploymentPathProperty =
            DependencyProperty.Register(nameof(DeploymentPath), typeof(string), typeof(SetupDeployment), new PropertyMetadata("", DeploymentPathChanged));


        /// <summary>
        /// Gets or sets the command to execute when the edit button has been clicked.
        /// </summary>
        public ICommand EditClickedCommand
        {
            get => (ICommand)GetValue(EditClickedCommandProperty);
            set => SetValue(EditClickedCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="EditClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty EditClickedCommandProperty =
            DependencyProperty.Register(nameof(EditClickedCommand), typeof(ICommand), typeof(SetupDeployment), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets command that is executed when the deploy button has been clicked.
        /// </summary>
        public ICommand DeployClickedCommand
        {
            get => (ICommand)GetValue(DeployClickedCommandProperty);
            set => SetValue(DeployClickedCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="DeployClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty DeployClickedCommandProperty =
            DependencyProperty.Register(nameof(DeployClickedCommand), typeof(ICommand), typeof(SetupDeployment), new PropertyMetadata(null));


        /// <summary>
        /// Gets or stes the a value indicating if the control has an error due to
        /// and invalid <see cref="DeploymentPath"/>.
        /// </summary>
        private bool HasError
        {
            get => (bool)GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="HasError"/> property.
        /// </summary>
        private static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(SetupDeployment), new PropertyMetadata(false));
        #endregion


        #region Private Methods
        /// <summary>
        /// Allows the user to choose a deployment path.
        /// </summary>
        private void EditCustomButton_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderDialog
            {
                Description = "Choose setup deployment destination . . .",
                SelectedPath = string.IsNullOrEmpty(DeploymentPath) ? @"C:\" : DeploymentPath
            };

            var dialogResult = folderDialog.ShowDialog();

            if (dialogResult == WinDialogResult.OK)
            {
                DeploymentPath = folderDialog.SelectedPath;

                EditClicked?.Invoke(this, new DeploySetupEventArgs(DeploymentPath));
                EditClickedCommand?.Execute(new DeploySetupEventArgs(DeploymentPath));
            }
        }


        /// <summary>
        /// Invokes the <see cref="DeployClicked"/> event.
        /// </summary>
        private void DeploySetupCustomButton_Click(object sender, EventArgs e)
        {
            DeployClicked?.Invoke(this, new DeploySetupEventArgs(DeploymentPath));
            DeployClickedCommand?.Execute(new DeploySetupEventArgs(DeploymentPath));
        }


        /// <summary>
        /// Refreshes the UI.
        /// </summary>
        private static void DeploymentPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SetupDeployment)d;

            if (ctrl == null)
                return;

            ctrl.Refresh();
        }


        /// <summary>
        /// Invoked by the refresh task to refresh the UI at a set interval.
        /// </summary>
        private void RefreshAction()
        {
            while (!_refreshTokenSrc.IsCancellationRequested)
            {
                _refreshTokenSrc.Token.WaitHandle.WaitOne(2000);

                if (App.IsShuttingDown)
                    _refreshTokenSrc.Cancel();

                Refresh();
            }
        }


        /// <summary>
        /// Refreshes the UI.
        /// </summary>
        private void Refresh()
        {
            if (_refreshTokenSrc.IsCancellationRequested)
                return;

            Dispatcher.Invoke(() =>
            {
                HasError = DesignerProperties.GetIsInDesignMode(this) ? false : !string.IsNullOrEmpty(DeploymentPath) && !Directory.Exists(DeploymentPath);
            });
        }
        #endregion
    }
}
