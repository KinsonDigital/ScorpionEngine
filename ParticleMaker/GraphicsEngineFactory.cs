using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleMaker
{
    /// <summary>
    /// Creates dependencies for a <see cref="GraphicsEngine"/>.
    /// </summary>
    public class GraphicsEngineFactory : IGraphicsEngineFactory
    {
        #region Constructors
        public GraphicsEngineFactory()
        {
            CoreEngine = new CoreEngine();
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
        public SpriteBatch SpriteBatch { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Creates a new <see cref="ContentLoader"/>.
        /// </summary>
        /// <returns></returns>
        public ContentLoader NewContentLoader()
        {
            var particleTextureLoader = App.DIContainer.GetInstance<ParticleTextureLoader>();
            particleTextureLoader.InjectData(CoreEngine.GraphicsDevice);


            return new ContentLoader(particleTextureLoader);
        }


        /// <summary>
        /// Creates a new <see cref="Renderer"/>.
        /// </summary>
        /// <returns></returns>
        public Renderer NewRenderer()
        {
            SpriteBatch = new SpriteBatch(CoreEngine.GraphicsDevice);

            var particleRenderer = new ParticleRenderer(SpriteBatch);


            return new Renderer(particleRenderer);
        }
        #endregion
    }
}
