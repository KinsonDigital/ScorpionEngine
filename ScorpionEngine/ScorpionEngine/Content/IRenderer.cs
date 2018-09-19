using ScorpionEngine.Core;
using System;
using System.Drawing;

namespace ScorpionEngine.Content
{
    public interface IRenderer
    {
        void Clear(Color color);

        void Render(ITexture texture, Vector position);

        void Render(IText texture, Vector position);
    }
}
