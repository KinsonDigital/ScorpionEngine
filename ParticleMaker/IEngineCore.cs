using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMaker
{
    /// <summary>
    /// The core engine that helps drive the <see cref="GraphicsEngine"/> class or another custome engine class.
    /// </summary>
    public interface ICoreEngine : IDisposable
    {
        #region Props
        /// <summary>
        /// Gets or sets a value indicating if the engine is running.
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// Gets or sets the handle to the rendering surface.
        /// </summary>
        IntPtr RenderSurfaceHandle { get; set; }

        /// <summary>
        /// Gets or sets the width of the render surface.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the render surface.
        /// </summary>
        int RenderHeight { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Starts the engine.
        /// </summary>
        void Start();


        /// <summary>
        /// Stops the engine.
        /// </summary>
        void Stop();


        /// <summary>
        /// Plays the engine.
        /// </summary>
        void Play();


        /// <summary>
        /// Pauses the engine.
        /// </summary>
        /// <param name="clearSurface">If true, will clear the rendering surface before pausing.</param>
        void Pause(bool clearSurface);
        #endregion
    }
}
