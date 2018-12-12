using KDParticleEngine;
using KDScorpionCore.Graphics;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using CoreVector = KDScorpionCore.Vector;
using ThreadTimer = System.Threading.Timer;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphicsEngine _graphicsEngine;
        private ThreadTimer _timer;
        private ThreadTimer _shutDownTimer;
        private bool _timerRan;
        private ParticleEngine _particleEngine;
        private Email _email;


        public MainWindow()
        {
            InitializeComponent();

            _email = new Email();

            DataContext = this;
            ElementHost.EnableModelessKeyboardInterop(this);
        }

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        // Using a DependencyProperty as the backing store for MessageProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MainWindow), new PropertyMetadata(""));


        protected override void OnClosing(CancelEventArgs e)
        {
            _graphicsEngine.Exit();

            //Give the graphics engine some time to shutdown and cleanup
            _shutDownTimer = new ThreadTimer((state) =>
            {
                base.OnClosing(e);
            }, null, 2000, 0);
        }
            

        protected override void OnInitialized(EventArgs e)
        {
            _timer = new ThreadTimer(TimerCallback, null, 0, 250);

            _particleEngine = new ParticleEngine(new CoreVector(350, 200))
            {
                Enabled = true
            };

            //var texture = new Texture();

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
                    _graphicsEngine = new GraphicsEngine((winFormsHost.Child as PictureBox).Handle, _particleEngine);
                    _graphicsEngine.Run();
                }
            });
        }
    }
}
