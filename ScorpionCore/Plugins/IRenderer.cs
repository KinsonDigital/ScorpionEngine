namespace ScorpionCore.Plugins
{
    public interface IRenderer : IPlugin
    {
        void Clear(byte red, byte green, byte blue, byte alpha);


        void Start();


        void End();


        void Render(ITexture texture, float x, float y);


        //Angle is in degrees
        void Render(ITexture texture, float x, float y, float angle);


        //Angle is in degrees
        void Render(ITexture texture, float x, float y, float angle, float size, byte red, byte green, byte blue, byte alpha);


        void Render(IText text, float x, float y);


        void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY);
    }
}
