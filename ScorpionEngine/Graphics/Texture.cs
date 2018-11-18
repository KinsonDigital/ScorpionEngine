using ScorpionCore;

namespace ScorpionEngine.Graphics
{
    public class Texture
    {
        #region Props
        internal ITexture InternalTexture { get; set; }

        public int Width
        {
            get => InternalTexture.Width;
            set => InternalTexture.Width = value;
        }

        public int Height
        {
            get => InternalTexture.Height;
            set => InternalTexture.Height = value;
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
