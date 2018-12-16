﻿using KDParticleEngine;
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

            _particleEngine = new ParticleEngine(new CoreVector(200, 200))
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
        public int RenderSurfaceWidth
        {
            get => _graphicsEngine.Width;
            set
            {
                _graphicsEngine.Width = value;
            }
        }

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
                NotifyPropChange();
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
                NotifyPropChange();
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
                NotifyPropChange();
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
                NotifyPropChange();
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
                NotifyPropChange();
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
                NotifyPropChange();
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
                NotifyPropChange();
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
                    _graphicsEngine = new GraphicsEngine(_renderSurface.Handle, _particleEngine, 400, 400);
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
