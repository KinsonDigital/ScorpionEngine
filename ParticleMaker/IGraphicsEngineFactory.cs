using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleMaker
{
    public interface IGraphicsEngineFactory
    {
        ICoreEngine CoreEngine { get; }
        SpriteBatch SpriteBatch { get; set; }

        ContentLoader NewContentLoader();
        Renderer NewRenderer();
    }
}