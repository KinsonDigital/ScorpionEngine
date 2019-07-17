using KDParticleEngine;
using System;

namespace ParticleMaker
{
    /// <summary>
    /// Renders graphics to a render target.
    /// </summary>
    public interface IRenderer : IDisposable
    {
        #region Props
        /// <summary>
        /// Gets the pointer/window handler of the window to render the graphics to.
        /// </summary>
        IntPtr WindowHandle { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Initializes the renderer.
        /// </summary>
        /// <param name="windowHandle">The window handle of the window to render to.</param>
        void Init(IntPtr windowHandle);


        /// <summary>
        /// Loads texture file from disk and returns it as a <see cref="Particle{ITexture}"/>.
        /// </summary>
        /// <param name="path">The path to the texture file.</param>
        /// <returns></returns>
        ParticleTexture LoadTexture(string path);


        /// <summary>
        /// Begins the rendering process.
        /// </summary>
        /// <remarks>This method must be invoked first before the <see cref="Render(Particle{ParticleTexture})"/> method.</remarks>
        void Begin();


        /// <summary>
        /// Renders the given <paramref name="particle"/> to the render target.
        /// Each render call will be batched.
        /// </summary>
        /// <param name="particle">The particle to render.</param>
        void Render(Particle<ParticleTexture> particle);


        /// <summary>
        /// Ends the rendering process.  All of the graphics batched in the <see cref="Render(Particle{ParticleTexture})"/>
        /// calls will be rendered to the render target when this method is invoked.
        /// </summary>
        void End();


        /// <summary>
        /// Shuts down the renderer.
        /// </summary>
        void ShutDown();
        #endregion
    }
}
