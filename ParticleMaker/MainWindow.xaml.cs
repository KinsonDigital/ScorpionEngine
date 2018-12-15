using ParticleMaker.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
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
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel = new MainViewModel(winFormsHost.Child as PictureBox, Dispatcher);
            DataContext = _mainViewModel;

            _mainViewModel.RedMax = 255;
        }
        #endregion
    }
}
