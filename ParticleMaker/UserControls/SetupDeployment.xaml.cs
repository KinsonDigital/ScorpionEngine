using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinDialogResult = System.Windows.Forms.DialogResult;
using FolderDialog = System.Windows.Forms.FolderBrowserDialog;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupDeployment.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class SetupDeployment : UserControl, IDisposable
    {
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

            _refreshTask = new Task(Refresh, _tokenSrc.Token);
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
        #endregion


        /// <summary>
        /// Gets or sets the directory path to where a setup deployment will occur.
        /// </summary>
        public string DeploymentPath
        {
            get { return (string)GetValue(DeploymentPathProperty); }
            set { SetValue(DeploymentPathProperty, value); }
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
        /// Refreshes the UI at a set interval.
        /// </summary>
        private void Refresh()
        {
            while(!_tokenSrc.IsCancellationRequested)
            {
                _tokenSrc.Token.WaitHandle.WaitOne(3000);
            }
        }
        #endregion
    }
}
