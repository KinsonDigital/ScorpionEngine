using KDScorpionCore.Plugins;

namespace KDScorpionCore.Graphics
{
    public interface ITexture : IPlugin
    {
        int Width { get; }

        int Height { get; }

        T GetTextureAsClass<T>() where T : class;

        T GetTextureAsStruct<T>() where T : struct;
    }
}