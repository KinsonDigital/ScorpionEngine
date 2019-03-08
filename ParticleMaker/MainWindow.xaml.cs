using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.ComponentModel;
using ParticleMaker.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;
using WinMsgBox = System.Windows.MessageBox;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class MainWindow : Window
    {
        #region Fields
        private static Window _self;
        private static CancellationTokenSource _setFocusTaskTokenSrc;
        private static Task _setFocusTask;
        private MainViewModel _mainViewModel;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            _mainViewModel = App.DIContainer.GetInstance<MainViewModel>();
            _mainViewModel.DialogOwner = this;

            ElementHost.EnableModelessKeyboardInterop(this);

            InitializeComponent();

            Loaded += MainWindow_Loaded;

            _self = this;
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Shuts down the graphics engine before closing the window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (_mainViewModel.SettingsChanged)
            {
                var msgResult = WinMsgBox.Show("You have unsaved settings.  Save first?", "Save Changes", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (msgResult == MessageBoxResult.Yes)
                    _mainViewModel.SaveSetup.Execute(null);
            }

            _mainViewModel.ShutdownEngine();

            base.OnClosing(e);
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Sets the window in focus.
        /// </summary>
        public static void SetFocus()
        {
            _setFocusTaskTokenSrc = new CancellationTokenSource();
            var totalAttempts = 0;

            _setFocusTask = new Task(() =>
            {
                while (!_setFocusTaskTokenSrc.IsCancellationRequested)
                {
                    _setFocusTaskTokenSrc.Token.WaitHandle.WaitOne(250);

                    _self.Dispatcher.Invoke(() =>
                    {
                        _self.Focus();
                        totalAttempts += 1;
                    });

                    if (totalAttempts >= 4)
                        _setFocusTaskTokenSrc.Cancel();
                }
            });

            _setFocusTask.Start();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Creates the <see cref="MainViewModel"/> with default settings.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel.RenderSurface = (winFormsHost.Child as PictureBox);
            _mainViewModel.UIDispatcher = Dispatcher;

            _mainViewModel.StartEngine();
            _mainViewModel.Pause.Execute(null);
            
            DataContext = _mainViewModel;
        }
        #endregion

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.StartEngine();
        }

        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.ShutdownEngine();
        }
    }
}
