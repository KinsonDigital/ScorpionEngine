using System;
using System.Windows.Forms;
using System.Windows.Threading;
using KDParticleEngine;
using ThreadTimer = System.Threading.Timer;
using CoreVector = KDScorpionCore.Vector;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using KDScorpionCore.Graphics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;

namespace ParticleMaker.ViewModels
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Private Fields
        private readonly GraphicsEngine _graphicsEngine;
        private readonly CancellationTokenSource _cancelTokenSrc;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="renderSurface">The surface to render the graphics on.</param>
        /// <param name="uiDispatcher">The UI thread to start the graphics engine on.</param>
        [ExcludeFromCodeCoverage]
        public MainViewModel(GraphicsEngine graphicsEngine)
        {
            _graphicsEngine = graphicsEngine;

            _cancelTokenSrc = new CancellationTokenSource();

            StartupTask = new Task(() =>
            {
                //If the cancellation has not been requested, keep processing.
                while(!_cancelTokenSrc.IsCancellationRequested)
                {
                    _cancelTokenSrc.Token.WaitHandle.WaitOne(250);

                    UIDispatcher.Invoke(() =>
                    {
                        if (_cancelTokenSrc.IsCancellationRequested == false && RenderSurface != null && RenderSurface.Handle != IntPtr.Zero)
                        {
                            _cancelTokenSrc.Cancel();

                            _graphicsEngine.RenderSurfaceHandle = RenderSurface.Handle;
                            _graphicsEngine.ParticleEngine.LivingParticlesCountChanged += _particleEngine_LivingParticlesCountChanged;
                            _graphicsEngine.ParticleEngine.SpawnLocation = new CoreVector(200, 200);
                            _graphicsEngine.Run();
                        }
                    });
                }
            }, _cancelTokenSrc.Token);


            StartupTask.Start();
        }
        #endregion


        #region Props
        internal Task StartupTask { get; set; }

        /// <summary>
        /// Gets or sets the timer for 
        /// </summary>
        internal ThreadTimer StartupTimer { get; set; }

        /// <summary>
        /// Gets or sets the surface that the particles will render to.
        /// </summary>
        public Control RenderSurface { get; set; }

        /// <summary>
        /// Gets or sets the dispatcher of the UI thread.
        /// </summary>
        public Dispatcher UIDispatcher { get; set; }

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
        /// Gets or sets the total number of particles that can be alive at any time.
        /// </summary>
        public int TotalParticles
        {
            get => _graphicsEngine.ParticleEngine.TotalParticlesAliveAtOnce;
            set => _graphicsEngine.ParticleEngine.TotalParticlesAliveAtOnce = value;
        }

        /// <summary>
        /// Gets the total number of living particles.
        /// </summary>
        public int TotalLivingParticles => _graphicsEngine.ParticleEngine.TotalLivingParticles;

        /// <summary>
        /// Gets the total number of dead particles.
        /// </summary>
        public int TotalDeadParticles => _graphicsEngine.ParticleEngine.TotalDeadParticles;

        /// <summary>
        /// Gets or sets the minimum value of the red color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int RedMin
        {
            get => _graphicsEngine.ParticleEngine.RedMin;
            set
            {
                _graphicsEngine.ParticleEngine.RedMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the red color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int RedMax
        {
            get => _graphicsEngine.ParticleEngine.RedMax;
            set
            {
                _graphicsEngine.ParticleEngine.RedMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the green color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int GreenMin
        {
            get => _graphicsEngine.ParticleEngine.GreenMin;
            set
            {
                _graphicsEngine.ParticleEngine.GreenMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the green color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int GreenMax
        {
            get => _graphicsEngine.ParticleEngine.GreenMax;
            set
            {
                _graphicsEngine.ParticleEngine.GreenMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the blue color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int BlueMin
        {
            get => _graphicsEngine.ParticleEngine.BlueMin;
            set
            {
                _graphicsEngine.ParticleEngine.BlueMin = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the blue color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int BlueMax
        {
            get => _graphicsEngine.ParticleEngine.BlueMax;
            set
            {
                _graphicsEngine.ParticleEngine.BlueMax = (byte)value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum size of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float SizeMin
        {
            get => _graphicsEngine.ParticleEngine.SizeMin;
            set
            {
                _graphicsEngine.ParticleEngine.SizeMin = value;
           }
        }

        /// <summary>
        /// Gets or sets the maximum size of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float SizeMax
        {
            get => _graphicsEngine.ParticleEngine.SizeMax;
            set
            {
                _graphicsEngine.ParticleEngine.SizeMax = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMin
        {
            get => _graphicsEngine.ParticleEngine.AngleMin;
            set => _graphicsEngine.ParticleEngine.AngleMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMax
        {
            get => _graphicsEngine.ParticleEngine.AngleMax;
            set => _graphicsEngine.ParticleEngine.AngleMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum angular velocity of the range that a <see cref="Particle"/> be will randomly set to.
        /// </summary>
        public float AngularVelocityMin
        {
            get => _graphicsEngine.ParticleEngine.AngularVelocityMin;
            set => _graphicsEngine.ParticleEngine.AngularVelocityMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum angular velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngularVelocityMax
        {
            get => _graphicsEngine.ParticleEngine.AngularVelocityMax;
            set => _graphicsEngine.ParticleEngine.AngularVelocityMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMin
        {
            get => _graphicsEngine.ParticleEngine.VelocityXMin;
            set => _graphicsEngine.ParticleEngine.VelocityXMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMax
        {
            get => _graphicsEngine.ParticleEngine.VelocityXMax;
            set => _graphicsEngine.ParticleEngine.VelocityXMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMin
        {
            get => _graphicsEngine.ParticleEngine.VelocityYMin;
            set => _graphicsEngine.ParticleEngine.VelocityYMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum Y velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMax
        {
            get => _graphicsEngine.ParticleEngine.VelocityYMax;
            set => _graphicsEngine.ParticleEngine.VelocityYMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifetimeMin
        {
            get => _graphicsEngine.ParticleEngine.LifeTimeMin;
            set => _graphicsEngine.ParticleEngine.LifeTimeMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifetimeMax
        {
            get => _graphicsEngine.ParticleEngine.LifeTimeMax;
            set => _graphicsEngine.ParticleEngine.LifeTimeMax = value;
        }

        /// <summary>
        /// Gets or sets the minimum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMin
        {
            get => _graphicsEngine.ParticleEngine.SpawnRateMin;
            set => _graphicsEngine.ParticleEngine.SpawnRateMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMax
        {
            get => _graphicsEngine.ParticleEngine.SpawnRateMax;
            set => _graphicsEngine.ParticleEngine.SpawnRateMax = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if the colors will be randomly chosen from a list.
        /// </summary>
        public bool UseColorsFromList
        {
            get => _graphicsEngine.ParticleEngine.UseTintColorList;
            set
            {
                _graphicsEngine.ParticleEngine.UseTintColorList = value;
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

                for (int i = 0; i < _graphicsEngine.ParticleEngine.TintColors.Length; i++)
                {
                    var clrItem = _graphicsEngine.ParticleEngine.TintColors[i].ToColorItem();
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

                _graphicsEngine.ParticleEngine.TintColors = result.ToArray();
            }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Shuts down the graphics engine.
        /// </summary>
        public void ShutdownEngine()
        {
            _cancelTokenSrc.Cancel();

            _graphicsEngine.Stop();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the <see cref="TotalLivingParticles"/> property to update the UI.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void _particleEngine_LivingParticlesCountChanged(object sender, EventArgs e)
        {
            NotifyPropChange(nameof(TotalLivingParticles));
            NotifyPropChange(nameof(TotalDeadParticles));
        }


        /// <summary>
        /// Invoked by the dispatcher on the UI thread.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void StartUp()
        {
            //if (RenderSurface.Handle != IntPtr.Zero)
            //{
            //    _cancelTokenSrc.Cancel();

            //    _graphicsEngine.RenderSurfaceHandle = RenderSurface.Handle;
            //    _graphicsEngine.ParticleEngine.LivingParticlesCountChanged += _particleEngine_LivingParticlesCountChanged;
            //    _graphicsEngine.ParticleEngine.SpawnLocation = new CoreVector(200, 200);
            //    _graphicsEngine.Run();
            //}
        }


        /// <summary>
        /// Notifies the binding system that a property value has changed.
        /// </summary>
        /// <param name="propName"></param>
        [ExcludeFromCodeCoverage]
        private void NotifyPropChange([CallerMemberName]string propName = "")
        {
            if (!string.IsNullOrEmpty(propName))
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
