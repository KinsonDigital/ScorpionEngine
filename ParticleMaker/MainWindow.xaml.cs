using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.ComponentModel;
using ParticleMaker.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;
using WinMsgBox = System.Windows.MessageBox;
using CoreVector = KDScorpionCore.Vector;
using System.Drawing;

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
        private readonly MainViewModel _mainViewModel;
        private bool _isMouseDown = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            _mainViewModel = App.DIContainer.GetInstance<MainViewModel>();
            _mainViewModel.MainWindow = this;

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
            _mainViewModel.RenderSurface = (WinFormsHost.Child as PictureBox);
            _mainViewModel.UIDispatcher = Dispatcher;

            _mainViewModel.StartEngine();
            _mainViewModel.Pause.Execute(null);
            
            DataContext = _mainViewModel;
        }


        /// <summary>
        /// Sets the down state of the left mouse button to true.
        /// </summary>
        private void RenderSurface_MouseDown(object sender, MouseEventArgs e) => _isMouseDown = true;


        /// <summary>
        /// Sets the down state of the left mouse button to false.
        /// </summary>
        private void RenderSurface_MouseUp(object sender, MouseEventArgs e) => _isMouseDown = false;


        /// <summary>
        /// Updates the particle spawn location if the left mouse button is in the down position.
        /// </summary>
        private void RenderSurface_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
                _mainViewModel.SpawnLocation = new PointF(e.X, e.Y);
        }
        #endregion
    }
}
