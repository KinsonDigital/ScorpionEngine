using System;
using System.Drawing;

namespace ScorpionCore.Plugins
{
    public interface IRenderer : IPlugin
    {
        void Clear(byte red, byte green, byte blue, byte alpha);

        void Render(ITexture texture, float x, float y);

        void Render(IText texture, float x, float y);
    }
}
