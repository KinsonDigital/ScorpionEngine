using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker
{
    /// <summary>
    /// Creates dependencies for a <see cref="GraphicsEngine"/>.
    /// </summary>
    public class GraphicsEngineFactory : IGraphicsEngineFactory
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="GraphicsEngineFactory"/>.
        /// </summary>
        public GraphicsEngineFactory(ICoreEngine coreEngine)
        {
            CoreEngine = coreEngine;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the <see cref="ICoreEngine"/>.
        /// </summary>
        public ICoreEngine CoreEngine { get; private set; }

        /// <summary>
        /// Gets the <see cref="SpriteBatch"/> for rendering.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SpriteBatch SpriteBatch { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Creates a new <see cref="Renderer"/>.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public Renderer NewRenderer()
        {
            SpriteBatch = new SpriteBatch(CoreEngine.GraphicsDevice);

            var particleRenderer = new ParticleRenderer(SpriteBatch);


            return new Renderer(particleRenderer);
        }
        #endregion
    }
}
