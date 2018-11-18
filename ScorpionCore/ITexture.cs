using ScorpionCore.Plugins;

namespace ScorpionCore
{
    public interface ITexture : IPlugin
    {
        int Width { get; set; }

        int Height { get; set; }

        T GetTexture<T>() where T : class;
    }
}