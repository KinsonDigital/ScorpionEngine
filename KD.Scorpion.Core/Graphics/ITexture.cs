using KDScorpionCore.Plugins;

namespace KDScorpionCore.Graphics
{
    public interface ITexture : IPlugin
    {
        int Width { get; }

        int Height { get; }
    }
}