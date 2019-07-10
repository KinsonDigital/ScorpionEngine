namespace KDScorpionCore.Graphics
{
    /// <summary>
    /// The texture to render to the screen.
    /// </summary>
    public class Texture
    {
        #region Props
        /// <summary>
        /// Gets the internal texture plugin.
        /// </summary>
        internal ITexture InternalTexture { get; set; }

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        public int Width => InternalTexture.Width;

        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        public int Height => InternalTexture.Height;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Texture"/>.
        /// </summary>
        /// <param name="texture">The texture implementation to inject.</param>
        internal Texture(ITexture texture) => InternalTexture = texture;
        #endregion
    }
}
