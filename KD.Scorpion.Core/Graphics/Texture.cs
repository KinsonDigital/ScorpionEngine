using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(assemblyName: "ScorpionEngineTests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo(assemblyName: "ScorpionCoreTests", AllInternalsVisible = true)]

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
    }
}
