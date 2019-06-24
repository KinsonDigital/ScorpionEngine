namespace KDScorpionCore.Graphics
{
    public class Texture
    {
        #region Props
        internal ITexture InternalTexture { get; set; }

        public int Width
        {
            get => InternalTexture.Width;
        }

        public int Height
        {
            get => InternalTexture.Height;
        }
        #endregion


        #region Constructors
        internal Texture(ITexture texture)
        {
            InternalTexture = texture;
        }
        #endregion
    }
}
