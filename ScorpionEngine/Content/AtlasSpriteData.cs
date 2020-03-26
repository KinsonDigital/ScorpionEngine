using Raptor;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngine.Content
{
    //TODO: Look into using this later during the building of a test game.
    [ExcludeFromCodeCoverage]
    internal class AtlasSpriteData
    {
        #region Props
        /// <summary>
        /// Gets or sets the bounds of the sprite data.
        /// </summary>
        public Rect Bounds { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the sprite data.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
