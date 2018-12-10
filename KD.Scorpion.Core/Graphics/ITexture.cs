using KDScorpionCore.Plugins;

namespace KDScorpionCore.Graphics
{
    public interface ITexture : IPlugin
    {
        int Width { get; set; }

        int Height { get; set; }

        T GetTexture<T>() where T : class;
    }
}