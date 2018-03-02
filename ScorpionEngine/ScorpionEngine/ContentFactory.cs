using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine
{
    /// <summary>
    /// Creates content like textures, sounds, and other content that can be used by the game.
    /// </summary>
    public class ContentFactory
    {
        private ContentManager _contentManager;

        public ContentFactory(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        /// <summary>
        /// Creates a new texture using the given path.
        /// </summary>
        /// <param name="assetPathName">The path and name of the content.</param>
        /// <returns></returns>
        public Texture2D CreateTexture(string assetPathName)
        {
            return _contentManager.Load<Texture2D>(assetPathName);
        }
    }
}