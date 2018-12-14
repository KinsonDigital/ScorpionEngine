using KDParticleEngine;
using KDScorpionCore;
using System;
using System.Windows.Forms;
using System.Windows.Threading;
using ThreadTimer = System.Threading.Timer;

namespace ParticleMaker.ViewModels
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    public class MainViewModel
    {
        #region Fields
        private GraphicsEngine _graphicsEngine;
        private ThreadTimer _timer;
        private PictureBox _renderSurface;
        private bool _timerRan;
        private ParticleEngine _particleEngine;
        private Dispatcher _uiDispatcher;
        #endregion


        #region Constructor
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

            _particleEngine = new ParticleEngine(new Vector(350, 200))
            {
                Enabled = true
            };
        }
        #endregion


        #region Props
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
        #endregion
    }
}
