using ScorpionCore.Plugins;

namespace ScorpionCore
{
    //TODO: Add docs
    public interface IText : IPlugin
    {
        int Width { get; }

        int Height { get; }

        string Text { get; set; }

        byte[] Color { get; set; }

        T GetText<T>() where T : class;
    }
}
