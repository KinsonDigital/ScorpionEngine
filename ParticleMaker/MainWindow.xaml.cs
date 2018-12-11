using System;
using System.Windows;
using System.Windows.Forms;
using ThreadTimer = System.Threading.Timer;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphicsEngine _graphics;
        private ThreadTimer _timer;
        private bool _timerRan;


        public MainWindow()
        {
            InitializeComponent();
        }


        protected override void OnInitialized(EventArgs e)
        {
            _timer = new ThreadTimer(TimerCallback, null, 0, 250);

            base.OnInitialized(e);
        }


        private void TimerCallback(object state)
        {
            if (_timerRan)
                return;

            _timerRan = true;

            Dispatcher.Invoke(() =>
            {
                if ((winFormsHost.Child as PictureBox).Handle != IntPtr.Zero)
                {
                    _timer?.Dispose();
                    _timer = null;
                    _graphics = new GraphicsEngine((winFormsHost.Child as PictureBox).Handle);
                    _graphics.Run();
                }
            });
        }
    }
}
