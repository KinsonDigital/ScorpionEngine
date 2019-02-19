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

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupDeployment.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class SetupDeployment : UserControl, IDisposable
    {
        #region Public Events
        /// <summary>
        /// Invoked when the deploy setup button has been clicked.
        /// </summary>
        public event EventHandler<DeploySetupEventArgs> DeployClicked;
        #endregion


        #region Fields
        private Task _refreshTask;
        private CancellationTokenSource _tokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupDeployment"/>.
        /// </summary>
        public SetupDeployment()
        {
            InitializeComponent();

            Unloaded += SetupDeployment_Unloaded;

            _tokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _tokenSrc.Token);
            _refreshTask.Start();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="DeploymentPath"/> property.
        /// </summary>
        public static readonly DependencyProperty DeploymentPathProperty =
            DependencyProperty.Register(nameof(DeploymentPath), typeof(string), typeof(SetupDeployment), new PropertyMetadata("", DeploymentPathChanged));

        /// <summary>
        /// Registers the <see cref="DeployClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty DeployClickedCommandProperty =
            DependencyProperty.Register(nameof(DeployClickedCommand), typeof(ICommand), typeof(SetupDeployment), new PropertyMetadata(null));
        #endregion


        /// <summary>
        /// Gets or sets the directory path to where a setup deployment will occur.
        /// </summary>
        public string DeploymentPath
        {
            get { return (string)GetValue(DeploymentPathProperty); }
            set { SetValue(DeploymentPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets command that is executed when the deploy button has been clicked.
        /// </summary>
        public ICommand DeployClickedCommand
        {
            get { return (ICommand)GetValue(DeployClickedCommandProperty); }
            set { SetValue(DeployClickedCommandProperty, value); }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Disposes of the refresh task.
        /// </summary>
        public void Dispose()
        {
            _tokenSrc.Dispose();
            _refreshTask.Dispose();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Cancels the refresh task.
        /// </summary>
        private void SetupDeployment_Unloaded(object sender, RoutedEventArgs e)
        {
            _tokenSrc.Cancel();
        }


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
                DeploymentPath = folderDialog.SelectedPath;
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
            while(!_tokenSrc.IsCancellationRequested)
            {
                _tokenSrc.Token.WaitHandle.WaitOne(3000);

                Refresh();
            }
        }


        /// <summary>
        /// Refreshes the UI.
        /// </summary>
        private void Refresh()
        {
        }
        #endregion
    }
}
