using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.ComponentModel;
using ParticleMaker.ViewModels;
using ThreadTimer = System.Threading.Timer;
using WinMsgBox = System.Windows.MessageBox;
using ParticleMaker.UserControls;
using System.Windows.Media;

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


        private void ColorListItem_DeleteClicked(object sender, ColorItemClickedEventArgs e)
        {
            WinMsgBox.Show($"Del Img '{e.Id}' Clicked!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var startingClr = Color.FromRgb(62, 125, 255);

            var colorPicker = new ColorPicker(startingClr);

            colorPicker.ShowDialog();

            if (colorPicker.DialogResult == true)
            {
            }
        }
    }
}
