using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleMaker
{
    /// <summary>
    /// Provides ability to generate required components for a <see cref="ICoreEngine"/> implementation to run.
    /// </summary>
    public interface IGraphicsEngineFactory
    {
        #region Props
        /// <summary>
        /// Gets the core engine that helps drive a main <see cref="ICoreEngine"/> implementation.
        /// </summary>
        ICoreEngine CoreEngine { get; }

        /// <summary>
        /// Gets or sets the sprite batch used for rendering.
        /// </summary>
        SpriteBatch SpriteBatch { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Creates a new <see cref="ContentLoader"/> to load content.
        /// </summary>
        /// <returns></returns>
        ContentLoader NewContentLoader();

        /// <summary>
        /// Creates a new <see cref="Renderer"/> to render content to the rendering surface.
        /// </summary>
        /// <returns></returns>
        Renderer NewRenderer();
        #endregion
    }
}