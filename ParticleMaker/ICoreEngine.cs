using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker
{
    /// <summary>
    /// The core engine that helps drive the <see cref="GraphicsEngine"/> class or another custome engine class.
    /// </summary>
    public interface ICoreEngine : IDisposable
    {
        #region Events
        /// <summary>
        /// Invoked once to initialize the <see cref="ICoreEngine"/>.
        /// </summary>
        event EventHandler<EventArgs> OnInitialize;

        /// <summary>
        /// Invoked once to load any content that needs to be rendering.
        /// </summary>
        event EventHandler<EventArgs> OnLoadContent;

        /// <summary>
        /// Invoked every frame to update the <see cref="ICoreEngine"/>.
        /// </summary>
        event EventHandler<UpdateEventArgs> OnUpdate;

        /// <summary>
        /// Invoked every frame when the engine needs to render to the rendering surface.
        /// </summary>
        event EventHandler<DrawEventArgs> OnDraw;

        /// <summary>
        /// Invoked once to unload any content.
        /// </summary>
        event EventHandler<EventArgs> OnUnLoadContent;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets a value indicating if the engine is running.
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// The original render window.
        /// </summary>
        GameWindow OriginalWindow { get; set; }

        /// <summary>
        /// Gets the graphics device that performs the rendering.
        /// </summary>
        GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Gets or sets the handle to the rendering surface.
        /// </summary>
        IntPtr RenderSurfaceHandle { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Runs the <see cref="ICoreEngine"/>.
        /// </summary>
        void Run();


        /// <summary>
        /// Exists and stops the <see cref="ICoreEngine"/>.
        /// </summary>
        void Exit();


        /// <summary>
        /// Unpauses the engine.
        /// </summary>
        void Play();


        /// <summary>
        /// Pauses the engine.
        /// </summary>
        void Pause();
        #endregion
    }
}