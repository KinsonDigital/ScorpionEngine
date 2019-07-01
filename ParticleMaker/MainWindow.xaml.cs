using System.Windows;
using System.Drawing;
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
        private static CancellationTokenSource _dockRenderWindowTokenSrc;
        private static Task _dockRenderWindowTask;
        private readonly MainViewModel _mainViewModel;
        private RenderSurface _renderSurface;
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
            _dockRenderWindowTokenSrc.Cancel();

            if (_mainViewModel.SettingsChanged)
            {
                var msgResult = WinMsgBox.Show("You have unsaved settings.  Save first?", "Save Changes", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (msgResult == MessageBoxResult.Yes)
                    _mainViewModel.SaveSetup.Execute(null);
            }

            _mainViewModel.StopEngine();
            _mainViewModel.Dispose();

            App.IsShuttingDown = true;

            _renderSurface.Close();

            base.OnClosing(e);
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Creates the <see cref="MainViewModel"/> with default settings.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _renderSurface = new RenderSurface();
            DockRenderWindow();//Dock it once so it doesn't look like it teleports
            _renderSurface.Show();

            _dockRenderWindowTokenSrc = new CancellationTokenSource();
            _dockRenderWindowTask = new Task(() =>
            {
                while (!_dockRenderWindowTokenSrc.IsCancellationRequested)
                {
                    Thread.Sleep(62);
                    Dispatcher.Invoke(() =>
                    {
                        DockRenderWindow();
                    });
                }
            }, _dockRenderWindowTokenSrc.Token);

            _mainViewModel.RenderSurfaceHandle = _renderSurface.WindowHandle;

            _mainViewModel.UIDispatcher = Dispatcher;

            _mainViewModel.InitEngine();
            _mainViewModel.StartEngine();

            //TODO: Possibly remove this
            //_mainViewModel.StartEngine();
            //_mainViewModel.Pause.Execute(null);

            DataContext = _mainViewModel;

            _dockRenderWindowTask.Start();
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


        public void DockRenderWindow()
        {
            _renderSurface.Left = Left + (Width - 10);
            _renderSurface.Top = Top;
            _renderSurface.Width = Width;
            _renderSurface.Height = Height - 7;

            _renderSurface.Topmost = IsActive;
        }
        #endregion
    }
}
