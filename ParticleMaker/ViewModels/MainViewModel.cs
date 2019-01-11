using System;
using System.Windows.Forms;
using System.Windows.Threading;
using KDParticleEngine;
using ThreadTimer = System.Threading.Timer;
using CoreVector = KDScorpionCore.Vector;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using KDScorpionCore.Graphics;

namespace ParticleMaker.ViewModels
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    public class MainViewModel
    {
        #region Private Fields
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
        public MainViewModel()
        {
            _particleEngine = new ParticleEngine(new CoreVector(200, 200));
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

            _particleEngine = new ParticleEngine(new CoreVector(200, 200))
            {
                Enabled = true,
                RedMin = 0,
                RedMax = 255,
                GreenMin = 0,
                GreenMax = 255,
                BlueMin = 0,
                BlueMax = 255
            };
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the width of the render surface.
        /// </summary>
        public int RenderSurfaceWidth
        {
            get => _graphicsEngine.Width;
            set
            {
                _graphicsEngine.Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the render surface.
        /// </summary>
        public int RenderSurfaceHeight
        {
            get => _graphicsEngine.Height;
            set
            {
                _graphicsEngine.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the red color component range.
        /// </summary>
        public int RedMin
        {
            get => _particleEngine.RedMin;
            set
            {
                _particleEngine.RedMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the red color component range.
        /// </summary>
        public int RedMax
        {
            get => _particleEngine.RedMax;
            set
            {
                _particleEngine.RedMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the green color component range.
        /// </summary>
        public int GreenMin
        {
            get => _particleEngine.GreenMin;
            set
            {
                _particleEngine.GreenMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the green color component range.
        /// </summary>
        public int GreenMax
        {
            get => _particleEngine.GreenMax;
            set
            {
                _particleEngine.GreenMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the blue color component range.
        /// </summary>
        public int BlueMin
        {
            get => _particleEngine.BlueMin;
            set
            {
                _particleEngine.BlueMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the blue color component range.
        /// </summary>
        public int BlueMax
        {
            get => _particleEngine.BlueMax;
            set
            {
                _particleEngine.BlueMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum size of the range that the size can be randomly chosen from.
        /// </summary>
        public float SizeMin
        {
            get => _particleEngine.SizeMin;
            set
            {
                _particleEngine.SizeMin = value;
           }
        }

        /// <summary>
        /// Gets or sets the maximum size of the range that the size can be randomly chosen from.
        /// </summary>
        public float SizeMax
        {
            get => _particleEngine.SizeMax;
            set
            {
                _particleEngine.SizeMax = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum angle of the range that a particle will randomly be set to.
        /// </summary>
        public float AngleMin
        {
            get => _particleEngine.AngleMin;
            set => _particleEngine.AngleMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum angle of the range that a particle will randomly be set to.
        /// </summary>
        public float AngleMax
        {
            get => _particleEngine.AngleMax;
            set => _particleEngine.AngleMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum angular velocity of the range that a particle will randomly be set to.
        /// </summary>
        public float AngularVelocityMin
        {
            get => _particleEngine.AngularVelocityMin;
            set => _particleEngine.AngularVelocityMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum angular velocity of the range that a particle will randomly be set to.
        /// </summary>
        public float AngularVelocityMax
        {
            get => _particleEngine.AngularVelocityMax;
            set => _particleEngine.AngularVelocityMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum life time of the range that a particle will randomly be set to.
        /// </summary>
        public int LifetimeMin
        {
            get => _particleEngine.LifeTimeMin;
            set => _particleEngine.LifeTimeMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum life time of the range that a particle will randomly be set to.
        /// </summary>
        public int LifetimeMax
        {
            get => _particleEngine.LifeTimeMax;
            set => _particleEngine.LifeTimeMax = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if the colors will be randomly chosen from a list.
        /// </summary>
        public bool UseColorsFromList
        {
            get => _particleEngine.UseTintColorList;
            set
            {
                _particleEngine.UseTintColorList = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of colors to randomly choose from.
        /// </summary>
        public ObservableCollection<ColorItem> Colors
        {
            get
            {
                var result = new ObservableCollection<ColorItem>();

                for (int i = 0; i < _particleEngine.TintColors.Length; i++)
                {
                    var clrItem = _particleEngine.TintColors[i].ToColorItem();
                    clrItem.Id = i;

                    result.Add(clrItem);
                }


                return result;
            }
            set
            {
                var result = new List<GameColor>();

                foreach (var clr in value)
                {
                    result.Add(new GameColor(clr.ColorBrush.Color.R, clr.ColorBrush.Color.G, clr.ColorBrush.Color.B, clr.ColorBrush.Color.A));
                }

                _particleEngine.TintColors = result.ToArray();
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
                    _graphicsEngine = new GraphicsEngine(_renderSurface.Handle, _particleEngine, 400, 400);
                    _graphicsEngine.Run();
                }
            });
        }
        #endregion
    }
}
