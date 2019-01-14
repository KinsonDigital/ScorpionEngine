using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.ComponentModel;
using ParticleMaker.ViewModels;
using ThreadTimer = System.Threading.Timer;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private ThreadTimer _shutDownTimer;
        private MainViewModel _mainViewModel;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            ElementHost.EnableModelessKeyboardInterop(this);

            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Shuts down the graphics engine before closing the window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            _mainViewModel.ShutdownEngine();
            
            //Give the graphics engine some time to shutdown and cleanup
            _shutDownTimer = new ThreadTimer((state) =>
            {
                base.OnClosing(e);
            }, null, 2000, 0);
        }
        #endregion


        #region Event Methods
        /// <summary>
        /// Creates the <see cref="MainViewModel"/> with default settings.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel = App.DIContainer.GetInstance<MainViewModel>();
            _mainViewModel.RenderSurface = (winFormsHost.Child as PictureBox);
            _mainViewModel.UIDispatcher = Dispatcher;

            DataContext = _mainViewModel;

            _mainViewModel.RedMin = 0;
            _mainViewModel.RedMax = 255;
            _mainViewModel.GreenMin = 0;
            _mainViewModel.GreenMax = 255;
            _mainViewModel.BlueMin = 0;
            _mainViewModel.BlueMax = 255;
            _mainViewModel.SizeMin = 1;
            _mainViewModel.SizeMax = 2;
        }
        #endregion
    }
}
