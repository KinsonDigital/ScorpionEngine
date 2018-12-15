using KDParticleEngine;
using KDScorpionCore;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using ThreadTimer = System.Threading.Timer;
using CoreVector = KDScorpionCore.Vector;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ParticleMaker.ViewModels
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Public Fields
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Private Fields
        private GraphicsEngine _graphicsEngine;
        private ThreadTimer _timer;
        private PictureBox _renderSurface;
        private bool _timerRan;
        private ParticleEngine _particleEngine;
        private Dispatcher _uiDispatcher;
        private int _redMin;
        private int _redMax;
        private int _greenMin;
        private int _greenMax;
        private int _blueMin;
        private int _blueMax;
        #endregion


        #region Constructor
        public MainViewModel()
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="renderSurface">The surface to render the graphics on.</param>
        /// <param name="uiDispatcher">The UI thread to start the graphics engine on.</param>
        public MainViewModel(PictureBox renderSurface, Dispatcher uiDispatcher)
        {
            _renderSurface = renderSurface;
            _uiDispatcher = uiDispatcher;

            _timer = new ThreadTimer(TimerCallback, null, 0, 250);

            _particleEngine = new ParticleEngine(new CoreVector(350, 200))
            {
                Enabled = true,
                GreenMin = 0,
                GreenMax = 0,
                BlueMin = 0,
                BlueMax = 0
            };
        }
        #endregion


        #region Props
        public bool IsReady { get; set; }

        public int RedMin
        {
            get => _redMin;
            set
            {
                _redMin = value;
                _particleEngine.RedMin = (byte)value;
                NotifyPropChange();
            }
        }

        public int RedMax
        {
            get { return _redMax; }
            set
            {
                _redMax = value;
                _particleEngine.RedMax = (byte)value;
                NotifyPropChange();
            }
        }

        public int GreenMin
        {
            get { return _greenMin; }
            set
            {
                _greenMin = value;
                NotifyPropChange();
            }
        }

        public int GreenMax
        {
            get { return _greenMax; }
            set
            {
                _greenMax = value;
                NotifyPropChange();
            }
        }

        public int BlueMin
        {
            get { return _blueMin; }
            set
            {
                _blueMin = value;
                NotifyPropChange();
            }
        }

        public int BlueMax
        {
            get { return _blueMax; }
            set
            {
                _blueMax = value;
                NotifyPropChange();
            }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Shuts down the graphics engine.
        /// </summary>
        public void ShutdownEngine()
        {
            _graphicsEngine.Exit();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Invoked at a specified interval and checks when the render surface handle
        /// is available.  Once available, the graphics engine is created and the
        /// graphics engine rendering is pointed to the render surface.
        /// </summary>
        /// <param name="state">The state passed to the callback.</param>
        private void TimerCallback(object state)
        {
            if (_timerRan)
                return;

            _timerRan = true;

            _uiDispatcher.Invoke(() =>
            {
                if (_renderSurface.Handle != IntPtr.Zero)
                {
                    _timer?.Dispose();
                    _timer = null;
                    _graphicsEngine = new GraphicsEngine(_renderSurface.Handle, _particleEngine);
                    _graphicsEngine.Run();
                }
            });
        }


        private void NotifyPropChange([CallerMemberName] string propName = "")
        {
            if (!string.IsNullOrEmpty(propName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
