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
        public Texture(ITexture texture)
        {
            InternalTexture = texture;
        }
        #endregion


        #region Public Methods
        public T GetTexture<T>() where T : class
        {
            return InternalTexture.GetTexture<T>();
        }
        #endregion
    }
}
