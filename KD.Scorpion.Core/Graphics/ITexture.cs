using KDScorpionCore.Plugins;

namespace KDScorpionCore.Graphics
{
    public interface ITexture : IPlugin
    {
        #region Props
        int Width { get; }

        int Height { get; }
        #endregion
    }
}