using System;
using System.Drawing;

namespace ScorpionCore.Plugins
{
    public interface IRenderer : IPlugin
    {
        void Clear(byte red, byte green, byte blue, byte alpha);


        void Start();


        void End();


        void Render(ITexture texture, float x, float y);


        //Angle is in degrees
        void Render(ITexture text, float x, float y, float angle);


        void Render(IText text, float x, float y);


        void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY);
    }
}
