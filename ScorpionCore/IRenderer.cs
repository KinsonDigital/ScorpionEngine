using System;
using System.Drawing;

namespace ScorpionCore
{
    public interface IRenderer
    {
        void Clear(byte red, byte green, byte blue, byte alpha);

        void Render(ITexture texture, float x, float y);

        void Render(IText texture, float x, float y);
    }
}
